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
            List<string>    BDDTableUsed = new List<string>();
            _container = container;
            _api = api;

            //La tu mets les tables que tu utilse maggle
            //BDDTableUsed.Add("tatable");
            metadataCatalog.Add(new ModuleMetadata("Orders Manager", "OrdersManagerModule", "1.0", "This module allow to manage the list of Orders", "BMS", BDDTableUsed));
        }

        public void Initialize()
        {
            var viewModel = new ViewModel.OrdersManagerModuleViewModel(_api, _container);
            var view = new View.OrdersManagerModuleView();
            view.DataContext = viewModel;
            System.Console.Error.WriteLine("Initialize");
            TransientLifetimeManager tlm = new TransientLifetimeManager();
            _container.RegisterInstance(typeof(object), "OrdersManagerModuleView", view);
        }
    }
}
