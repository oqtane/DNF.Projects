using System;
using System.Collections.Generic;
using System.Linq;
using DNF.Projects.Models;
using DNF.Projects.Repository;
using DNF.Projects.Shared;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Oqtane.Infrastructure;
using Oqtane.Models;
using Oqtane.Repository;
using Oqtane.Shared;
using RestSharp;
using RestSharp.Authenticators;

namespace DNF.Projects.Jobs
{
    public class GithubJob : HostedServiceBase
    {
        // JobType = "DNF.Projects.Jobs.GithubJob, DNF.Projects.Server.Oqtane"

        public GithubJob(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory) { }

        public override string ExecuteJob(IServiceProvider provider)
        {
            string log = "";

            // iterate through aliases in this installation
            var aliasRepository = provider.GetRequiredService<IAliasRepository>();
            List<Alias> aliases = aliasRepository.GetAliases().ToList();
            foreach (Alias alias in aliases)
            {
                // use the SiteState to set the Alias explicitly so the tenant can be resolved
                var siteState = provider.GetRequiredService<SiteState>();
                siteState.Alias = alias;

                // get services which require tenant resolution
                var siteRepository = provider.GetRequiredService<ISiteRepository>();
                var settingRepository = provider.GetRequiredService<ISettingRepository>();
                var projectRepository = provider.GetRequiredService<IProjectRepository>();

                // iterate through sites 
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
                        if (settings.ContainsKey("GithubUsername") && settings.ContainsKey("GithubPassword"))
                        {
                            try
                            {
                                string resource = project.Url.Replace(Common.UrlPrefix, "");

                                var activity = new ProjectActivity();
                                activity.ProjectId = project.ProjectId;
                                activity.Date = DateTime.Now;

                                var client = new RestClient("https://api.github.com/");
                                client.Authenticator = new HttpBasicAuthenticator(settings["GithubUsername"], settings["GithubPassword"]);

                                JObject jObject;
                                JArray jArray;

                                var request = new RestRequest("repos/" + resource);
                                var response = client.Execute(request);
                                if (!string.IsNullOrEmpty(response.Content))
                                {
                                    jObject = JObject.Parse(response.Content);
                                    activity.Stars = int.Parse(jObject.GetValue("stargazers_count").ToString());
                                    activity.Forks = int.Parse(jObject.GetValue("forks_count").ToString());
                                    activity.Watchers = int.Parse(jObject.GetValue("subscribers_count").ToString());
                                }

                                int Contributors = 0;
                                int Commits = 0;
                                int Retries = 0;
                                request = new RestRequest("repos/" + resource + "/stats/contributors");
                                response = client.Execute(request);
                                while ((string.IsNullOrEmpty(response.Content) || response.Content == "{}") && Retries < 5)
                                {
                                    response = client.Execute(request);
                                    Retries += 1;
                                }
                                if (!string.IsNullOrEmpty(response.Content))
                                {
                                    jArray = JArray.Parse(response.Content);
                                    foreach (JObject contributor in jArray)
                                    {
                                        Contributors += 1;
                                        Commits = int.Parse(contributor.GetValue("total").ToString());
                                    }
                                }
                                activity.Contributors = Contributors;
                                activity.Commits = Commits;

                                request = new RestRequest("search/issues?q=repo:" + resource + "+type:issue+state:open");
                                response = client.Execute(request);
                                if (!string.IsNullOrEmpty(response.Content))
                                {
                                    jObject = JObject.Parse(response.Content);
                                    activity.Issues = int.Parse(jObject.GetValue("total_count").ToString());
                                }

                                request = new RestRequest("search/issues?q=repo:" + resource + "+type:issue+state:closed");
                                response = client.Execute(request);
                                if (!string.IsNullOrEmpty(response.Content))
                                {
                                    jObject = JObject.Parse(response.Content);
                                    activity.Issues += int.Parse(jObject.GetValue("total_count").ToString());
                                }

                                request = new RestRequest("search/issues?q=repo:" + resource + "+type:pr+state:open");
                                response = client.Execute(request);
                                if (!string.IsNullOrEmpty(response.Content))
                                {
                                    jObject = JObject.Parse(response.Content);
                                    activity.PullRequests = int.Parse(jObject.GetValue("total_count").ToString());
                                }

                                request = new RestRequest("search/issues?q=repo:" + resource + "+type:pr+state:closed");
                                response = client.Execute(request);
                                if (!string.IsNullOrEmpty(response.Content))
                                {
                                    jObject = JObject.Parse(response.Content);
                                    activity.PullRequests += int.Parse(jObject.GetValue("total_count").ToString());
                                }

                                projectRepository.AddProjectActivity(activity);

                                log += " - Succeeded";
                            }
                            catch (Exception ex)
                            {
                                log += " - Failed: " + ex.Message.ToString();
                            }
                        }
                        else
                        {
                            log += " - Failed: Github Username/Password Not Specified";
                        }
                        log += "<br />";
                    }
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