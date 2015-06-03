using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ModuleList.ViewModel
{
    class ModuleListViewModel : BindableBase, IModuleListViewModel
    {
        private readonly IModuleCatalog _catalog;
        protected List<String> _moduleListString;
        protected List<Stock> _moduleStockList;

        public List<String> ModuleListString
        {
            get
            {

                return this._moduleListString;
            }
            set { return; }
        }
        public List<Stock> ModuleStockList
        {
            get
            {
                return this._moduleStockList;
            }
            set { return; }
        }

        public ModuleListViewModel(IModuleCatalog moduleCatalog, List<Stock> _listStock)
        {
            Debug.WriteLine("NANANANANA");
            _catalog = moduleCatalog;
            IList<ModuleInfo> toto = _catalog.Modules.ToList<ModuleInfo>();
            _moduleListString = new List<String>();
            foreach (var elem in _listStock)
            {
                _moduleListString.Add(elem.ToString());
                System.Console.Error.WriteLine("Le module se nomme : " + elem.ToString());
            }
            _moduleStockList = _listStock;
//            _catalog = catalog;
        }
    }
}
