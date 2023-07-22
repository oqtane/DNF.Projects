using Oqtane.Models;
using Oqtane.Modules;

namespace DNF.Projects
{
    public class ModuleInfo : IModule
    {
        public ModuleDefinition ModuleDefinition => new ModuleDefinition
        {
            Name = "Project Trends",
            Description = "GitHub Project Trends",
            Version = "1.0.6",
            ReleaseVersions = "1.0.0,1.0.1,1.0.2,1.0.3,1.0.4,1.0.5,1.0.6",
            ServerManagerType = "DNF.Projects.Manager.ProjectManager, DNF.Projects.Server.Oqtane",
            SettingsType = "DNF.Projects.Settings, DNF.Projects.Client.Oqtane",
            Dependencies = "DNF.Projects.Shared.Oqtane"
        };
    }
}
