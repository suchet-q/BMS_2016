using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Service;
using SimpleEventBroker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace RoleManagerModule
{
    public class RoleManagerModule : ABMSModule
    {
        IUnityContainer _container;
        IAPI _api;
        IMetadataModuleCatalog _metadataCatalog;

        public RoleManagerModule(IUnityContainer container, IAPI api, IMetadataModuleCatalog metadataCatalog) : base(container)
        {
            List<string> BDDTableUsed = new List<string>();
            _container = container;
            _api = api;
            _metadataCatalog = metadataCatalog;

            //La tu mets les tables que tu utilse maggle
            BDDTableUsed.Add("Role");

            metadataCatalog.Add(new ModuleMetadata("Role Manager", "RoleManagerModule", "1.0", "This module allow to manage the list of Role", "BMS", BDDTableUsed));
        }

        override public void Initialize()
        {
            // le new ParameterOverride permet de spécifier que RoleManagerModuleViewModel sera instancié avec l'object _api donné pour le parametre "api" de son constructeur
            var viewModel = _container.Resolve<RoleManagerModuleViewModel>(new ParameterOverride("api", _api), new ParameterOverride("container", _container), new ParameterOverride("moduleCatalog", _metadataCatalog));
            var view = _container.Resolve<View.RoleManagerModuleView>();
            view.DataContext = viewModel;
            System.Console.Error.WriteLine("Initialize");
            TransientLifetimeManager tlm = new TransientLifetimeManager();
            _container.RegisterInstance(typeof(object), "RoleManagerModuleView", view);
            _container.RegisterInstance<IModuleMainViewModel>("RoleManagerModuleViewModel", viewModel);
        }

        //Register les types
        override protected void RegisterType(IUnityContainer container)
        {
            container.RegisterType<RoleManagerModuleViewModel>();
            container.RegisterType<View.RoleManagerModuleView>();
        }
    }
}
