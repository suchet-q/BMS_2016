using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ModuleList
{
    public class ModuleList : IModule 
    {
        private readonly IRegionManager _manager;
        private readonly IUnityContainer _container;
        private readonly IModuleCatalog _catalog;
        private List<Stock> _listStock;

         public ModuleList(IUnityContainer container, IRegionManager manager)
        {
            this._container = container;
            this._manager = manager;
            _catalog = new ModuleCatalog();
            _listStock = new List<Stock>();
            // Parsing CSV
            // TODO Change file path
            Debug.WriteLine(System.AppDomain.CurrentDomain.BaseDirectory + @"..\..\input.csv");
            TextFieldParser parser = new TextFieldParser(System.AppDomain.CurrentDomain.BaseDirectory + @"..\..\input.csv");       
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(";");
            string[] fields = parser.ReadFields();
            while (!parser.EndOfData)
            {
                //Process row
                fields = parser.ReadFields();
                if (fields.Length == 3)
                    _listStock.Add(new Stock(fields[0], fields[1], fields[2]));
                else
                    _listStock.Add(new Stock(fields[0], fields[1], fields[2], fields[3], fields[4]));
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
             _container.RegisterType<List<Stock>, List<Stock>>();
             _container.RegisterType(typeof(object), typeof(View.ModuleListView), "ModuleListView", tlm);
             _container.RegisterType<ViewModel.IModuleListViewModel, ViewModel.ModuleListViewModel>(new InjectionConstructor(_catalog, _listStock));

//             _container.RegisterType<ViewModel.IModuleListViewModel, ViewModel.ModuleListViewModel>();
             //var view = _container.Resolve<View.ModuleListView>();
            // _manager.Regions["MainNavigationRegion"].Add(view);

         }
    }
}
