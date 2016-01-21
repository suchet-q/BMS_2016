using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Service;
using SimpleEventBroker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketModule
{
    public class MarketModule : ABMSModule
    {
        IUnityContainer _container;
        IAPI _api;

        //Publie l'event "MarketModuleInitialize", il est maintenant possible a n'importe quelle objet
        //instancié via le container d'y "souscrire" en rajoutant l'attribut [SubscribesTo("MarketModuleInitialize")]
        //au dessus d'une de ses methodes
        [Publishes("MarketModuleInitialize")]
        public event EventHandler MarketModuleInitialized;

        public MarketModule(IUnityContainer container, IAPI api, IMetadataModuleCatalog metadataCatalog) : base(container)
        {
            _container = container;
            _api = api;

            metadataCatalog.Add(new ModuleMetadata("BMS Market", "MarketModule", "1.0", "This module allow access the module market", "BMS", new List<string>()));
        }

        override public void Initialize()
        {
            // le new ParameterOverride permet de spécifier que MarketModuleViewModel sera instancié avec l'object _api donné pour le parametre "api" de son constructeur
            var viewModel = _container.Resolve<ViewModel.MarketModuleViewModel>(new ParameterOverride("api", _api));
            var view = _container.Resolve<View.MarketModuleView>();
            view.DataContext = viewModel;
            System.Console.Error.WriteLine("Initialize");
            TransientLifetimeManager tlm = new TransientLifetimeManager();
            _container.RegisterInstance(typeof(object), "MarketModuleView", view);
            _container.RegisterInstance<IModuleMainViewModel>("MarketModuleViewModel", viewModel);

            //Trigger l'event "MarketModuleInitialize"
            EventHandler initializedHandler = MarketModuleInitialized;
            if (initializedHandler != null)
            {
                initializedHandler(this, EventArgs.Empty);
            }
        }

        //Register les types
        override protected void RegisterType(IUnityContainer container)
        {
            container.RegisterType<ViewModel.MarketModuleViewModel>();
            container.RegisterType<View.MarketModuleView>();
        }
    }
}
