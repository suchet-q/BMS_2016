// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System.Windows;

namespace UserManagerModule
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell : Window
    {
   /*     private readonly IRegionManager _manager;
        private readonly IUnityContainer _container;*/
        public Shell(/*IUnityContainer container, IRegionManager manager*/)
        {
            InitializeComponent();
           /* this._container = container;
            this._manager = manager;
            this.InitializeNavigationMenu();*/

        }
        /*virtual protected void InitializeNavigationMenu()
        {
            this._container.RegisterType<ViewModel.IListModuleViewModel, ViewModel.ListModuleViewModel>();
            var view = this._container.Resolve<View.ListModuleView>();
            this._manager.Regions["MainNavigationRegion"].Add(view);
        }*/
    }
}
