using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DNF.Projects.Models;

namespace DNF.Projects.Services
{
    public interface IProjectActivityService 
    {
        Task<List<ProjectActivity>> GetProjectActivityAsync(int ProjectId, DateTime FromDate, DateTime ToDate, int ModuleId);

        Task<ProjectActivity> AddProjectActivityAsync(ProjectActivity ProjectActivity, int ModuleId);
    }
}
