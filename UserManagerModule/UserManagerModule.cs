using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagerModule
{
    public class UserManagerModule : IModule
    {
        IUnityContainer _container;
        IAPI            _api;

        public UserManagerModule(IUnityContainer container, IAPI api, IMetadataModuleCatalog metadataCatalog)
        {
            _container = container;
            _api = api;
            metadataCatalog.ModuleMetadata.Add(new ModuleMetadata("User Manager", "UserManagerModule", "1.0", "This module allow to manage the list of User", "BMS"));
        }

        public void Initialize()
        {
            var viewModel = new ViewModel.UserManagerModuleViewModel(_api);
            var view = new View.UserManagerModuleView();
            view.DataContext = viewModel;
            System.Console.Error.WriteLine("Initialize");
            TransientLifetimeManager tlm = new TransientLifetimeManager();
            _container.RegisterInstance(typeof(object), "UserManagerModuleView", view);
        }
    }
}
