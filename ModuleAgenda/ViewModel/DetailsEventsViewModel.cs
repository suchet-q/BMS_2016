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
using System.Windows.Controls;
using System.Windows;
using Microsoft.Practices.Prism.Mvvm;

namespace ModuleAgenda.ViewModel
{
    public class DetailsEventsViewModel : ViewModelBase
    {
        IAPI _api;
        private string _pattern;
        private Regex _rgx;

        private DateTime _currentDate;
        public ICommand DeleteLocation { get; private set; }
        
        public AgendaEvent _model { get; private set; }
        public AgendaListEvent _currentevent { get; private set; }
        private ObservableCollection<AgendaEvent> _listEvent;
        private ObservableCollection<AgendaListEvent> _currentListEvent;
        private ObservableCollection<AgendaEvent> _listEventToDisplay;
        private ObservableCollection<Location> _listLocation;
        public ICommand AddNewLocation { get; set; }
        System.Windows.Visibility _locationVisibility = Visibility.Collapsed;
        public ICommand ConfirmNewLocation { get; set; }
        public string newlocation { get; set; }

        private int _iduser;
         
        private int _idGroupe;
        //public List<string> hours { "12:00", "12:30", "01:00", "01:30" };
        public DetailsEventsViewModel(ref ObservableCollection<AgendaEvent> listEvent, IAPI api, ObservableCollection<AgendaEvent> listEventToDisplay, int idGroupe, ObservableCollection<Location> listLocation, int iduser)
        {
            _api = api;
            _idGroupe = idGroupe;
            _model = new AgendaEvent();
            _listEvent = listEvent;
            _iduser = iduser;
            _listLocation = listLocation;
            _listEventToDisplay = listEventToDisplay;
            _currentListEvent = new ObservableCollection<AgendaListEvent>();
            this.DeleteEventCommand = new DelegateCommand((o) => this.DeleteEvent());
            AddNewLocation = new DelegateCommand((o) => this.AddLocation());
            ConfirmNewLocation = new DelegateCommand((o) => this.ConfirmLocation());
            DeleteLocation = new DelegateCommand((o) => this.ConfirmDeleteLocation());
            this.AddEventCommand = new DelegateCommand((o) => this.AddEvent());

            this._pattern = "([01]?[0-9]|2[0-3]):[0-5][0-9]";
            this._rgx = new Regex(this._pattern, RegexOptions.IgnoreCase);
            this._model.startevent = "00:00";
            this._model.endevent = "00:00";
            this._model.color = "green";
        }


        private void ConfirmDeleteLocation()
        {
            _api.Orm.Delete("delete from location where id = @Id", new { Id = this.SelectLocation.id });
            var tmp = this.SelectLocation;
            this._listLocation.Remove(tmp);
            this.SelectLocation = this._listLocation.Count() > 0 ? this._listLocation.First() : null;
        }

        public Location SelectLocation
        {
            get;
            set;
        }
        public ObservableCollection<Location> AllLocation
        {
            get
            {
                return this._listLocation;
            }
            set
            {
                if (this._listLocation == value) return;
                this._listLocation = value;
                this.OnPropertyChanged("AllLocation");
            }
        }
        private void ConfirmLocation()
        {
            Location toInsert = new Location();

            toInsert.location = this.newlocation;
            this.newlocation = "";
            var res = _api.Orm.Query("select max(id) as maxId from location");
            toInsert.id = res.First().maxId + 1;
            _api.Orm.Insert("insert into location(location) values (@location)", new { location = toInsert.location});
            _listLocation.Add(toInsert);
            LocationVisibility = Visibility.Collapsed;
            OnPropertyChanged("AllLocation");
        }
        private void AddLocation()
        {
            if (LocationVisibility == Visibility.Visible)
                LocationVisibility = Visibility.Collapsed;
            else
                LocationVisibility = Visibility.Visible;
        }
        public System.Windows.Visibility LocationVisibility
        {
            get
            {
                return this._locationVisibility;
            }

            set
            {
                this._locationVisibility = value;
                this.OnPropertyChanged("LocationVisibility");
            }
        }

        private List<string> _amPm = new List<string>(new string[] { "Am", "Pm" });
        public List<string> AmPm
        {
            get
            {
                return this._amPm;
            }
            set
            {
                if (this._amPm == value)
                    return;
                this._amPm = value;
                this.OnPropertyChanged("AmPm");
            }
        }

        private string _amPmSelectedStart;
        public string AmPmSelectedStart
        {
            get
            {
                return this._amPmSelectedStart;
            }
            set
            {
                if (this._amPmSelectedStart == value)
                    return;
                this._amPmSelectedStart = value;
                //System.Console.WriteLine("jai selectionne ampm= " + this._amPmSelectedStart);
                this.OnPropertyChanged("AmPmSelected");

            }
        }
        private string _amPmSelectedEnd;
        public string AmPmSelectedEnd
        {
            get
            {
                return this._amPmSelectedEnd;
            }
            set
            {
                if (this._amPmSelectedEnd == value)
                    return;
                this._amPmSelectedEnd = value;
                //System.Console.WriteLine("jai selectionne ampm= " + this._amPmSelectedEnd);
                this.OnPropertyChanged("AmPmSelected");

            }
        }
        private string _hourStartSelected;
        public string HourStartSelected
        {
            get
            {
                return this._hourStartSelected;
            }
            set
            {
                if (this._hourStartSelected == value)
                    return;
                this._hourStartSelected = value;
                //System.Console.WriteLine("jai selectionne = " + this._hourStartSelected);
                this.OnPropertyChanged("HourStartSelected");
            }
        }

