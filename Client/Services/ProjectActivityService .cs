using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Oqtane.Modules;
using Oqtane.Services;
using Oqtane.Shared;
using DNF.Projects.Models;
using System;
using System.Globalization;

namespace DNF.Projects.Services
{
    public class ProjectActivityService : ServiceBase, IProjectActivityService, IService
    {
        public ProjectActivityService(HttpClient http, SiteState siteState) : base(http, siteState) { }

        private string ApiUrl => CreateApiUrl("ProjectActivity");

        public async Task<List<ProjectActivity>> GetProjectActivityAsync(int ProjectId, DateTime FromDate, DateTime ToDate, int ModuleId)
        {
            return await GetJsonAsync<List<ProjectActivity>>(CreateAuthorizationPolicyUrl($"{ApiUrl}?projectid={ProjectId}&fromdate={FromDate.ToString("MMM-dd-yyyy", CultureInfo.InvariantCulture)}&todate={ToDate.ToString("MMM-dd-yyyy", CultureInfo.InvariantCulture)}", EntityNames.Module, ModuleId));
        }

        public async Task<ProjectActivity> AddProjectActivityAsync(ProjectActivity ProjectActivity, int ModuleId)
        {
            return await PostJsonAsync<ProjectActivity>(CreateAuthorizationPolicyUrl(ApiUrl, EntityNames.Module, ModuleId), ProjectActivity);
        }

    }
}
