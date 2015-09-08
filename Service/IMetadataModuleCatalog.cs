using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public delegate void AddedModuleMetadataHandler(object sender, EventArgs e);

    public interface IMetadataModuleCatalog
    {
        List<ModuleMetadata> ModuleMetadata { get; set; }
        event AddedModuleMetadataHandler Changed;
        void Add(ModuleMetadata toAdd);
        void OnChange(EventArgs e);
    }
}
