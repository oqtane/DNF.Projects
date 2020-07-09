using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DNF.Projects.Models;

namespace DNF.Projects.Services
{
    public interface IProjectService 
    {
        Task<List<Project>> GetProjectsAsync(int ModuleId);

        Task<Project> GetProjectAsync(int ProjectId, int ModuleId);

        Task<Project> AddProjectAsync(Project Project);

        Task<Project> UpdateProjectAsync(Project Project);

        Task DeleteProjectAsync(int ProjectId, int ModuleId);

        Task<List<ProjectActivity>> GetProjectActivityAsync(int ProjectId, DateTime FromDate, DateTime ToDate, int ModuleId);

        Task<ProjectActivity> AddProjectActivityAsync(ProjectActivity ProjectActivity, int ModuleId);
    }
}
