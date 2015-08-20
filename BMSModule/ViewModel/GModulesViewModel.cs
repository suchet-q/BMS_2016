using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Service;
using Service.Model;

namespace BMSModule.ViewModel
{
    public class GModulesViewModel : ViewModelBase
    {
        readonly IAPI _api;
        ObservableCollection<ViewModelBase> _viewModels;

        public GModulesViewModel(IAPI api)
        {
            this._api = api;
            UserListViewModel viewModel = new UserListViewModel(api);
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
