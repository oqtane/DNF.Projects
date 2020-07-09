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
            List<Project> Projects = await GetJsonAsync<List<Project>>(CreateAuthorizationPolicyUrl(CreateApiUrl(_siteState.Alias, "Project") + $"?moduleid={ModuleId}", ModuleId));
            return Projects.OrderBy(item => item.Url).ToList();
        }

        public async Task<Project> GetProjectAsync(int ProjectId, int ModuleId)
        {
            return await GetJsonAsync<Project>(CreateAuthorizationPolicyUrl(CreateApiUrl(_siteState.Alias, "Project") + $"/{ProjectId}", ModuleId));
        }

        public async Task<Project> AddProjectAsync(Project Project)
        {
            return await PostJsonAsync<Project>(CreateAuthorizationPolicyUrl(CreateApiUrl(_siteState.Alias, "Project"), Project.ModuleId), Project);
        }

        public async Task<Project> UpdateProjectAsync(Project Project)
        {
            return await PutJsonAsync<Project>(CreateAuthorizationPolicyUrl(CreateApiUrl(_siteState.Alias, "Project") + $"/{Project.ProjectId}", Project.ModuleId), Project);
        }

        public async Task DeleteProjectAsync(int ProjectId, int ModuleId)
        {
            await DeleteAsync(CreateAuthorizationPolicyUrl(CreateApiUrl(_siteState.Alias, "Project") + $"/{ProjectId}", ModuleId));
        }

        public async Task<List<ProjectActivity>> GetProjectActivityAsync(int ProjectId, DateTime FromDate, DateTime ToDate, int ModuleId)
        {
            return await GetJsonAsync<List<ProjectActivity>>(CreateAuthorizationPolicyUrl(CreateApiUrl(_siteState.Alias, "ProjectActivity") + $"?projectid={ProjectId}&fromdate={FromDate:yyyy-MMM-dd}&todate={ToDate:yyyy-MMM-dd}", ModuleId));
        }

        public async Task<ProjectActivity> AddProjectActivityAsync(ProjectActivity ProjectActivity, int ModuleId)
        {
            return await PostJsonAsync<ProjectActivity>(CreateAuthorizationPolicyUrl(CreateApiUrl(_siteState.Alias, "ProjectActivity"), ModuleId), ProjectActivity);
        }

        // add entityid parameter to url for custom authorization policy
        private string CreateAuthorizationPolicyUrl(string url, int entityId)
        {
            if (url.Contains("?"))
            {
                return url + "&entityid=" + entityId.ToString();
            }
            else
            {
                return url + "?entityid=" + entityId.ToString();
            }
        }

    }
}
