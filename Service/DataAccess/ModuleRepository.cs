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
        IModuleCatalog _catalog; 

        public IEnumerable<Module> ListModule // VA FALLOIR trigger un event a l'ajout d'un module, ce qui mettra a jour toute les listes
        {
            get;
            private set;
        }

        public ModuleRepository(IModuleCatalog catalog)
        {
            this._catalog = catalog;

            IList<ModuleInfo> moduleInfoList = _catalog.Modules.ToList<ModuleInfo>();
            var tmp = new List<Module>();
            foreach (var elem in moduleInfoList)
            {                
                tmp.Add(Module.CreateModule(elem.ModuleName));
            }
            this.ListModule = new List<Module>(tmp);
        }

        public IEnumerable<Module> getListModule()
        {
            IList<ModuleInfo> moduleInfoList = _catalog.Modules.ToList<ModuleInfo>();
            var tmp = new List<Module>();
            foreach (var elem in moduleInfoList)
            {
                tmp.Add(Module.CreateModule(elem.ModuleName));
            }
            this.ListModule = tmp;

            return this.ListModule;
        }
    }
}
