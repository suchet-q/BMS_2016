using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleManagerModule
{
    public class ModuleManagerModule : IModule
    {
        IAPI        _api;
        IUnityContainer _container;
        IMetadataModuleCatalog _metadata;

        public ModuleManagerModule(IAPI api, IUnityContainer container, IMetadataModuleCatalog metadataCatalog)
        {
            List<string>    BDDTableUsed = new List<string>();
            _api = api;
            _container = container;

            metadataCatalog.Add(new ModuleMetadata("Module Manager", "ModuleManagerModule", "1.0", "This Module allow you to manager your Modules", "BMS", BDDTableUsed));
            _metadata = metadataCatalog;
        }

        public void Initialize()
        {
            var viewModel = new ViewModel.ModuleManagerModuleViewModel(_metadata, _container);
            var view = new View.ModuleManagerModuleView();
            view.DataContext = viewModel;

            _container.RegisterInstance(typeof(object), "ModuleManagerModuleView", view);

        }
    }
}
