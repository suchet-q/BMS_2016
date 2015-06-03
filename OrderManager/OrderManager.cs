
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace OrderManager
{
    public class OrderManager : IModule 
    {
        private readonly IRegionManager _manager;
        private readonly IUnityContainer _container;
        private readonly IModuleCatalog _catalog;
        private List<Order> _listOrder;

         public OrderManager(IUnityContainer container, IRegionManager manager)
        {
            this._container = container;
            this._manager = manager;
            _catalog = new ModuleCatalog();
            _listOrder = new List<Order>();
            // Parsing CSV
            // TODO Change file path
            Debug.WriteLine(System.AppDomain.CurrentDomain.BaseDirectory + @"..\..\orders.csv");
            TextFieldParser parser = new TextFieldParser(System.AppDomain.CurrentDomain.BaseDirectory + @"..\..\orders.csv");       
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(";");
            string[] fields = parser.ReadFields();
            while (!parser.EndOfData)
            {
                //Process row
                fields = parser.ReadFields();
                if (fields.Length == 3)
                    _listOrder.Add(new Order(fields[0], fields[1], fields[2], fields[3], fields[4]));
                else
                    _listOrder.Add(new Order(fields[0], fields[1], fields[2], fields[3], fields[4], fields[5]));
                //foreach (string field in fields)
                //{
                //    //TODO: Process field
                //    Debug.WriteLine(field);
                //}
            }
            parser.Close();
//            this._catalog = catalog;
         // this.regionViewRegistry = registry;
        }

        public void Initialize()
         {
             TransientLifetimeManager tlm = new TransientLifetimeManager();
             _container.RegisterType<List<Order>, List<Order>>();
             _container.RegisterType(typeof(object), typeof(View.OrderManagerView), "OrderManagerView", tlm);
             _container.RegisterType<ViewModel.IOrderManagerViewModel, ViewModel.OrderManagerViewModel>(new InjectionConstructor(_catalog, _listOrder));

//             _container.RegisterType<ViewModel.IModuleListViewModel, ViewModel.ModuleListViewModel>();
             //var view = _container.Resolve<View.ModuleListView>();
            // _manager.Regions["MainNavigationRegion"].Add(view);

         }
    }
}
