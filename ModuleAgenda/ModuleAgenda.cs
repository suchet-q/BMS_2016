using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleAgenda
{
    public class ModuleAgenda : IModule
    {
        IUnityContainer _container;
        IAPI _api;

        public ModuleAgenda(IUnityContainer container, IAPI api, IMetadataModuleCatalog metadataCatalog)
        {
            List<string> BDDTableUsed = new List<string>();
            _container = container;
            _api = api;

            //La tu mets les tables que tu utilse maggle
            //BDDTableUsed.Add("tatable");

            metadataCatalog.Add(new ModuleMetadata("Agenda", "ModuleAgenda", "1.0", "This Module allow you to manage yours event", "BMS", BDDTableUsed));
        }

        public void Initialize()
        {
            var viewModel = new ViewModel.ModuleAgendaViewModel(_api);
            var view = new View.ModuleAgendaView();
            view.DataContext = viewModel;
            System.Console.Error.WriteLine("Initialize");
            TransientLifetimeManager tlm = new TransientLifetimeManager();
            _container.RegisterInstance(typeof(object), "ModuleAgendaView", view);
        }
    }
}
