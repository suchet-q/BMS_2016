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

namespace ModuleAgenda.ViewModel
{
    public class DetailsEventsViewModel : ViewModelBase
    {
        IAPI _api;
        private DateTime _currentDate;
        public AgendaEvent _model { get; private set; }
        public AgendaListEvent _currentevent { get; private set; }
        private ObservableCollection<AgendaEvent> _listEvent;
        private ObservableCollection<AgendaListEvent> _currentListEvent;
        public DetailsEventsViewModel(ObservableCollection<AgendaEvent> listEvent, IAPI api)
        {
            _api = api;
            _model = new AgendaEvent();
            _listEvent = listEvent;
            _currentListEvent = new ObservableCollection<AgendaListEvent>();
            this.DeleteEventCommand = new DelegateCommand((o) => this.DeleteEvent());
        }

        public AgendaListEvent CurrentEvent
        {
            get
            {
                return _currentevent;
            }
            set
            {
                System.Console.WriteLine("je set levent en cours");
                _currentevent = value;
                OnPropertyChanged("CurrentEvent");
            }
        }
        public ICommand DeleteEventCommand { get; private set; }

        public void DeleteEvent()
        {
            System.Console.WriteLine("je delete un event " + this._currentevent._model.id);
            _api.Orm.Delete("delete from agendaevent where id=@idevent", new { idevent = this._currentevent._model.id });
            this._currentListEvent.Remove(this.CurrentEvent);
            //this.AllUsers.Remove(this.CurrentUser);
        }
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
                this._currentListEvent.Clear();
                _currentDate = value;
                if (_listEvent != null)
                {
                    foreach (AgendaEvent evt in _listEvent)
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

    }
}
