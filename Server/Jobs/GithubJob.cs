﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading;
using DNF.Projects.Models;
using DNF.Projects.Repository;
using DNF.Projects.Shared;
using Microsoft.Extensions.DependencyInjection;
using Oqtane.Infrastructure;
using Oqtane.Models;
using Oqtane.Repository;
using Oqtane.Shared;
using RestSharp;

namespace DNF.Projects.Jobs
{
    public class GithubJob : HostedServiceBase
    {
        // JobType = "DNF.Projects.Jobs.GithubJob, DNF.Projects.Server.Oqtane"

        public GithubJob(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory) 
        {
            Name = "Github Projects Job";
            Frequency = "d"; // daily
            Interval = 1;
            IsEnabled = false;
        }

        public override string ExecuteJob(IServiceProvider provider)
        {
            string log = "";
            int restrequests = 0;
            int searchrequests = 0;

            // get services which require tenant resolution
            var siteRepository = provider.GetRequiredService<ISiteRepository>();
            var settingRepository = provider.GetRequiredService<ISettingRepository>();
            var projectRepository = provider.GetRequiredService<IProjectRepository>();
            var projectActivityRepository = provider.GetRequiredService<IProjectActivityRepository>();
            var notificationRepository = provider.GetRequiredService<INotificationRepository>();

            // iterate through sites for this tenant
            List<Site> sites = siteRepository.GetSites().ToList();
            foreach (Site site in sites)
            {
                List<Project> projects = projectRepository.GetProjects(-1, site.SiteId).ToList();
                foreach (Project project in projects)
                {
                    log += "Processing Project: " + project.Url;

                    // get module settings
                    List<Setting> modulesettings = settingRepository.GetSettings(EntityNames.Module, project.ModuleId).ToList();
                    Dictionary<string, string> settings = GetSettings(modulesettings);
                    if (settings.ContainsKey("GithubToken"))
                    {
                        // search rate limit ( 30 requests per minute )
                        if (searchrequests == 28)
                        {
                            Thread.Sleep(60 * 1000); // 60 seconds
                            searchrequests = 0;
                        }

                        string resource = project.Url.Replace(Common.UrlPrefix, "");

                        var activity = new ProjectActivity();
                        activity.ProjectId = project.ProjectId;
                        activity.Date = DateTime.Now.Date;

                        // get metrics from previous day to initialize
                        var yesterday = projectActivityRepository.GetProjectActivity(project.ProjectId, activity.Date.AddDays(-1).Date, activity.Date.Date).FirstOrDefault();
                        if (yesterday != null)
                        {
                            activity.Stars = yesterday.Stars;
                            activity.Forks = yesterday.Forks;
                            activity.Watchers = yesterday.Watchers;
                            activity.Contributors = yesterday.Contributors;
                            activity.Commits = yesterday.Commits;
                            activity.Issues = yesterday.Issues;
                            activity.PullRequests = yesterday.PullRequests;
                        }

                        var client = new RestClient("https://api.github.com/");

                        RestRequest request = null;
                        IRestResponse response = null;
                        System.Text.Json.Nodes.JsonObject jObject;
                        System.Text.Json.Nodes.JsonArray jArray;
                        bool error = false;

                        // get stars, forks, watchers
                        try
                        {
                            request = new RestRequest("repos/" + resource);
                            request.AddHeader("Authorization", "Bearer " + settings["GithubToken"]);
                            response = client.Execute(request);
                            jObject = JsonNode.Parse(response.Content).AsObject();
                            activity.Stars = int.Parse(jObject["stargazers_count"].ToString());
                            activity.Forks = int.Parse(jObject["forks_count"].ToString());
                            activity.Watchers = int.Parse(jObject["subscribers_count"].ToString());
                            restrequests += 1;
                        }
                        catch (Exception ex)
                        {
                            error = true;
                            log += "<br /> Url: " + request.Resource + " Error: " + ex.Message + " REST Requests: " + restrequests.ToString();
                        }

                        // get contributors, commits
                        try
                        {
                            int contributors = 0;
                            int commits = 0;
                            int Page = 1;
                            while (Page != -1)
                            {
                                request = new RestRequest("repos/" + resource + "/contributors?anon=true&per_page=100&page=" + Page.ToString());
                                request.AddHeader("Authorization", "Bearer " + settings["GithubToken"]);
                                response = client.Execute(request);
                                jArray = JsonNode.Parse(response.Content).AsArray();
                                if (jArray.Count > 0)
                                {
                                    foreach (System.Text.Json.Nodes.JsonObject contributor in jArray)
                                    {
                                        contributors += 1;
                                        commits += int.Parse(contributor["contributions"].ToString());
                                    }
                                    Page += 1;
                                }
                                else
                                {
                                    Page = -1;
                                }
                                restrequests += 1;
                            }
                            activity.Contributors = contributors;
                            activity.Commits = commits;
                        }
                        catch (Exception ex)
                        {
                            error = true;
                            log += "<br /> Url: " + request.Resource + " Error: " + ex.Message + " REST Requests: " + restrequests.ToString();
                        }

                        // get issues
                        try
                        {
                            request = new RestRequest("search/issues?q=repo:" + resource + "+type:issue+state:open");
                            request.AddHeader("Authorization", "Bearer " + settings["GithubToken"]);
                            response = client.Execute(request);
                            jObject = JsonNode.Parse(response.Content).AsObject();
                            activity.Issues = int.Parse(jObject["total_count"].ToString());

                            request = new RestRequest("search/issues?q=repo:" + resource + "+type:issue+state:closed");
                            request.AddHeader("Authorization", "Bearer " + settings["GithubToken"]);
                            response = client.Execute(request);
                            jObject = JsonNode.Parse(response.Content).AsObject();
                            activity.Issues += int.Parse(jObject["total_count"].ToString());

                            searchrequests += 2;
                            restrequests += 2;
                        }
                        catch (Exception ex)
                        {
                            error = true;
                            log += "<br /> Url: " + request.Resource + " Error: " + ex.Message + " REST Requests: " + restrequests.ToString() + " Search Requests: " + searchrequests.ToString();
                        }

                        // get pull requests
                        try
                        {
                            request = new RestRequest("search/issues?q=repo:" + resource + "+type:pr+state:open");
                            request.AddHeader("Authorization", "Bearer " + settings["GithubToken"]);
                            response = client.Execute(request);
                            jObject = JsonNode.Parse(response.Content).AsObject();
                            activity.PullRequests = int.Parse(jObject["total_count"].ToString());

                            request = new RestRequest("search/issues?q=repo:" + resource + "+type:pr+state:closed");
                            request.AddHeader("Authorization", "Bearer " + settings["GithubToken"]);
                            response = client.Execute(request);
                            jObject = JsonNode.Parse(response.Content).AsObject();
                            activity.PullRequests += int.Parse(jObject["total_count"].ToString());

                            searchrequests += 2;
                            restrequests += 2;
                        }
                        catch (Exception ex)
                        {
                            error = true;
                            log += "<br /> Url: " + request.Resource + " Error: " + ex.Message + " REST Requests: " + restrequests.ToString() + " Search Requests: " + searchrequests.ToString();
                        }

                        // get downloads
                        if (!string.IsNullOrEmpty(project.Package))
                        {
                            try
                            {
                                client = new RestClient("https://azuresearch-usnc.nuget.org/");
                                request = new RestRequest("query?q=" + project.Package);
                                response = client.Execute(request);
                                jObject = JsonNode.Parse(response.Content).AsObject();
                                if (jObject != null)
                                {
                                    jArray = jObject["data"].AsArray();
                                    foreach (System.Text.Json.Nodes.JsonObject data in jArray)
                                    {
                                        if (data["id"].ToString().ToLower() == project.Package.ToLower())
                                        {
                                            if (int.TryParse(data["totalDownloads"].ToString(), out int downloads))
                                            {
                                                activity.Downloads = downloads;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                error = true;
                                log += "<br /> Url: " + request.Resource + " Error: " + ex.Message;
                            }
                        }

                        projectActivityRepository.AddProjectActivity(activity);

                        if (error && settings.ContainsKey("NotifyName") && settings.ContainsKey("NotifyEmail"))
                        {
                            var notification = new Notification(site.SiteId, "", "", settings["NotifyName"], settings["NotifyEmail"], "GitHub Project API Error", "Url: " + project.Url);
                            notificationRepository.AddNotification(notification);
                        }

                        log += " - Succeeded (" + DateTime.UtcNow.ToString("HH:mm:ss:fff") + ")";
                    }
                    else
                    {
                        log += " - Failed: Github Token Not Specified In Module Settings";
                    }
                    log += "<br />";
                }
            }

            return log;
        }


        private Dictionary<string, string> GetSettings(List<Setting> settings)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (Setting setting in settings.OrderBy(item => item.SettingName).ToList())
            {
                dictionary.Add(setting.SettingName, setting.SettingValue);
            }
            return dictionary;
        }
    }
}