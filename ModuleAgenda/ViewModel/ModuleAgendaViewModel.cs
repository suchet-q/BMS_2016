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
        public ObservableCollection<AgendaEvent> _listEventToDisplay;
        public DetailsEventsViewModel _viewEvent { get; set; }
        public ICommand AddNewCalendar { get; private set; }
        public ICommand DeleteCalendar { get; private set; }
        public ICommand ConfirmNewCalendar { get; private set; }
        public string newCalendarName { get; set; }
        public ObservableCollection<Location> _listLocation;

        public int _iduser { get; set; }
        System.Windows.Visibility _newVisibility = Visibility.Collapsed;
        public ModuleAgendaViewModel(IAPI api)
        {
            _api = api;
            _iduser = _api.LoggedUser.id;
            IEnumerable<AgendaEvent> result = null;
            AddNewCalendar = new DelegateCommand((o) => this.AddCalendar()); 
            ConfirmNewCalendar = new DelegateCommand((o) => this.ConfirmCalendar());
            DeleteCalendar = new DelegateCommand((o) => this.ConfirmDeleteCalendar());

            this.OnPropertyChanged("newReceiverVisibility");
            //check if user have is own calendar and create it in case is not 
            result = _api.Orm.ObjectQuery<AgendaEvent>("select * from agendaname where userid=@UserId and name=@Name", new { UserId = _api.LoggedUser.id, Name = _api.LoggedUser.login });
            bool exist = false;
            if (result.Count() != 0)
                exist = true;
            if (exist == false)
            {
                AgendaName request = new AgendaName();
                request.name = _api.LoggedUser.login;
                request.userid = _api.LoggedUser.id;
                IEnumerable<dynamic> idmax = _api.Orm.Query("select max(idgroupe) as maxId from agendaname");
                if (idmax != null)
                {
                    request.idgroupe = (int)idmax.First().maxId + 1;
                    List<int> requestuserid = new List<int>();
                    requestuserid.Add(_api.LoggedUser.id);
                    request.participants = this.ListToDataBaseString(requestuserid);
                    int res = _api.Orm.InsertObject<AgendaName>(request);
                    if (res > 0)
                    {
                        //error message
                    }
                }
                else
                {
                    //error message
                }
            }
             IEnumerable<Location> tmpListLocation = _api.Orm.ObjectQuery<Location>("select * from location");
            IEnumerable<AgendaName> tmpListAgendaName = _api.Orm.ObjectQuery<AgendaName>("select * from agendaname");
            IEnumerable<User> tmpUserList = _api.Orm.ObjectQuery<User>("select * from user");
            IEnumerable<AgendaEvent> tmpList = _api.Orm.ObjectQuery<AgendaEvent>("select * from agendaevent");
            if (tmpListLocation != null)
                _listLocation = new ObservableCollection<Location>(tmpListLocation);
            else
                _listUsers = null;
            if (tmpUserList != null)
                _listUsers = new ObservableCollection<User>(tmpUserList);
            else
                _listUsers = null;

            if (tmpList != null)
                _listEvent = new ObservableCollection<AgendaEvent>(tmpList);
            else
                _listEvent = null;
            //get all agenda you can display
            if (tmpListAgendaName != null)
            {
                ObservableCollection<AgendaName> getValidAgendaName = new ObservableCollection<AgendaName>(tmpListAgendaName);
                _listAgendaName = new ObservableCollection<AgendaName>();
                foreach (AgendaName elem in getValidAgendaName)
                {
                    if (elem.participants == null) /*.IndexOf(this._api.LoggedUser.id) != -1*/
                        _listAgendaName.Add(elem);
                    else if (this.DataBaseStringToList(elem.participants).IndexOf(this._api.LoggedUser.id) != -1)
                    {
                        _listAgendaName.Add(elem);
                    }
                }
            }
            else
                _listAgendaName = null;
            //get firm calendar
            _listEventToDisplay = new ObservableCollection<AgendaEvent>();
            foreach (AgendaEvent evt in _listEvent)
            {
                foreach (AgendaName elem in _listAgendaName)
                {
                    if (evt.idgroupe == 0)// 1 = agenda Firm
                    {
                        //System.Console.WriteLine(" je regqrde dans firm = " + evt.title + "  ___  " + evt.idgroupe);
                        _listEventToDisplay.Add(evt);
                        break;
                    }
                }
            }
            this.CurrentCalendar = ListAgendaName.FirstOrDefault();
            _viewEvent = new DetailsEventsViewModel(ref _listEvent, _api, _listEventToDisplay, 1, _listLocation, this._iduser);
            this._currentDate = DateTime.Now;
            this.CurrentDate = this._currentDate;
            this.OnPropertyChanged("CurrentDate");
            this._viewEvent.CurrentDate = this._currentDate;
            //this.CurrentCalendar = "Firm"; select the current aganda display 
        }
        private void ConfirmDeleteCalendar()
        {
            _api.Orm.Delete("delete from agendaname where id = @Id", new { Id = this._currentCalendar.id });
            _api.Orm.Delete("delete from agendaevent where idgroupe = @Id", new { Id = this._currentCalendar.idgroupe });
            var tmp = this._currentCalendar;
            this._listAgendaName.Remove(tmp);
            this._currentCalendar = this._listAgendaName.Count() > 0 ? this._listAgendaName.First() : null;
        }
        private void ConfirmCalendar()
        {
            //System.Console.WriteLine(" new new calenar de ouf ========= ");
            AgendaName toInsert = new AgendaName();

            var res =  _api.Orm.Query("select max(idgroupe) as maxId from agendaname");
            toInsert.name = this.newCalendarName;
            toInsert.idgroupe = res.First().maxId + 1;
            toInsert.participants = "";
            toInsert.userid = _iduser;
            //toInsert.address = this.newClientAddress;
            //this.newClientAddress = "";
            this.newCalendarName = "";

            _api.Orm.Insert("insert into agendaname(idgroupe, name, participants, userid) values (@idgroupe, @name, @participants, @userid)", new { newname = toInsert.name, idgroupe = res.First().maxId + 1, participants = "", userid = _iduser });
            if (res != null)
            {
                var resid = _api.Orm.Query("select max(id) as maxId from agendaname");
                toInsert.id = resid.First().maxId + 1;
                this.ListAgendaName.Add(toInsert);
                this.OnPropertyChanged("ListAgendaName");
            }
            NewVisibility = Visibility.Collapsed;
        }
        private void AddCalendar()
        {
            if (NewVisibility == Visibility.Visible)
                NewVisibility = Visibility.Collapsed;
            else
                NewVisibility = Visibility.Visible;
        }

        public System.Windows.Visibility NewVisibility
        {
            get
            {
                return this._newVisibility;
            }

            set
            {
                this._newVisibility = value;
                this.OnPropertyChanged("NewVisibility");
            }
        }
        private string ListToDataBaseString(List<int> list)
        {
            string result;
            result = "";

            foreach (int elem in list)
            {
                result += elem.ToString() + ";";
            }
            return result;
        }
        private List<int> DataBaseStringToList(string list)
        {
            List<int> result;
            string[] tokens = null;
            char[] delimiterChars = { ';' };

            tokens = list.Split(delimiterChars);
            result = new List<int>();

            for (int i = 0; i < tokens.Length; i++)
            {
                if (tokens[i] != "")
                    result.Add(Int32.Parse(tokens[i]));
            }
            return result;
        }
        private ObservableCollection<AgendaName> _listAgendaName;
        public ObservableCollection<AgendaName> ListAgendaName
        {
            get { return this._listAgendaName; }
            set
            {
                if (this._listAgendaName == value)
                    return;
                this._listAgendaName = value;
                this.OnPropertyChanged("ListAgendaName");
            }
        }
        private ObservableCollection<User> _listUsers;
        public ObservableCollection<User> ListUsers
        {
            get { return this._listUsers; }
            set
            {
                if (this._listUsers == value)
                    return;
                this._listUsers = value;
                System.Console.WriteLine("list user = " + value);
                this.OnPropertyChanged("ListUsers");
            }
        }


        /*private User _currentCalendar;
        public User CurrentCalendar
        {
            get { return this._currentCalendar; }
            set
            {
                if (this._currentCalendar == value)
                    return;
                this._currentCalendar = value;
                System.Console.WriteLine("curent calendar = " + value);
                this.OnPropertyChanged("CurrentCalendar");
            }
        }*/

        private AgendaName _currentCalendar;
        public AgendaName CurrentCalendar
        {
            get { return this._currentCalendar; }
            set
            {
                if (this._currentCalendar == value)
                    return;
                this._currentCalendar = value;
                this._listEventToDisplay.Clear();
                foreach (AgendaEvent evt in _listEvent)
                {
                  //  System.Console.WriteLine("ajout de + " + evt.idgroupe + " ------" + evt.title + " idgroup = " + this._currentCalendar.idgroupe);
                    if (evt.idgroupe == this._currentCalendar.idgroupe)
                    {
                    //    System.Console.WriteLine("ajout de + " + evt.title);
                        this._listEventToDisplay.Add(evt);
                    }
                }
                this.OnPropertyChanged("CurrentCalendar");
                this._viewEvent = new DetailsEventsViewModel(ref _listEvent, _api, _listEventToDisplay, this._currentCalendar.idgroupe, _listLocation, this._iduser);
                this._currentDate = DateTime.Now;
                this._viewEvent.CurrentDate = this._currentDate;
                this.OnPropertyChanged("_viewEvent");
                this.OnPropertyChanged("CurrentDate");
               // System.Console.WriteLine("je change le calendrier");
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
                Console.Error.WriteLine("date changed");
                this._currentDate = value;
                this._viewEvent.CurrentDate = value;
                this.OnPropertyChanged("_viewEvent");
                this.OnPropertyChanged("CurrentDate");

            }
        }
    }
}
