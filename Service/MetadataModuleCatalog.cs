using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class MetadataModuleCatalog : IMetadataModuleCatalog
    {
        List<ModuleMetadata> _moduleMetadata;

        public List<ModuleMetadata> ModuleMetadata
        {
            get
            {
                return _moduleMetadata;
            }
            set
            {
                if (_moduleMetadata == value) return;
                _moduleMetadata = value;
            }
        }

        public MetadataModuleCatalog()
        {
            this.ModuleMetadata = new List<ModuleMetadata>();
        }
    }
}
