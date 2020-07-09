using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Oqtane.Modules;
using DNF.Projects.Models;
using System;

namespace DNF.Projects.Repository
{
    public class ProjectRepository : IProjectRepository, IService
    {
        private readonly ProjectContext _db;

        public ProjectRepository(ProjectContext context)
        {
            _db = context;
        }

        public IEnumerable<Project> GetProjects(int ModuleId, int SiteId)
        {
            return _db.Project.Where(item => item.ModuleId == ModuleId || (ModuleId == -1 && item.SiteId == SiteId));
        }

        public Project GetProject(int ProjectId)
        {
            return _db.Project.Find(ProjectId);
        }

        public Project AddProject(Project Project)
        {
            _db.Project.Add(Project);
            _db.SaveChanges();
            return Project;
        }

        public Project UpdateProject(Project Project)
        {
            _db.Entry(Project).State = EntityState.Modified;
            _db.SaveChanges();
            return Project;
        }

        public void DeleteProject(int ProjectId)
        {
            Project Project = _db.Project.Find(ProjectId);
            _db.Project.Remove(Project);
            _db.SaveChanges();
        }

        public IEnumerable<ProjectActivity> GetProjectActivity(int ProjectId, DateTime FromDate, DateTime ToDate)
        {
            return _db.ProjectActivity
                .Include(item => item.Project) // eager load projects
                .Where(item => (ProjectId == -1 || item.ProjectId == ProjectId) && item.Date >= FromDate && item.Date <= ToDate);
        }

        public ProjectActivity AddProjectActivity(ProjectActivity ProjectActivity)
        {
            ProjectActivity.Date = DateTime.Parse(ProjectActivity.Date.ToShortDateString());
            ProjectActivity activity = _db.ProjectActivity.FirstOrDefault(item => item.ProjectId == ProjectActivity.ProjectId && item.Date == ProjectActivity.Date);
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
