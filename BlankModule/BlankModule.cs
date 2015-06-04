using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlankModule
{
    class BlankModule : IModule
    {
        private readonly IRegionManager _manager;
        private readonly IUnityContainer _container;

        public BlankModule(IUnityContainer container, IRegionManager manager)
        {
            this._manager = manager;
            this._container = container;
        }

        public void Initialize()
        {
            TransientLifetimeManager tlm = new TransientLifetimeManager();
            _container.RegisterType(typeof(object), typeof(View.BlankModuleView), "BlankModuleView", tlm);
            _container.RegisterType<ViewModel.IBlankModuleViewModel, ViewModel.BlankModuleViewModel>(new InjectionConstructor());
        }
    }
}
