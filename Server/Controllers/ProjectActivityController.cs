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

namespace DNF.Projects.Controllers
{
    [Route("{site}/api/[controller]")]
    public class ProjectActivityController : Controller
    {
        private readonly IProjectRepository _Projects;
        private readonly ILogManager _logger;
        protected int _entityId = -1; // passed as a querystring parameter for policy authorization and used for validation

        public ProjectActivityController(IProjectRepository Projects, ILogManager logger, IHttpContextAccessor accessor)
        {
            _Projects = Projects;
            _logger = logger;

            if (accessor.HttpContext.Request.Query.ContainsKey("entityid"))
            {
                _entityId = int.Parse(accessor.HttpContext.Request.Query["entityid"]);
            }
        }

        // GET: api/<controller>?projectid=x&fromdate=yyyy-MMM-dd&todate=yyyy-MMM-dd
        [HttpGet]
        [Authorize(Policy = "ViewModule")]
        public IEnumerable<ProjectActivity> Get(string projectid, string fromdate, string todate)
        {
            List<ProjectActivity> activities = new List<ProjectActivity>();
            foreach(var activity in _Projects.GetProjectActivity(int.Parse(projectid), DateTime.Parse(fromdate), DateTime.Parse(todate)))
            {
                if (activity.Project.ModuleId == _entityId)
                {
                    activities.Add(activity);
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
                if (Project != null && Project.ModuleId == _entityId)
                {
                    ProjectActivity = _Projects.AddProjectActivity(ProjectActivity);
                    _logger.Log(LogLevel.Information, this, LogFunction.Create, "Project Activity Added {ProjectActivity}", ProjectActivity);
                }
            }
            return ProjectActivity;
        }
    }
}
