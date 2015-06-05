using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$
{
    public class $safeprojectname$ : IModule
    {
        private readonly IRegionManager _manager;
        private readonly IUnityContainer _container;

        public $safeprojectname$(IUnityContainer container, IRegionManager manager)
        {
            this._manager = manager;
            this._container = container;
        }

        public void Initialize()
        {
            TransientLifetimeManager tlm = new TransientLifetimeManager();
            _container.RegisterType(typeof(object), typeof(View.$safeprojectname$View), "$safeprojectname$View", tlm);
            _container.RegisterType<ViewModel.I$safeprojectname$ViewModel, ViewModel.$safeprojectname$ViewModel>(new InjectionConstructor());
        }
    }
}
