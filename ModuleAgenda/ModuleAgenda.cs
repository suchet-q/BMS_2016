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
        IAPI            _api;

        public ModuleAgenda(IUnityContainer container, IAPI api)
        {
            _container = container;
            _api = api;
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
