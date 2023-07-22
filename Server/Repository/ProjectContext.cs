using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Oqtane.Modules;
using Oqtane.Repository;
using DNF.Projects.Models;
using Oqtane.Repository.Databases.Interfaces;

namespace DNF.Projects.Repository
{
    public class ProjectContext : DBContextBase, IService, IMultiDatabase
    {
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<ProjectActivity> ProjectActivity { get; set; }

        public ProjectContext(IDBContextDependencies DBContextDependencies) : base(DBContextDependencies)
        {
            // ContextBase handles multi-tenant database connections
        }
    }
}
