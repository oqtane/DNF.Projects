using System;
using System.Collections.Generic;
using DNF.Projects.Models;

namespace DNF.Projects.Repository
{
    public interface IProjectActivityRepository
    {
        IEnumerable<ProjectActivity> GetProjectActivity(int ProjectId, DateTime FromDate, DateTime ToDate);
        ProjectActivity AddProjectActivity(ProjectActivity ProjectActivity);
    }
}
