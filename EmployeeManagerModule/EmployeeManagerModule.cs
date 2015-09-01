using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagerModule
{
    public class EmployeeManagerModule : IModule
    {
        IUnityContainer _container;
        IAPI            _api;

        public EmployeeManagerModule(IUnityContainer container, IAPI api, IMetadataModuleCatalog metadataCatalog)
        {
            _container = container;
            _api = api;
            metadataCatalog.ModuleMetadata.Add(new ModuleMetadata("Employee Manager", "EmployeeManagerModule", "1.0", "This module allow to manage the list of employee", "BMS"));
        }

        public void Initialize()
        {
            var viewModel = new ViewModel.EmployeeManagerModuleViewModel(_api, _container);
            var view = new View.EmployeeManagerModuleView();
            view.DataContext = viewModel;
            System.Console.Error.WriteLine("Initialize");
            TransientLifetimeManager tlm = new TransientLifetimeManager();
            _container.RegisterInstance(typeof(object), "EmployeeManagerModuleView", view);
        }
    }
}
