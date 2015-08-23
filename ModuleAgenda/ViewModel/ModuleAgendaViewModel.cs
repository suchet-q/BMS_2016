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
            foreach (AgendaEvent evt in listEvent)
            {
                this.AllEvents.Add(new AgendaEventViewModel(evt, _listAllEvents, _api));
            }
        }
    }
}
