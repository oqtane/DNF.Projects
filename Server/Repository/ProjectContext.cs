using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Oqtane.Modules;
using Oqtane.Repository;
using DNF.Projects.Models;
using Oqtane.Infrastructure;

namespace DNF.Projects.Repository
{
    public class ProjectContext : DBContextBase, IService
    {
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<ProjectActivity> ProjectActivity { get; set; }

        public ProjectContext(ITenantManager tenantManager, IHttpContextAccessor accessor) : base(tenantManager, accessor)
        {
            // ContextBase handles multi-tenant database connections
        }
    }
}
