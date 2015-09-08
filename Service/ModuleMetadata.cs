using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public enum ModuleStatus { Activated = 1, ToBeDeleted = 2 };
    
    public class ModuleMetadata
    {
        public string Name { get; set; }
        public string ModuleName { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public List<string> BDDTableUsed { get; set; }
        public ModuleStatus State { get; set; }

        public ModuleMetadata(string name, string moduleName, string version, string description, string createdBy, List<string> tableUsed)
        {
            Name = name;
            ModuleName = moduleName;
            Version = version;
            Description = description;
            CreatedBy = createdBy;
            BDDTableUsed = tableUsed;
            State = ModuleStatus.Activated;
        }
    }
}
