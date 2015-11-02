using Service;
using Service.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ContractManagerModule.ViewModel
{
    public class ContractManagerModuleViewModel : ViewModelBase
    {
        IAPI _api;
        public ObservableCollection<Contract> _listContract;
        public ContractViewModel _viewContract { get; set; }
        public AddContractViewModel _viewAddContract { get; set; }
        public ContractManagerModuleViewModel(IAPI api)
        {
            _api = api;
            /* this.DisplayConnexionErrMsg = false;*/
            IEnumerable<Contract> tmpList = _api.Orm.ObjectQuery<Contract>("select * from ");
            if (tmpList != null)
                _listContract = new ObservableCollection<Contract>(tmpList);
            else
                _listContract = null;
            _viewContract = new ContractViewModel(_listContract, _api);
            _viewAddContract = new AddContractViewModel(_api);
            this._currentDate = DateTime.Now;
            this._viewContract.CurrentDate = this._currentDate;
            this._viewAddContract.CurrentDate = this._currentDate;
        }


        private DateTime _currentDate;

        public DateTime CurrentDate
        {
            get
            {
                return _currentDate;
            }
            set
            {
                Console.Error.WriteLine("date changed");
                _currentDate = value;
                this._viewEvent.CurrentDate = value;
                this._viewAddContract.CurrentDate = value;
                this.OnPropertyChanged("CurrentDate");
                this.OnPropertyChanged("_viewContract");
            }
        }
    }
}
