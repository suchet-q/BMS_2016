// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Windows;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.UnityExtensions;
using System.IO;
using Microsoft.Practices.Prism.Regions;
using Service;

namespace BMS
{
    class Bootstrapper : UnityBootstrapper
    {
        protected IAPI api;

        protected override DependencyObject CreateShell()
        {
            return this.Container.Resolve<Shell>();
        }
        
        protected override void InitializeShell()
        {
            base.InitializeShell();

            App.Current.MainWindow = (Window)this.Shell;
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
            this.InitializeAPI();
         }
        protected void InitializeAPI()
        {
            this.api = new API();
            this.api.Initialize();
            this.Container.RegisterInstance<IAPI>(this.api);
        }
        protected override void InitializeModules()
        {
            base.InitializeModules();
            this.InitializeShellContext();
            
            /*           this.Container.RegisterType<ViewModel.ILoginViewModel, ViewModel.LoginViewModel>(); // METTRE MON ILOGIN
            this.Container.RegisterType<ViewModel.IMainViewModel, ViewModel.MainViewModel>(); // METTRE MON IMAIN*/
  /*          var view = this.Container.Resolve(typeof(object), "MainGModulesView");
            manager.Regions["MainContentRegion"].Add(view);*/
        }

        private void InitializeShellContext()
        {
            IRegionManager manager = this.Container.Resolve<IRegionManager>();

            var viewModel = new ViewModel.ShellViewModel(this.ModuleCatalog, this.Container, manager);
            App.Current.MainWindow.DataContext = viewModel;
            App.Current.MainWindow.Show();
        }
    }
}
