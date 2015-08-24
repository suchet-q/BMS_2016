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

namespace ModuleAgenda.ViewModel
{
    public class ModuleAgendaViewModel : ViewModelBase
    {
        IAPI _api;

        public ObservableCollection<AgendaEventViewModel> AllEvents { get; private set; }

        private ObservableCollection<AgendaEvent> _listAllEvents;

        private AgendaEventViewModel _currentEvent;
        public AgendaEventViewModel CurrentEvent
        {
            get
            {
                return _currentEvent;
            }

            set
            {
                _currentEvent = value;
                this.OnPropertyChanged("CurrentEvent");
            }
        }

        public ModuleAgendaViewModel(IAPI api)
        {
            _api = api;

            // Get list de la BD
            IEnumerable<AgendaEvent> listEvent = _api.Orm.ObjectQuery<AgendaEvent>("select * from agenda_event");
            _listAllEvents = new ObservableCollection<AgendaEvent>(listEvent);
            this.AllEvents = new ObservableCollection<AgendaEventViewModel>();
            this.CreateEventPanelVisibility = System.Windows.Visibility.Hidden;
            this.InfosEventPanelVisibility = System.Windows.Visibility.Collapsed;
            this.CreateEventCommand = new DelegateCommand((o) => this.CreateEvent());
        }

        private System.Windows.Visibility _infosEventPanelVisibility;

        public System.Windows.Visibility InfosEventPanelVisibility
        {
            get
            {
                return this._infosEventPanelVisibility;
            }
            set 
            {
                this._infosEventPanelVisibility = value;
                this.OnPropertyChanged("InfosEventPanelVisibility");
            }
        }

        private System.Windows.Visibility _createEventPanelVisibility;

        public System.Windows.Visibility CreateEventPanelVisibility
        {
            get 
            { 
                return this._createEventPanelVisibility;
            }
            set 
            { 
                this._createEventPanelVisibility = value;
                this.OnPropertyChanged("CreateEventPanelVisibility");

            }
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
                AllEvents.Clear();
                _currentDate = value;
                foreach (AgendaEvent evt in _listAllEvents)
                {
                    if (evt.date == value)
                        this.AllEvents.Add(new AgendaEventViewModel(evt, _listAllEvents, _api));
                }
                this.OnPropertyChanged("CurrentDate");
                this.OnPropertyChanged("AllEvents");
                this.CreateEventPanelVisibility = System.Windows.Visibility.Visible;
            }
        }

        public void CreateEvent()
        {
          Console.Error.WriteLine("CreateEvent");
          this.InfosEventPanelVisibility = System.Windows.Visibility.Visible;
          this.CurrentEvent = new AgendaEventViewModel(new AgendaEvent(), _listAllEvents, _api);
         
        }

        public ICommand CreateEventCommand { get; private set; }
    }
}
