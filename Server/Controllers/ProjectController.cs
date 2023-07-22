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
using Oqtane.Models;
using System.Net;

namespace DNF.Projects.Controllers
{
    [Route(ControllerRoutes.ApiRoute)]
    public class ProjectController : ModuleControllerBase
    {
        private readonly IProjectRepository _ProjectRepository;

        public ProjectController(IProjectRepository ProjectRepository, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _ProjectRepository = ProjectRepository;
        }

        // GET: api/<controller>?moduleid=x
        [HttpGet]
        [Authorize(Policy = "ViewModule")]
        public IEnumerable<Project> Get(string moduleid)
        {
            if (int.Parse(moduleid) == _authEntityId[EntityNames.Module])
            {
                return _ProjectRepository.GetProjects(int.Parse(moduleid), -1);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Project Get Attempt {ModuleId}", moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        [Authorize(Policy = "ViewModule")]
        public Project Get(int id)
        {
            Project Project = _ProjectRepository.GetProject(id);
            if (Project != null && Project.ModuleId == _authEntityId[EntityNames.Module])
            {
                return Project;
            }
            else
            {
                if (Project != null)
                {
                    _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Project Get Attempt {ProjectId}", id);
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                }
                else
                {
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                }
                return null;
            }
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Policy = "EditModule")]
        public Project Post([FromBody] Project Project)
        {
            if (ModelState.IsValid && Project.ModuleId == _authEntityId[EntityNames.Module])
            {
                Project = _ProjectRepository.AddProject(Project);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "Project Added {Project}", Project);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Project Post Attempt {Project}", Project);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                Project = null;
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
                Project = _ProjectRepository.UpdateProject(Project);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "Project Updated {Project}", Project);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Project Put Attempt {Project}", Project);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                Project = null;
            }
            return Project;
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "EditModule")]
        public void Delete(int id)
        {
            Project Project = _ProjectRepository.GetProject(id);
            if (Project != null && Project.ModuleId ==_authEntityId[EntityNames.Module])
            {
                _ProjectRepository.DeleteProject(id);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Project Deleted {ProjectId}", id);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Project Delete Attempt {ProjectId}", id);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
        }
    }
}
