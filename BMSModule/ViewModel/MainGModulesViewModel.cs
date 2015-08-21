using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Service;
using Service.Model;
using Microsoft.Practices.Prism.Modularity;

namespace BMSModule.ViewModel
{
    public class MainGModulesViewModel : ViewModelBase
    {
        ObservableCollection<ViewModelBase> _viewModels;

        public MainGModulesViewModel(IModuleCatalog catalog)
        {
            GModulesViewModel viewModel = new GModulesViewModel(catalog);
            this.ViewModels.Add(viewModel);
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
