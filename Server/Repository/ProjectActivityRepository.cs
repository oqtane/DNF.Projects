using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Oqtane.Modules;
using DNF.Projects.Models;
using System;

namespace DNF.Projects.Repository
{
    public class ProjectActivityRepository : IProjectActivityRepository, IService
    {
        private readonly ProjectContext _db;

        public ProjectActivityRepository(ProjectContext context)
        {
            _db = context;
        }

        public IEnumerable<ProjectActivity> GetProjectActivity(int ProjectId, DateTime FromDate, DateTime ToDate)
        {
            return _db.ProjectActivity
                .Include(item => item.Project) // eager load projects
                .Where(item => (ProjectId == -1 || item.ProjectId == ProjectId) && item.Date >= FromDate && item.Date <= ToDate);
        }

        public ProjectActivity AddProjectActivity(ProjectActivity ProjectActivity)
        {
            ProjectActivity activity = _db.ProjectActivity.AsNoTracking().FirstOrDefault(item => item.ProjectId == ProjectActivity.ProjectId && item.Date.Date == ProjectActivity.Date.Date);
            if (activity == null)
            {
                _db.ProjectActivity.Add(ProjectActivity);
                _db.SaveChanges();
            }
            else
            {
                _db.Entry(ProjectActivity).State = EntityState.Modified;
                _db.SaveChanges();
            }
            return ProjectActivity;
        }
    }
}
