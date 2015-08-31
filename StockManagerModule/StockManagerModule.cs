using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockManagerModule
{
    public class StockManagerModule : IModule
    {

        IAPI        _api;
        IUnityContainer _container;

        public StockManagerModule(IAPI api, IUnityContainer container, IMetadataModuleCatalog metadataCatalog)
        {
            _api = api;
            _container = container;
            metadataCatalog.ModuleMetadata.Add(new ModuleMetadata("Stock Manager", "StockManagerModule", "1.0", "This Module allow you to manager your Stock", "BMS"));
        }

        public void Initialize()
        {
            var viewModel = new ViewModel.StockManagerModuleViewModel(_api, _container);
            var view = new View.StockManagerModuleView();
            view.DataContext = viewModel;

            _container.RegisterInstance(typeof(object), "StockManagerModuleView", view);

        }
    }
}