        private string _hourEndSelected;
        public string HourEndSelected
        {
            get
            {
                return this._hourEndSelected;
            }
            set
            {
                if (this._hourEndSelected == value)
                    return;
                this._hourEndSelected = value;
                //System.Console.WriteLine("jai selectionne = " + this._hourStartSelected);
                this.OnPropertyChanged("HourEndSelected");
            }
        }
        private List<string> _hoursStart = new List<string>(new string[] 
                                        { "12:00", "12:30", "01:00", "01:30", "02:00", "02:30", "03:00", 
                                          "03:30", "04:00", "04:30", "05:00", "05:30", "06:00", "06:30", 
                                          "07:00", "07:30", "08:00", "08:30", "09:00", "09:30", "10:00", 
                                          "10:30", "11:00", "11:30"});
        public List<string> HoursStart
        {
            get
            {
                return this._hoursStart;
            }
            set
            {
                if (this._hoursStart == value)
                    return;
                this._hoursStart = value;
               // System.Console.WriteLine("ma list dheure = " + this._hoursStart);
                this.OnPropertyChanged("HoursStart");
            }
        }
        //Current event selected
        public AgendaListEvent CurrentEvent
        {
            get
            {
                return _currentevent;
            }
            set
            {
                _currentevent = value;
                this.OnPropertyChanged("CurrentEvent");
            }
        }
        //Current list display in view
        public ObservableCollection<AgendaListEvent> CurrentListEvent
        {
            get
            {
                return this._currentListEvent;
            }
            set
            {
                this._currentListEvent = value;
                this.OnPropertyChanged("CurrentListEvent");
                System.Console.WriteLine("je passe dans current list event");
            }
        }
        //handle the list of the day selected
        public DateTime CurrentDate
        {
            get
            {
                return this._currentDate;
            }
            set
            {
                if (_currentDate == value) return;
                this._currentListEvent.Clear();
                _currentDate = value;
                //part for add event
                this._model.date = _currentDate;
                this.DateAdd = _currentDate;
                //end
                System.Console.WriteLine("je passe dans current date");
                if (_listEventToDisplay != null)
                {
                    foreach (AgendaEvent evt in _listEventToDisplay)
                    {
                        if (evt.date == value)
                        {
                            this._currentListEvent.Add(new AgendaListEvent(evt, _api));
                        }
                    }
                }
                this.OnPropertyChanged("CurrentDate");
            }
        }
        //Part delete event
        public ICommand DeleteEventCommand { get; private set; }

        public void DeleteEvent()
        {
            _api.Orm.Delete("delete from agendaevent where id=@idevent", new { idevent = this._currentevent._model.id });
            this._currentListEvent.Remove(this.CurrentEvent);
            this._listEventToDisplay.Remove(this.CurrentEvent._model);
        }
        //Part add event
        public ICommand AddEventCommand { get; private set; }
        public void AddEvent()
        {
            if (this._model.title != null && this._model.startevent != null && this._model.endevent != null)
            {
                this._model.title = this._model.title.Trim();
                this._model.startevent = this._hourStartSelected + " " + this._amPmSelectedStart;//this._model.startevent.Trim();
                this._model.endevent = this._hourEndSelected + " " + this._amPmSelectedEnd;//this._model.endevent.Trim();
                if (this._model.title.Length > 0 && this._model.startevent.Length > 0 && this._model.endevent.Length > 0)
                {
                    this._model.idgroupe = this._idGroupe;
                    this._model.userid = this._iduser;
                    int res = _api.Orm.InsertObject<AgendaEvent>(this._model);
                    IEnumerable<dynamic> idmax = _api.Orm.Query("select max(id) as maxId from agendaevent");
                    this._model.id = (int)idmax.First().maxId;
                    int resupdate = _api.Orm.UpdateObject<AgendaEvent>(@"update agendaevent set date = @date where Id = @Id", this._model);
                    if (res > 0 && resupdate > 0)
                    {
                        this._currentListEvent.Add(new AgendaListEvent(this._model, _api));
                        this._listEventToDisplay.Add(this._model);
                        this._model = new AgendaEvent();
                        DateAdd = this._currentDate;
                        StartAdd = "00:00";
                        EndAdd = "00:00";
                        TitleAdd = "";
                        DescriptionAdd = "";
                        LocationAdd = "";
                        ColorAdd = "";
                        StatusAdd = "";
                    }
                }
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
                    this._model.title = "";
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
                this._model.date = value;
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
        public string StatusAdd
        {
            get
            {
                return this._model.status;
            }
            set
            {
                /*if (int.TryParse("123", out value) == false)
                {
                    this._model.status = 0;
                    this.OnPropertyChanged("StatusAdd");
                    return;
                }*/
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
                    this._model.color = "";
                }
                this.OnPropertyChanged("ColorAdd");
            }
        }
    }
}