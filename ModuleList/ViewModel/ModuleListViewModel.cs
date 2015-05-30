using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ModuleList.ViewModel
{
    class ModuleListViewModel : BindableBase, IModuleListViewModel
    {
        private readonly IModuleCatalog _catalog;
        protected List<String> _moduleListString;

        public List<String> ModuleListString
        {
            get
            {
                return this._moduleListString;
            }
            set { return; }
        }

        public ModuleListViewModel(IModuleCatalog moduleCatalog)
        {
            _catalog = moduleCatalog;
            IList<ModuleInfo> toto = _catalog.Modules.ToList<ModuleInfo>();
            _moduleListString = new List<String>();
            foreach (var elem in toto)
            {
                _moduleListString.Add(elem.ModuleName);
                System.Console.Error.WriteLine("Le module se nomme : " + elem.ModuleName);
            }
//            _catalog = catalog;
        }
    }
}
