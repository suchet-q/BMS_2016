using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Service;
using SimpleEventBroker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagerModule
{
    public class UserManagerModule : ABMSModule
    {
        IUnityContainer _container;
        IAPI            _api;

        //Publie l'event "UserManagerModuleInitialize", il est maintenant possible a n'importe quelle objet
        //instancié via le container d'y "souscrire" en rajoutant l'attribut [SubscribesTo("UserManagerModuleInitialize")]
        //au dessus d'une de ses methodes
        [Publishes("UserManagerModuleInitialize")]
        public event EventHandler UserManagerModuleInitialized;

        public UserManagerModule(IUnityContainer container, IAPI api, IMetadataModuleCatalog metadataCatalog) : base(container)
        {
            List<string> BDDTableUsed = new List<string>();
            _container = container;
            _api = api;

            //La tu mets les tables que tu utilse maggle
            BDDTableUsed.Add("user");

            metadataCatalog.Add(new ModuleMetadata("User Manager", "UserManagerModule", "1.0", "This module allow to manage the list of User", "BMS", BDDTableUsed));
        }

        override public void Initialize()
        {
            // le new ParameterOverride permet de spécifier que UserManagerModuleViewModel sera instancié avec l'object _api donné pour le parametre "api" de son constructeur
            var viewModel = _container.Resolve<ViewModel.UserManagerModuleViewModel>(new ParameterOverride("api", _api));
            var view =  _container.Resolve<View.UserManagerModuleView>();
            view.DataContext = viewModel;
            System.Console.Error.WriteLine("Initialize");
            TransientLifetimeManager tlm = new TransientLifetimeManager();
            _container.RegisterInstance(typeof(object), "UserManagerModuleView", view);
            _container.RegisterInstance<IModuleMainViewModel>("UserManagerModuleViewModel", viewModel);

            //Trigger l'event "UserManagerModuleInitialize"
            EventHandler initializedHandler = UserManagerModuleInitialized;
            if (initializedHandler != null)
            {
                initializedHandler(this, EventArgs.Empty);
            }
        }

        //Register les types
        override protected void RegisterType(IUnityContainer container)
        {
            container.RegisterType<ViewModel.UserManagerModuleViewModel>();
            container.RegisterType<View.UserManagerModuleView>();
        }
    }
}
