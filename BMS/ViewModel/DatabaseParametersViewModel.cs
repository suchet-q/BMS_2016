using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BMS.ViewModel
{
    public class DatabaseParametersViewModel : ViewModelBase
    {
        IAPI _api;

        public DatabaseParametersViewModel(IAPI api)
        {
            _api = api;
            this.ConnectDatabaseCommand = new DelegateCommand((o) => this.ConnectDatabase());
            this.Host = api.Orm.Server;
            this.Database = api.Orm.Database;
            this.Port = api.Orm.Port;
            this.Login = api.Orm.Uid;
            this.Password = api.Orm.Password;
        }

        private void ConnectDatabase()
        {
            _api.Orm.Initialize(this.Host, this.Database, this.Port, this.Login, this.Password);
        }

        public ICommand ConnectDatabaseCommand { get; set; }


        string _host;
        public string Host
        { 
            get
            {
                return _host;
            }
            set
            {
                if (_host == value) return;
                _host = value;
                this.OnPropertyChanged("Host");
            }
        }

        string _database;
        public string Database
        {
            get
            {
                return _database;
            }
            set
            {
                if (_database == value) return;
                _database = value;
                this.OnPropertyChanged("Database");
            }
        }

        int _port;
        public int Port
        {
            get
            {
                return _port;
            }
            set
            {
                if (_port == value) return;
                _port = value;
                this.OnPropertyChanged("Port");
            }
        }

        string _login;
        public string Login
        {
            get
            {
                return _login;
            }
            set
            {
                if (_login == value) return;
                _login = value;
                this.OnPropertyChanged("Login");
            }
        }

        string _password;
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                if (_password == value) return;
                _password = value;
                this.OnPropertyChanged("Password");
            }
        }
    }
}
