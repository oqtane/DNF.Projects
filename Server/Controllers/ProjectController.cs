using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Oqtane.Shared;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using DNF.Projects.Models;
using DNF.Projects.Repository;
using Microsoft.AspNetCore.Http;
using Oqtane.Controllers;

namespace DNF.Projects.Controllers
{
    [Route(ControllerRoutes.ApiRoute)]
    public class ProjectController : ModuleControllerBase
    {
        private readonly IProjectRepository _Projects;

        public ProjectController(IProjectRepository Projects, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _Projects = Projects;
        }

        // GET: api/<controller>?moduleid=x
        [HttpGet]
        [Authorize(Policy = "ViewModule")]
        public IEnumerable<Project> Get(string moduleid)
        {
            if (int.Parse(moduleid) == _authEntityId[EntityNames.Module])
            {
                return _Projects.GetProjects(int.Parse(moduleid), -1);
            }
            else
            {
                return null;
            }
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        [Authorize(Policy = "ViewModule")]
        public Project Get(int id)
        {
            Project Project = _Projects.GetProject(id);
            if (Project != null && Project.ModuleId != _authEntityId[EntityNames.Module])
            {
                Project = null;
            }
            return Project;
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Policy = "EditModule")]
        public Project Post([FromBody] Project Project)
        {
            if (ModelState.IsValid && Project.ModuleId == _authEntityId[EntityNames.Module])
            {
                Project = _Projects.AddProject(Project);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "Project Added {Project}", Project);
            }
            return Project;
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Policy = "EditModule")]
        public Project Put(int id, [FromBody] Project Project)
        {
            if (ModelState.IsValid && Project.ModuleId == _authEntityId[EntityNames.Module])
            {
                Project = _Projects.UpdateProject(Project);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "Project Updated {Project}", Project);
            }
            return Project;
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "EditModule")]
        public void Delete(int id)
        {
            Project Project = _Projects.GetProject(id);
            if (Project != null && Project.ModuleId ==_authEntityId[EntityNames.Module])
            {
                _Projects.DeleteProject(id);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Project Deleted {ProjectId}", id);
            }
        }
    }
}
