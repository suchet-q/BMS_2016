using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManagerModule
{
    public class IssueManagerModule : IModule
    {
        IUnityContainer _container;
        IAPI            _api;

        public IssueManagerModule(IUnityContainer container, IAPI api, IMetadataModuleCatalog metadataCatalog)
        {
            List<string> BDDTableUsed = new List<string>();
            _container = container;
            _api = api;

            //La tu mets les tables que tu utilse maggle
            //BDDTableUsed.Add("tatable");

            BDDTableUsed.Add("user");
            BDDTableUsed.Add("issue");
            BDDTableUsed.Add("issue_type");
            metadataCatalog.Add(new ModuleMetadata("Issue Manager", "IssueManagerModule", "1.0", "This module allow to manage the list of issues", "BMS", BDDTableUsed));
        }

        public void Initialize()
        {
            var viewModel = new ViewModel.IssueManagerModuleViewModel(_api, _container);
            var view = new View.IssueManagerModuleView();
            view.DataContext = viewModel;
            System.Console.Error.WriteLine("Initialize");
            TransientLifetimeManager tlm = new TransientLifetimeManager();
            _container.RegisterInstance(typeof(object), "IssueManagerModuleView", view);
        }
    }
}
