using Oqtane.Models;
using Oqtane.Modules;

namespace DNF.Projects
{
    public class ModuleInfo : IModule
    {
        public ModuleDefinition ModuleDefinition => new ModuleDefinition
        {
            Name = "Project",
            Description = "Project",
            Version = "1.0.5",
            ServerManagerType = "DNF.Projects.Manager.ProjectManager, DNF.Projects.Server.Oqtane",
            ReleaseVersions = "1.0.0,1.0.1,1.0.2,1.0.3,1.0.4,1.0.5",
            SettingsType = "DNF.Projects.Settings, DNF.Projects.Client.Oqtane",
            Dependencies = "DNF.Projects.Shared.Oqtane"
        };
    }
}
