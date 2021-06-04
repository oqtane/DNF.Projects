using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Oqtane.Modules;
using Oqtane.Services;
using Oqtane.Shared;
using DNF.Projects.Models;
using System;

namespace DNF.Projects.Services
{
    public class ProjectService : ServiceBase, IProjectService, IService
    {
        private readonly SiteState _siteState;

        public ProjectService(HttpClient http, SiteState siteState) : base(http)
        {
            _siteState = siteState;
        }

        public async Task<List<Project>> GetProjectsAsync(int ModuleId)
        {
            List<Project> Projects = await GetJsonAsync<List<Project>>(CreateAuthorizationPolicyUrl(CreateApiUrl("Project", _siteState.Alias) + $"?moduleid={ModuleId}", EntityNames.Module, ModuleId));
            return Projects.OrderBy(item => item.Url).ToList();
        }

        public async Task<Project> GetProjectAsync(int ProjectId, int ModuleId)
        {
            return await GetJsonAsync<Project>(CreateAuthorizationPolicyUrl(CreateApiUrl("Project", _siteState.Alias) + $"/{ProjectId}", EntityNames.Module, ModuleId));
        }

        public async Task<Project> AddProjectAsync(Project Project)
        {
            return await PostJsonAsync<Project>(CreateAuthorizationPolicyUrl(CreateApiUrl("Project", _siteState.Alias), EntityNames.Module, Project.ModuleId), Project);
        }

        public async Task<Project> UpdateProjectAsync(Project Project)
        {
            return await PutJsonAsync<Project>(CreateAuthorizationPolicyUrl(CreateApiUrl("Project", _siteState.Alias) + $"/{Project.ProjectId}", EntityNames.Module, Project.ModuleId), Project);
        }

        public async Task DeleteProjectAsync(int ProjectId, int ModuleId)
        {
            await DeleteAsync(CreateAuthorizationPolicyUrl(CreateApiUrl("Project", _siteState.Alias) + $"/{ProjectId}", EntityNames.Module, ModuleId));
        }

        public async Task<List<ProjectActivity>> GetProjectActivityAsync(int ProjectId, DateTime FromDate, DateTime ToDate, int ModuleId)
        {
            return await GetJsonAsync<List<ProjectActivity>>(CreateAuthorizationPolicyUrl(CreateApiUrl("ProjectActivity", _siteState.Alias) + $"?projectid={ProjectId}&fromdate={FromDate:yyyy-MMM-dd}&todate={ToDate:yyyy-MMM-dd}", EntityNames.Module, ModuleId));
        }

        public async Task<ProjectActivity> AddProjectActivityAsync(ProjectActivity ProjectActivity, int ModuleId)
        {
            return await PostJsonAsync<ProjectActivity>(CreateAuthorizationPolicyUrl(CreateApiUrl("ProjectActivity", _siteState.Alias), EntityNames.Module, ModuleId), ProjectActivity);
        }

    }
}
