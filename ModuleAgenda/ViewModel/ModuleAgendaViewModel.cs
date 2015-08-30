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
        public ObservableCollection<AgendaEvent> _listEvent;
        public DetailsEventsViewModel _viewEvent { get; set; }
        public AddEventViewModel _viewAddEvent { get; set; }
        public ModuleAgendaViewModel(IAPI api)
        {
            _api = api;
           /* this.DisplayConnexionErrMsg = false;*/
            IEnumerable<AgendaEvent> tmpList = _api.Orm.ObjectQuery<AgendaEvent>("select * from agendaevent");
            if (tmpList != null)
                _listEvent = new ObservableCollection<AgendaEvent>(tmpList);
            else
                _listEvent = null;
            _viewEvent = new DetailsEventsViewModel(_listEvent, _api);
            _viewAddEvent = new AddEventViewModel(_api);
            this._currentDate = DateTime.Now;
            this._viewEvent.CurrentDate = this._currentDate;
            this._viewAddEvent.CurrentDate = this._currentDate;
        }

       /* private bool _displayDatabaseErrMsg;
        public bool DisplayConnexionErrMsg
        {
            get
            {
                return _displayDatabaseErrMsg;
            }
            set
            {
                if (_displayDatabaseErrMsg == value) return;
                _displayDatabaseErrMsg = value;
                this.OnPropertyChanged("DisplayConnexionErrMsg");
            }
        }*/

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
