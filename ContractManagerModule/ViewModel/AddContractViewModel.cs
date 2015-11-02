using System;
using Service;
using Service.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace ContractManagerModule.ViewModel
{
    public class AddContractModuleViewModel : ViewModelBase
    {
        IAPI _api;
        private DateTime _currentDate;
        public Contract _model { get; private set; }
        private string _pattern;
        private Regex _rgx;
        public AddContractModuleViewModel(IAPI api)
        {
            _api = api;
            _model = new Contract();
        }

        public ICommand DeleteContractCommand { get; private set; }

        public void DeleteContract()
        {

        }
        public ICommand AddContractCommand { get; private set; }

        public void AddContract()
        {
            
        }
        public DateTime CurrentDate
        {
            get
            {
                return this._currentDate;
            }
            set
            {
                if (_currentDate == value) return;
                _currentDate = value;
                this._model.date = _currentDate;
                this.DateAdd = _currentDate;
                this.OnPropertyChanged("CurrentDate");
            }
        }
        public string TitleAdd
        {
            get
            {
                return this._model.title;
            }
            set
            {
                this._model.title = value;
                this._model.title = this._model.title.Trim();
                if (this._model.title.Length == 0)
                {
                    this._model.title = "titre";
                }
                this.OnPropertyChanged("TitleAdd");
            }
        }
        public DateTime DateAdd
        {
            get
            {
                return this._model.date;
            }
            set
            {
                this._model.date = this._currentDate;
                this.OnPropertyChanged("DateAdd");
            }
        }
        public string StartAdd
        {
            get
            {
                return this._model.startevent;
            }
            set
            {
                this._model.startevent = value;
                this._model.startevent = this._model.startevent.Trim();
                if (_rgx.IsMatch(this._model.startevent) == false)
                {
                    this._model.startevent = "00:00";
                }
                this.OnPropertyChanged("StartAdd");
            }
        }
        public string EndAdd
        {
            get
            {
                return this._model.endevent;
            }
            set
            {
                this._model.endevent = value;
                this._model.endevent = this._model.endevent.Trim();
                if (_rgx.IsMatch(this._model.endevent) == false)
                {
                    this._model.endevent = "00:00";
                }
                this.OnPropertyChanged("EndAdd");
            }
        }
        public string DescriptionAdd
        {
            get
            {
                return this._model.description;
            }
            set
            {
                this._model.description = value;
                this._model.description = this._model.description.Trim();
                this.OnPropertyChanged("DescriptionAdd");
            }
        }
        public string LocationAdd
        {
            get
            {
                return this._model.location;
            }
            set
            {
                this._model.location = value;
                this._model.location = this._model.location.Trim();
                this.OnPropertyChanged("LocationAdd");
            }
        }
        public int StatusAdd
        {
            get
            {
                return this._model.status;
            }
            set
            {
                if (int.TryParse("123", out value) == false)
                {
                    this._model.status = 0;
                    this.OnPropertyChanged("StatusAdd");
                    return;
                }
                this._model.status = value;
                this.OnPropertyChanged("StatusAdd");
            }
        }
    }
}
