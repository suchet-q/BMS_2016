using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractManagerModule
{
    public class ContractManagerModule : IModule
    {
        IUnityContainer _container;
        IAPI _api;

        public ContractManagerModule(IUnityContainer container, IAPI api, IMetadataModuleCatalog metadataCatalog)
        {
            List<string> BDDTableUsed = new List<string>();
            _container = container;
            _api = api;

            //La tu mets les tables que tu utilse maggle
            //BDDTableUsed.Add("tatable");

            metadataCatalog.Add(new ModuleMetadata("Contract", "ContractManagerModule", "1.0", "This Module allow you to add or edit contract", "BMS", BDDTableUsed));
        }

        public void Initialize()
        {
            var viewModel = new ViewModel.ContractManagerModuleViewModel(_api);
            var view = new View.ContractManagerModuleView();
            view.DataContext = viewModel;
            System.Console.Error.WriteLine("Initialize");
            TransientLifetimeManager tlm = new TransientLifetimeManager();
            _container.RegisterInstance(typeof(object), "ContractManagerModuleView", view);
        }
    }
}
