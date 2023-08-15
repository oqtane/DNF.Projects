using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Oqtane.Modules;
using Oqtane.Models;
using Oqtane.Infrastructure;
using Oqtane.Repository;
using DNF.Projects.Models;
using DNF.Projects.Repository;
using DNF.Projects.Shared;
using Oqtane.Enums;
using Oqtane.Migrations.Framework;
using Oqtane.Shared;

namespace DNF.Projects.Manager
{
    public class ProjectManager : MigratableModuleBase, IInstallable, IPortable
    {
        private readonly IProjectRepository _ProjectRepository;
        private readonly IProjectActivityRepository _ProjectActivityRepository;
        private readonly IDBContextDependencies _DBContextDependencies;
        private readonly ISqlRepository _SqlRepository;

        public ProjectManager(IProjectRepository ProjectRepository, IProjectActivityRepository ProjectActivityRepository, IDBContextDependencies DBContextDependencies, ISqlRepository SqlRepository)
        {
            _ProjectRepository = ProjectRepository;
            _ProjectActivityRepository = ProjectActivityRepository;
            _DBContextDependencies = DBContextDependencies;
            _SqlRepository = SqlRepository;
        }

        public bool Install(Tenant tenant, string version)
        {
            if (tenant.DBType == Constants.DefaultDBType && version == "1.0.6")
            {
                // earlier versions used SQL scripts rather than migrations, so we need to seed the migration history table
                _SqlRepository.ExecuteNonQuery(tenant, MigrationUtils.BuildInsertScript("DNF.Projects.01.00.00.00"));
            }
            return Migrate(new ProjectContext(_DBContextDependencies), tenant, MigrationType.Up);
        }

        public bool Uninstall(Tenant tenant)
        {
            return Migrate(new ProjectContext(_DBContextDependencies), tenant, MigrationType.Down);
        }

        public string ExportModule(Module module)
        {
            string content = "";
            List<Project> Projects = _ProjectRepository.GetProjects(module.ModuleId, -1).ToList();
            if (Projects != null)
            {
                content = JsonSerializer.Serialize(Projects);
            }
            return content;
        }

        public void ImportModule(Module module, string content, string version)
        {
            List<Project> Projects = null;
            if (!string.IsNullOrEmpty(content))
            {
                Projects = JsonSerializer.Deserialize<List<Project>>(content);
            }
            if (Projects != null)
            {
                var projects = _ProjectRepository.GetProjects(-1, module.SiteId);
                foreach(Project Project in Projects)
                {
                    var project = projects.FirstOrDefault(item => item.Url == Project.Url);
                    if (project == null)
                    {
                        Project _Project = new Project();
                        _Project.ModuleId = module.ModuleId;
                        _Project.SiteId = module.SiteId;
                        _Project.Url = Project.Url;
                        _Project.Package = Project.Package;
                        _Project.Title = Common.TruncateString(Project.Title, 50);
                        _Project.Description = Common.TruncateString(Project.Description, 500);
                        _Project.Category = Common.TruncateString(Project.Category, 50);
                        _Project.IsActive = Project.IsActive;
                        project = _ProjectRepository.AddProject(_Project);
                    }
                    if (Project.Date != null)
                    {
                        ProjectActivity activity = new ProjectActivity();
                        activity.ProjectId = project.ProjectId;
                        activity.Date = Project.Date.Value.Date;
                        activity.Watchers = Project.Watchers;
                        activity.Stars = Project.Stars;
                        activity.Forks = Project.Forks;
                        activity.Contributors = Project.Contributors;
                        activity.Commits = Project.Commits;
                        activity.Issues = Project.Issues;
                        activity.PullRequests = Project.PullRequests;
                        activity.Downloads = Project.Downloads;
                        _ProjectActivityRepository.AddProjectActivity(activity);
                    }
                }
            }
        }
    }
}