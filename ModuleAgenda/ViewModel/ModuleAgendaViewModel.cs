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
        ObservableCollection<AgendaEvent> _listEvent;
        public DetailsEventsViewModel _viewEvent { get; set; }
        public AddEventViewModel _viewAddEvent { get; set; }
        public ModuleAgendaViewModel(IAPI api)
        {
            _api = api;
            IEnumerable<AgendaEvent> tmpList = _api.Orm.ObjectQuery<AgendaEvent>("select * from agenda_event");
            _listEvent = new ObservableCollection<AgendaEvent>(tmpList);
            _viewEvent = new DetailsEventsViewModel(_listEvent, _api);
            _viewAddEvent = new AddEventViewModel(_api);
            this._currentDate = DateTime.Now;
            this._viewEvent.CurrentDate = this._currentDate;
            this._viewAddEvent.CurrentDate = this._currentDate;
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
                this._viewAddEvent.CurrentDate = value;
                this.OnPropertyChanged("CurrentDate");
                this.OnPropertyChanged("_viewEvent");
            }
        }
    }
}
