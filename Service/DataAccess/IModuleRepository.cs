using Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DataAccess
{
    public interface IModuleRepository
    {
        List<ModuleMetadata> getListModule();
        List<ModuleMetadata> getListActivatedModule();
        IMetadataModuleCatalog Catalog { get; set; }
    }
}
