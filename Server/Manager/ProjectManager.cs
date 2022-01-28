using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Oqtane.Modules;
using Oqtane.Models;
using Oqtane.Infrastructure;
using Oqtane.Repository;
using DNF.Projects.Models;
using DNF.Projects.Repository;

namespace DNF.Projects.Manager
{
    public class ProjectManager : IInstallable, IPortable
    {
        private IProjectRepository _Projects;
        private ISqlRepository _sql;

        public ProjectManager(IProjectRepository Projects, ISqlRepository sql)
        {
            _Projects = Projects;
            _sql = sql;
        }

        public bool Install(Tenant tenant, string version)
        {
            return _sql.ExecuteScript(tenant, GetType().Assembly, "DNF.Projects." + version + ".sql");
        }

        public bool Uninstall(Tenant tenant)
        {
            return _sql.ExecuteScript(tenant, GetType().Assembly, "DNF.Projects.Uninstall.sql");
        }

        public string ExportModule(Module module)
        {
            string content = "";
            List<Project> Projects = _Projects.GetProjects(module.ModuleId, -1).ToList();
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
                var projects = _Projects.GetProjects(-1, module.SiteId);
                foreach(Project Project in Projects)
                {
                    var project = projects.FirstOrDefault(item => item.Url == Project.Url);
                    if (project == null)
                    {
                        Project _Project = new Project();
                        _Project.ModuleId = module.ModuleId;
                        _Project.SiteId = module.SiteId;
                        _Project.Url = Project.Url;
                        _Project.Category = Project.Category;
                        project = _Projects.AddProject(_Project);
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
                        _Projects.AddProjectActivity(activity);
                    }
                }
            }
        }
    }
}