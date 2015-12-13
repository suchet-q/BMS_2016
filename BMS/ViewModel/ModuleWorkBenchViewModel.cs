using BMS.ViewModel;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Service;
using Service.DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BMS.ViewModel
{
    public class ModuleWorkBenchViewModel : ViewModelBase
    {
        ObservableCollection<ViewModelBase> _viewModels;

        readonly IModuleRepository          _moduleRepository;

        readonly IModuleCatalog             _catalog;
        IUnityContainer                     _container;
        IRegionManager                      _manager;

        public ModuleWorkBenchViewModel(IModuleCatalog catalog, IUnityContainer container, IRegionManager manager, IMetadataModuleCatalog metadataCatalog)
        {
            _catalog = catalog;
            _container = container;
            _manager = manager;
            _moduleRepository = new ModuleRepository(metadataCatalog);
            var viewModel = new MenuModuleViewModel(_moduleRepository, _manager, _catalog, container);
            MenuModule.Add(viewModel);
        }

        public ObservableCollection<ViewModelBase> MenuModule
        {
            get
            {
                if (_viewModels == null)
                {
                    _viewModels = new ObservableCollection<ViewModelBase>();
                }
                return _viewModels;
            }
        }


        public ObservableCollection<ViewModelBase> ViewModels
        {
            get
            {
                if (_viewModels == null)
                {
                    _viewModels = new ObservableCollection<ViewModelBase>();
                }
                return _viewModels;
            }
        }
    }
}

