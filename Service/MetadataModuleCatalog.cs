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

        public event AddedModuleMetadataHandler Changed;

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
                OnChange(null);
            }
        }

        public MetadataModuleCatalog()
        {
            this.ModuleMetadata = new List<ModuleMetadata>();
        }

        public void Add(ModuleMetadata toAdd)
        {
            this.ModuleMetadata.Add(toAdd);
            this.OnChange(null);
        }

        public void OnChange(EventArgs e)
        {
            if (Changed != null)
                Changed(this, e);
        }
    }
}
