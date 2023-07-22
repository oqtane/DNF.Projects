using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Oqtane.Modules;
using DNF.Projects.Models;

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
    }
}
