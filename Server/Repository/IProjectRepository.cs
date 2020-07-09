using System;
using System.Collections.Generic;
using DNF.Projects.Models;

namespace DNF.Projects.Repository
{
    public interface IProjectRepository
    {
        IEnumerable<Project> GetProjects(int ModuleId, int SiteId);
        Project GetProject(int ProjectId);
        Project AddProject(Project Project);
        Project UpdateProject(Project Project);
        void DeleteProject(int ProjectId);

        IEnumerable<ProjectActivity> GetProjectActivity(int ProjectId, DateTime FromDate, DateTime ToDate);
        ProjectActivity AddProjectActivity(ProjectActivity ProjectActivity);

    }
}
