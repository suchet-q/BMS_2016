using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service;
using Service.DataAccess;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Regions;
using BMS.ViewModel;
using BMS.View;

namespace BMS.ViewModel
{
    class ShellViewModel : ViewModelBase
    {
        ObservableCollection<ViewModelBase> _viewModels;

        readonly IModuleRepository          _moduleRepository;

        readonly IModuleCatalog             _catalog;
        IUnityContainer                     _container;
        IRegionManager                      _manager;

        public ShellViewModel(IModuleCatalog catalog, IUnityContainer container, IRegionManager manager, IAPI api)
        {
            _catalog = catalog;
            _container = container;
            _manager = manager;
            var viewModel = new LoginViewModel(api, _container);
            viewModel.EventLogin += this.NavigateToModuleWorkBench;
            ViewModels.Add(viewModel);
        }

        //public ObservableCollection<ViewModelBase> MenuModule
        //{
        //    get
        //    {
        //        if (_viewModels == null)
        //        {
        //            _viewModels = new ObservableCollection<ViewModelBase>();
        //        }
        //        return _viewModels;
        //    }
        //}


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
            set
            {
                if (_viewModels == value) return;
                _viewModels = value;
                this.OnPropertyChanged("ViewModels");
            }
        }

        void NavigateToModuleWorkBench(object sender, EventArgs args)
        {
            var viewModel = new ModuleWorkBenchViewModel(_catalog, _container, _manager);
            this.ViewModels.Clear();
            this.ViewModels.Add(viewModel);
        }
    }
}
