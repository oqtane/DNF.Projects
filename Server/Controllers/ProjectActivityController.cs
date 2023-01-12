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

namespace DNF.Projects.Controllers
{
    [Route(ControllerRoutes.ApiRoute)]
    public class ProjectActivityController : ModuleControllerBase
    {
        private readonly IProjectRepository _Projects;

        public ProjectActivityController(IProjectRepository Projects, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _Projects = Projects;
        }

        // GET: api/<controller>?projectid=x&fromdate=yyyy-MMM-dd&todate=yyyy-MMM-dd
        [HttpGet]
        [Authorize(Policy = "ViewModule")]
        public IEnumerable<ProjectActivity> Get(string projectid, string fromdate, string todate)
        {
            List<ProjectActivity> activities = new List<ProjectActivity>();
            if (int.TryParse(projectid, out _) && DateTime.TryParse(fromdate, out _) && DateTime.TryParse(todate, out _))
            {
                foreach (var activity in _Projects.GetProjectActivity(int.Parse(projectid), DateTime.Parse(fromdate).Date, DateTime.Parse(todate).Date))
                {
                    if (activity.Project.ModuleId == _authEntityId[EntityNames.Module])
                    {
                        activities.Add(activity);
                    }
                }
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
                Project Project = _Projects.GetProject(ProjectActivity.ProjectId);
                if (Project != null && Project.ModuleId == _authEntityId[EntityNames.Module])
                {
                    ProjectActivity = _Projects.AddProjectActivity(ProjectActivity);
                    _logger.Log(LogLevel.Information, this, LogFunction.Create, "Project Activity Added {ProjectActivity}", ProjectActivity);
                }
            }
            return ProjectActivity;
        }
    }
}
