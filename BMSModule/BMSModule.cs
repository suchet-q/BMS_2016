using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;

namespace BMSModule
{
    public class BMSModule : IModule
    {
        private readonly IRegionManager _manager;
        private readonly IUnityContainer _container;

        //        private readonly IRegionViewRegistry regionViewRegistry;

        //       public BMSModule(IRegionViewRegistry registry)
        public BMSModule(IUnityContainer container, IRegionManager manager)
        {
            _container = container;
            _manager = manager;
         // this.regionViewRegistry = registry;
        }

        public void Initialize()
        {
            //           regionViewRegistry.RegisterViewWithRegion("MainRegion", typeof(View.BMSView));
            // On utilise pas la methode de base pour register la view dans la region car comme on decouvre les modules a la volé,
            //il faut d'abord les register dans le container (ce qui est normalement fait quand on ne decouvre pas les modules a la volé par le bootstrapper)
            //pour qu'il puisse faire les injections de dependance et que le ViewModel puisse etre "bindé" sur la view

            TransientLifetimeManager tlm = new TransientLifetimeManager();
            _container.RegisterType(typeof(object), typeof(View.BMSView), "BMSView", tlm);
            var view = _container.Resolve<View.BMSView>();
            // _manager.Regions["MainContentRegion"].Add(view);
        }
    }
}
