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
            Version = "1.0.0",
            ServerManagerType = "DNF.Projects.Manager.ProjectManager, DNF.Projects.Server.Oqtane",
            ReleaseVersions = "1.0.0",
            Dependencies = "DNF.Projects.Shared.Oqtane"
        };
    }
}
