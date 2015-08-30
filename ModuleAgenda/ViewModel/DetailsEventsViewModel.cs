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
        public AgendaEvent _currentevent { get; private set; }
        private ObservableCollection<AgendaEvent> _listEvent;
        private ObservableCollection<AgendaListEvent> _currentListEvent;
        public DetailsEventsViewModel(ObservableCollection<AgendaEvent> listEvent, IAPI api)
        {
            _api = api;
            _model = new AgendaEvent();
            _listEvent = listEvent;
            _currentListEvent = new ObservableCollection<AgendaListEvent>();
            this.DeleteventCommand = new DelegateCommand((o) => this.DeleteEvent());
        }

        public AgendaEvent Currentevent
        {
            get
            {
                return _currentevent;
            }
            set
            {
                _currentevent = value;
                OnPropertyChanged("Currentevent");
            }
        }
        public ICommand DeleteventCommand { get; private set; }

        public void DeleteEvent()
        {
           //delete
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
