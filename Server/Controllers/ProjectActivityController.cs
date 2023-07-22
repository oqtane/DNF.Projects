using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Oqtane.Shared;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using DNF.Projects.Models;
using DNF.Projects.Repository;
using System;
using Microsoft.AspNetCore.Http;
using Oqtane.Controllers;
using Oqtane.Models;
using System.Net;
using System.Globalization;

namespace DNF.Projects.Controllers
{
    [Route(ControllerRoutes.ApiRoute)]
    public class ProjectActivityController : ModuleControllerBase
    {
        private readonly IProjectRepository _ProjectRepository;
        private readonly IProjectActivityRepository _ProjectActivityRepository;

        public ProjectActivityController(IProjectRepository ProjectRepository, IProjectActivityRepository ProjectActivityRepository, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _ProjectRepository = ProjectRepository;
            _ProjectActivityRepository = ProjectActivityRepository;
        }

        // GET: api/<controller>?projectid=x&fromdate=MMM-dd-yyyy&todate=MMM-dd-yyyy
        [HttpGet]
        [Authorize(Policy = "ViewModule")]
        public IEnumerable<ProjectActivity> Get(string projectid, string fromdate, string todate)
        {
            List<ProjectActivity> activities = new List<ProjectActivity>();
            if (int.TryParse(projectid, out _) && DateTime.TryParseExact(fromdate, "MMM-dd-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime FromDate) && DateTime.TryParseExact(todate, "MMM-dd-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime ToDate))
            {
                foreach (var activity in _ProjectActivityRepository.GetProjectActivity(int.Parse(projectid), FromDate.Date, ToDate.Date))
                {
                    if (activity.Project.ModuleId == _authEntityId[EntityNames.Module])
                    {
                        activities.Add(activity);
                    }
                }
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Project Activity Get Attempt {ProjectId} {FromDate} {ToDate}", projectid, fromdate, todate);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                activities = null;
            }
            return activities;
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Policy = "EditModule")]
        public ProjectActivity Post([FromBody] ProjectActivity ProjectActivity)
        {
            if (ModelState.IsValid)
            {
                Project Project = _ProjectRepository.GetProject(ProjectActivity.ProjectId);
                if (Project != null && Project.ModuleId == _authEntityId[EntityNames.Module])
                {
                    ProjectActivity = _ProjectActivityRepository.AddProjectActivity(ProjectActivity);
                    _logger.Log(LogLevel.Information, this, LogFunction.Create, "Project Activity Added {ProjectActivity}", ProjectActivity);
                }
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Project Activity Post Attempt {ProjectActivity}", ProjectActivity);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                ProjectActivity = null;
            }
            return ProjectActivity;
        }
    }
}
