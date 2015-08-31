using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersManagerModule
{
    public class OrdersManagerModule : IModule
    {
        IUnityContainer _container;
        IAPI            _api;

        public OrdersManagerModule(IUnityContainer container, IAPI api, IMetadataModuleCatalog metadataCatalog)
        {
            _container = container;
            _api = api;
            metadataCatalog.ModuleMetadata.Add(new ModuleMetadata("Orders Manager", "OrdersManagerModule", "1.0", "This module allow to manage the list of Orders", "BMS"));
        }

        public void Initialize()
        {
            var viewModel = new ViewModel.OrdersManagerModuleViewModel(_api);
            var view = new View.OrdersManagerModuleView();
            view.DataContext = viewModel;
            System.Console.Error.WriteLine("Initialize");
            TransientLifetimeManager tlm = new TransientLifetimeManager();
            _container.RegisterInstance(typeof(object), "OrdersManagerModuleView", view);
        }
    }
}
