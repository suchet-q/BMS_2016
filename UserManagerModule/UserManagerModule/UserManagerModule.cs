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
            List<string> BDDTableUsed = new List<string>();
            _container = container;
            _api = api;

            //Mettre les tables utilisé par le module, case-sensitive
            BDDTableUsed.Add("user");

            metadataCatalog.Add(new ModuleMetadata("User Manager", "UserManagerModule", "1.0", "This module allow to manage the list of User", "BMS", BDDTableUsed));
        }

        public void Initialize()
        {
            //on instancie le viewModel et la view d'acceuil
            var viewModel = new ViewModel.UserManagerModuleViewModel(_api);
            var view = new View.UserManagerModuleView();

            //On set le datacontext de la view pour qu'elle soit bindé sur le viewModel
            view.DataContext = viewModel;
            System.Console.Error.WriteLine("Initialize");

            // On register la view dans le container pour qu'elle soit instancié par le core par la suite
            _container.RegisterInstance(typeof(object), "UserManagerModuleView", view);
        }
    }
}
