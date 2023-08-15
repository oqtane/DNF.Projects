using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Oqtane.Modules;
using Oqtane.Services;
using Oqtane.Shared;
using DNF.Projects.Models;

namespace DNF.Projects.Services
{
    public class ProjectService : ServiceBase, IProjectService, IService
    {
        public ProjectService(HttpClient http, SiteState siteState) : base(http, siteState) { }

        private string ApiUrl => CreateApiUrl("Project");

        public async Task<List<Project>> GetProjectsAsync(int ModuleId)
        {
            List<Project> Projects = await GetJsonAsync<List<Project>>(CreateAuthorizationPolicyUrl($"{ApiUrl}?moduleid={ModuleId}", EntityNames.Module, ModuleId), Enumerable.Empty<Project>().ToList());
            return Projects.OrderBy(item => item.Url).ToList();
        }

        public async Task<Project> GetProjectAsync(int ProjectId, int ModuleId)
        {
            return await GetJsonAsync<Project>(CreateAuthorizationPolicyUrl($"{ApiUrl}/{ProjectId}", EntityNames.Module, ModuleId));
        }

        public async Task<Project> AddProjectAsync(Project Project)
        {
            return await PostJsonAsync<Project>(CreateAuthorizationPolicyUrl(ApiUrl, EntityNames.Module, Project.ModuleId), Project);
        }

        public async Task<Project> UpdateProjectAsync(Project Project)
        {
            return await PutJsonAsync<Project>(CreateAuthorizationPolicyUrl($"{ApiUrl}/{Project.ProjectId}", EntityNames.Module, Project.ModuleId), Project);
        }

        public async Task DeleteProjectAsync(int ProjectId, int ModuleId)
        {
            await DeleteAsync(CreateAuthorizationPolicyUrl($"{ApiUrl}/{ProjectId}", EntityNames.Module, ModuleId));
        }
    }
}
