using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.ViewModel
{
    class OrderManagerViewModel : BindableBase, IOrderManagerViewModel
    {
        private readonly IModuleCatalog _catalog;
        protected List<String> _moduleListString;
        protected List<Order> _orderList;

        public List<String> ModuleListString
        {
            get
            {
                return this._moduleListString;
            }
            set { return; }
        }
        public List<Order> OrderList
        {
            get
            {
                return this._orderList;
            }
            set { return; }
        }

        public OrderManagerViewModel(IModuleCatalog moduleCatalog, List<Order> _listStock)
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
            _orderList = _listStock;
            //            _catalog = catalog;
        }
    }
}
