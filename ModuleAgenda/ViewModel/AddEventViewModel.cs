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

namespace ModuleAgenda.ViewModel
{
    public class AddEventViewModel : ViewModelBase
    {
        IAPI _api;
        private DateTime _currentDate;
        public AgendaEvent _model { get; private set; }
        private string _pattern;
        private Regex _rgx;
        public AddEventViewModel(IAPI api)
        {
            _api = api;
            _model = new AgendaEvent();
            this.AddEventCommand = new DelegateCommand((o) => this.AddEvent());
            this.DeleteEventCommand = new DelegateCommand((o) => this.DeleteEvent());
            this._pattern = "([01]?[0-9]|2[0-3]):[0-5][0-9]";
            this._rgx = new Regex(this._pattern, RegexOptions.IgnoreCase);
            this._model.startevent = "00:00";
            this._model.endevent= "00:00";
            this._model.color = "green";
        }

        public ICommand DeleteEventCommand { get; private set; }

        public void DeleteEvent()
        { 
        }
        public ICommand AddEventCommand { get; private set; }

        public void AddEvent()
        {
             Console.Error.WriteLine("ajout nouveaux champs step 1");
             this._model.title = this._model.title.Trim();
             this._model.startevent = this._model.startevent.Trim();
             this._model.endevent = this._model.endevent.Trim();
             if (this._model.title.Length > 0 && this._model.startevent.Length > 0 && this._model.endevent.Length > 0)
             {
                 int res = _api.Orm.InsertObject<AgendaEvent>(this._model);
                 IEnumerable<dynamic> idmax = _api.Orm.Query("select max(id) as maxId from agendaevent");
                 this._model.id = (int)idmax.First().maxId;
                 int resupdate = _api.Orm.UpdateObject<AgendaEvent>(@"update agendaevent set date = @date where Id = @Id", this._model);
                 if (res > 0 && resupdate > 0) 
                 {
                     this._model = new AgendaEvent();
                     DateAdd = this._currentDate;
                     StartAdd = "00:00";
                     EndAdd = "00:00";
                     TitleAdd = "titre";
                     DescriptionAdd = "";
                     LocationAdd = "";
                     ColorAdd = "green";
                 }
            }
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
        public string ColorAdd
        {
            get
            {
                return this._model.color;
            }

            set
            {
                this._model.color = value;
                this._model.color = this._model.color.Trim();
                if (this._model.color.Length == 0)
                {
                    this._model.color = "green";
                }
                this.OnPropertyChanged("ColorAdd");
            }
        }
    }
}
