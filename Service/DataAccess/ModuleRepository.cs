using Microsoft.Practices.Prism.Modularity;
using Service.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DataAccess
{
    public class ModuleRepository : IModuleRepository
    {
        IMetadataModuleCatalog _catalog;
        public IMetadataModuleCatalog Catalog
        {
            get
            {
                return _catalog;
            }
            set
            {
                _catalog = value;
            }
        }      

        //public List<ModuleMetadata> ListModule // VA FALLOIR trigger un event a l'ajout d'un module, ce qui mettra a jour toute les listes
        //{
        //    get;
        //    private set;
        //}

        public ModuleRepository(IMetadataModuleCatalog catalog)
        {
            this._catalog = catalog;

            //IList<ModuleInfo> moduleInfoList = _catalog.Modules.ToList<ModuleInfo>();
            //var tmp = new List<Module>();
            //foreach (var elem in moduleInfoList)
            //{                
            //    tmp.Add(Module.CreateModule(elem.ModuleName));
            //}
            //this.ListModule = new List<Module>(tmp);
        }

        public List<ModuleMetadata> getListModule()
        {
            //IList<ModuleInfo> moduleInfoList = _catalog.Modules.ToList<ModuleInfo>();
            //var tmp = new List<Module>();
            //foreach (var elem in moduleInfoList)
            //{
            //    tmp.Add(Module.CreateModule(elem.ModuleName));
            //}
            //this.ListModule = tmp;

            return this._catalog.ModuleMetadata;
        }
        public List<ModuleMetadata> getListActivatedModule()
        {
            List<ModuleMetadata> res = new List<ModuleMetadata>(); 

            foreach (ModuleMetadata elem in _catalog.ModuleMetadata)
            {
                if (elem.State == ModuleStatus.Activated)
                    res.Add(elem);
            }
            return res;
        }
    }
}
