using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;

namespace ModuleList
{
    public class ModuleList : IModule 
    {
        private readonly IRegionManager _manager;
        private readonly IUnityContainer _container;
//        private readonly IModuleCatalog _catalog;

         public ModuleList(IUnityContainer container, IRegionManager manager)
        {
            this._container = container;
            this._manager = manager;
//            this._catalog = catalog;
         // this.regionViewRegistry = registry;
        }

        public void Initialize()
         {
             _container.RegisterType<ViewModel.IModuleListViewModel, ViewModel.ModuleListViewModel>();
             var view = _container.Resolve<View.ModuleListView>();
             _manager.Regions["MainNavigationRegion"].Add(view);

         }
    }
}
