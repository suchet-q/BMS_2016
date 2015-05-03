// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Windows;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.UnityExtensions;
using System.IO;
using Microsoft.Practices.Prism.Regions;

namespace BMS
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return this.Container.Resolve<Shell>();
        }
        
        protected override void InitializeShell()
        {
            base.InitializeShell();

            

            App.Current.MainWindow = (Window)this.Shell;
            App.Current.MainWindow.Show();
         
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            DynamicDirectoryModuleCatalog moduleCatalog = new DynamicDirectoryModuleCatalog(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Modules"));
            return moduleCatalog;
        }

        protected override void ConfigureContainer()
        {
 	        base.ConfigureContainer();
            this.Container.RegisterInstance<IModuleCatalog>(this.ModuleCatalog);
         }
        protected override void InitializeModules()
        {
            base.InitializeModules();
            IRegionManager manager = this.Container.Resolve<IRegionManager>();
            this.Container.RegisterType<ViewModel.IListModuleViewModel, ViewModel.ListModuleViewModel>();
            var view = this.Container.Resolve<View.ListModuleView>();
            manager.Regions["MainNavigationRegion"].Add(view);
        }
    }
}
