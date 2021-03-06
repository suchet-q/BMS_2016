﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service;
using Service.Model;
using System.Collections.ObjectModel;
using Service.DataAccess;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace BMS.ViewModel
{
    public class MenuModuleViewModel : ViewModelBase
    {
        IRegionManager _manager;
        IModuleRepository _moduleRepository;
        IUnityContainer _container;

        private ModuleMetadata _selectedModuleInTheList;

        public ModuleMetadata SelectedModuleInTheList
        {
            get { return _selectedModuleInTheList; }
            //setter appelé quand le focus change dans la listBox
            set
            {
                if (_selectedModuleInTheList == value)
                    return;
                _selectedModuleInTheList = value;
                string moduleHomeView = value.ModuleName + "View";

                Uri destination = new Uri(moduleHomeView, UriKind.Relative);
                IRegion regionToNavigate = this._manager.Regions["MainModuleRegion"];

                if (_container.IsRegistered<IModuleMainViewModel>(value.ModuleName + "ViewModel"))
                {
                    IModuleMainViewModel viewModelToNavigate = _container.Resolve<IModuleMainViewModel>(value.ModuleName + "ViewModel");
                    Task.Factory.StartNew(() => viewModelToNavigate.Refresh());
                }

                regionToNavigate.RequestNavigate(destination, MenuModuleViewModel.CheckForNavigationError); // TODO pour le futur : Implementer le IConfirmNavigation et ce genre de bordel
            }
        }

        public void AddedModule(object sender, EventArgs e)
        {
            this.ListModule = new ObservableCollection<ModuleMetadata>(_moduleRepository.getListActivatedModule());
            System.Console.Error.WriteLine("ON EST DANS KA FONCTION QUI SE FAT TRIGGER LA TETE");
            foreach (ModuleMetadata module in ListModule)
            {
                System.Console.Error.WriteLine("Module dans la liste : " + module.Name);                
            }
        }

        ObservableCollection<ModuleMetadata> _listModule;
        public ObservableCollection<ModuleMetadata> ListModule
        {
            get
            {
                return _listModule;
            }
            set
            {
                if (_listModule == value) return;
                _listModule = value;
                this.OnPropertyChanged("ListModule");
            }
        }

        public MenuModuleViewModel(IModuleRepository moduleRepository, IRegionManager manager, IModuleCatalog catalog, IUnityContainer container)
        {
            _container = container;
            _manager = manager;
            _moduleRepository = moduleRepository;
            var tmp = catalog as DynamicDirectoryModuleCatalog;
            tmp.Added += this.AddedModule;
//            catalog = tmp;
            moduleRepository.Catalog.Changed += this.AddedModule;

            this.ListModule = new ObservableCollection<ModuleMetadata>(moduleRepository.getListActivatedModule());
        }

        protected override void OnDispose()
        {
            this.ListModule.Clear();
        }

        public static void CheckForNavigationError(NavigationResult result)
        {
            if (result.Result == false)
            {
                throw new Exception(result.Error.Message);
            }
        }
    }
}
