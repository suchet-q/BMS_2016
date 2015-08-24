using Service;
using Service.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleAgenda.ViewModel
{
    public class AgendaEventViewModel : ViewModelBase
    {
        AgendaEvent         _event;
        public AgendaEvent Model { get; set; }
        ObservableCollection<AgendaEvent> _listEvent;
        IAPI _api;

        public AgendaEventViewModel(AgendaEvent user, ObservableCollection<AgendaEvent> listEvent, IAPI api)
        {
            _api = api;
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            _event = Model = user;
            _listEvent = listEvent;
        }

        public string DateString
        {
            get
            {
                return this.Model.date.ToShortDateString();
            }
        }

        public int Id
        {
            get
            {
                return this.Model.id;
            }

            set 
            {
                this.Model.id = value;
                this.OnPropertyChanged("id");
            }
        }

        public DateTime Date
        {
            get
            {
                return this.Model.date;
            }

            set
            {
                this.Model.date = value;
                this.OnPropertyChanged("date");
                _api.Orm.UpdateObject<AgendaEvent>(@"update agenda_event set date = @date where Id = @Id", Model);
            }
        }

        public string StartEvent
        {
            get
            {
                return this.Model.startevent;
            }

            set
            {
                this.Model.startevent = value;
                this.OnPropertyChanged("startevent");
                _api.Orm.UpdateObject<AgendaEvent>(@"update agenda_event set startevent = @startevent where Id = @Id", Model);
            }
        }

        public string EndEvent
        {
            get
            {
                return this.Model.endevent;
            }

            set
            {
                this.Model.endevent = value;
                this.OnPropertyChanged("endevent");
                _api.Orm.UpdateObject<AgendaEvent>(@"update agenda_event set endevent = @endevent where Id = @Id", Model);
            }
        }

        public string Title
        {
            get
            {
                return this.Model.title;
            }

            set
            {
                this.Model.title = value;
                this.OnPropertyChanged("title");
                _api.Orm.UpdateObject<AgendaEvent>(@"update agenda_event set title = @title where Id = @Id", Model);
            }
        }

        public string Description
        {
            get
            {
                return this.Model.description;
            }

            set
            {
                this.Model.description = value;
                this.OnPropertyChanged("description");
                _api.Orm.UpdateObject<AgendaEvent>(@"update agenda_event set description = @description where Id = @Id", Model);
            }
        }

        public string Location
        {
            get
            {
                return this.Model.location;
            }

            set
            {
                this.Model.location = value;
                this.OnPropertyChanged("location");
                _api.Orm.UpdateObject<AgendaEvent>(@"update agenda_event set location = @location where Id = @Id", Model);
            }
        }

        public int UserId
        {
            get
            {
                return this.Model.userid;
            }

            set
            {
                this.Model.userid = value;
                this.OnPropertyChanged("userid");
                _api.Orm.UpdateObject<AgendaEvent>(@"update agenda_event set userid = @userid where Id = @Id", Model);
            }
        }

        public string Color
        {
            get
            {
                return this.Model.color;
            }

            set
            {
                this.Model.color = value;
                this.OnPropertyChanged("color");
                _api.Orm.UpdateObject<AgendaEvent>(@"update agenda_event set color = @color where Id = @Id", Model);
            }
        }
        
        public int Status
        {
            get
            {
                return this.Model.status;
            }

            set
            {
                this.Model.status = value;
                this.OnPropertyChanged("status");
                _api.Orm.UpdateObject<AgendaEvent>(@"update agenda_event set status = @status where Id = @Id", Model);
            }
        }

       /* public List<int> Participants
        {
            get
            {
                return this.Model.participants;
            }
        
            set
            {
                this.Model.participants = value;
                this.OnPropertyChanged("participants");
                _api.Orm.UpdateObject<AgendaEvent>(@"update agenda_event set participants = @participant where Id = @Id", Model);
            }
        }*/
    }
}
