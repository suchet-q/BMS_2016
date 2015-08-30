using Service;
using Service.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ModuleAgenda.ViewModel
{
    public class AgendaListEvent : ViewModelBase
    {
        IAPI _api;
        public AgendaEvent _model { get; private set; }
        public string _tmpStartEvent;
        public string _tmpEndEvent;
        private string _pattern;
        private Regex _rgx;

        public AgendaListEvent(AgendaEvent model, IAPI api)
        {
            _api = api;
            _model = model;
            _tmpStartEvent = model.startevent;
            _tmpEndEvent = model.endevent;
            this._pattern = "([01]?[0-9]|2[0-3]):[0-5][0-9]";
            this._rgx = new Regex(this._pattern, RegexOptions.IgnoreCase);
        }

        public string DateString
        {
            get
            {
                return this._model.date.ToShortDateString();
            }
        }

        /*public int Id
        {
            get
            {
                return this._model.id;
            }

            set
            {
                this._model.id = value;
                this.OnPropertyChanged("id");
            }
        }*/

        public DateTime Date
        {
            get
            {
                return this._model.date;
            }

            set
            {
            }
        }

        public string StartEvent
        {
            get
            {
                return this._model.startevent;
            }

            set
            {
                this._model.startevent = value;
                this._model.startevent = this._model.startevent.Trim();
                if (this._model.startevent.Length == 0)
                {
                    this._model.startevent = this._tmpStartEvent;
                }
                if (_rgx.IsMatch(this._model.startevent) == false)
                {
                    this._model.startevent = "00:00";
                }
                this.OnPropertyChanged("startevent");
                _api.Orm.UpdateObject<AgendaEvent>(@"update agenda_event set startevent = @startevent where Id = @Id", _model);
            }
        }

        public string EndEvent
        {
            get
            {
                return this._model.endevent;
            }

            set
            {
                this._model.endevent = value;
                this._model.endevent = this._model.endevent.Trim();
                if (this._model.endevent.Length == 0)
                {
                    this._model.endevent = this._tmpEndEvent;
                }
                if (_rgx.IsMatch(this._model.startevent) == false)
                {
                    this._model.startevent = "00:00";
                }
                this.OnPropertyChanged("endevent");
                _api.Orm.UpdateObject<AgendaEvent>(@"update agenda_event set endevent = @endevent where Id = @Id", _model);
            }
        }

        public string Title
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
                    this._model.title = "title";
                }
                this.OnPropertyChanged("title");
                _api.Orm.UpdateObject<AgendaEvent>(@"update agenda_event set title = @title where Id = @Id", _model);
            }
        }

        public string Description
        {
            get
            {
                return this._model.description;
            }

            set
            {
                this._model.description = value;
                this._model.description = this._model.description.Trim();
                this.OnPropertyChanged("description");
                _api.Orm.UpdateObject<AgendaEvent>(@"update agenda_event set description = @description where Id = @Id", _model);
            }
        }

        public string Location
        {
            get
            {
                return this._model.location;
            }

            set
            {
                this._model.location = value;
                this._model.location = this._model.location.Trim();
                this.OnPropertyChanged("location");
                _api.Orm.UpdateObject<AgendaEvent>(@"update agenda_event set location = @location where Id = @Id", _model);
            }
        }

        public int UserId
        {
            get
            {
                return this._model.userid;
            }

            set
            {
                //remplacer par L'id de la personne en cous d'utilisation
                this._model.userid = value;
                this.OnPropertyChanged("userid");
                _api.Orm.UpdateObject<AgendaEvent>(@"update agenda_event set userid = @userid where Id = @Id", _model);
            }
        }

        public string Color
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
                    this._model.color = "green";
                }
                this.OnPropertyChanged("color");
                _api.Orm.UpdateObject<AgendaEvent>(@"update agenda_event set color = @color where Id = @Id", _model);
            }
        }

        public int Status
        {
            get
            {
                return this._model.status;
            }

            set
            {
                this._model.status = value;
                if (int.TryParse("123", out value) == false)
                {
                    this._model.status = 0;
                }
                this.OnPropertyChanged("status");
                _api.Orm.UpdateObject<AgendaEvent>(@"update agenda_event set status = @status where Id = @Id", _model);
                
            }
        }

    }
}
