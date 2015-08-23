using Service;
using Service.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ModuleAgenda.ViewModel
{
    public class ModuleAgendaViewModel : ViewModelBase
    {
        IAPI _api;

 

        public ObservableCollection<AgendaEventViewModel> AllEvents { get; private set; }

        private ObservableCollection<AgendaEvent> _listAllEvents;

        public ModuleAgendaViewModel(IAPI api)
        {
            _api = api;

            // Get list de la BD
            IEnumerable<AgendaEvent> listEvent = _api.Orm.ObjectQuery<AgendaEvent>("select * from agenda_event");
            _listAllEvents = new ObservableCollection<AgendaEvent>(listEvent);
            this.AllEvents = new ObservableCollection<AgendaEventViewModel>();

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
            }
        }
    }
}
