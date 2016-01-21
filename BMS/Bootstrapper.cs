// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Windows;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.UnityExtensions;
using System.IO;
using Microsoft.Practices.Prism.Regions;
using Service;
using Service.DataAccess;
using System.Collections.Generic;
using System;
using mscoree;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using EventBrokerExtension;

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
            this.Container.AddNewExtension<SimpleEventBrokerExtension>();
            this.Container.RegisterInstance(this.ModuleCatalog);
            this.CreateMetadataModuleCatalog();
            this.InitializeAPI();
        }

        protected void CreateMetadataModuleCatalog()
        {
            IMetadataModuleCatalog metadataCatalog = new MetadataModuleCatalog();

            this.Container.RegisterInstance<IMetadataModuleCatalog>(metadataCatalog);
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
        }

        private void InitializeShellContext()
        {
            IRegionManager manager = this.Container.Resolve<IRegionManager>();

            var viewModel = new ViewModel.ShellViewModel(this.ModuleCatalog, this.Container, manager, this.api, this.Container.Resolve<IMetadataModuleCatalog>());
            App.Current.MainWindow.DataContext = viewModel;
            App.Current.MainWindow.Closed += viewModel.WindowIsClosing;
            App.Current.MainWindow.Show();
        }

        public void LateInitializeModules()
        {
            base.InitializeModules();
        }

        public static IEnumerable<AppDomain> EnumAppDomains()
        {
            IntPtr enumHandle = IntPtr.Zero;
            ICorRuntimeHost host = null;

            try
            {
                host = new CorRuntimeHost();
                host.EnumDomains(out enumHandle);
                object domain = null;

                host.NextDomain(enumHandle, out domain);
                while (domain != null)
                {
                    yield return (AppDomain)domain;
                    host.NextDomain(enumHandle, out domain);
                }
            }
            finally
            {
                if (host != null)
                {
                    if (enumHandle != IntPtr.Zero)
                    {
                        host.CloseEnum(enumHandle);
                    }

                    Marshal.ReleaseComObject(host);
                }
            }
        }
    }
}
