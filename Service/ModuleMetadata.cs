using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ModuleMetadata
    {
        public string Name { get; set; }
        public string ModuleName { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }

        public ModuleMetadata(string name, string moduleName, string version, string description, string createdBy)
        {
            Name = name;
            ModuleName = moduleName;
            Version = version;
            Description = description;
            CreatedBy = createdBy;
        }
    }
}
