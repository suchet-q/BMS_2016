using Microsoft.Practices.Unity;
using Service;
using Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BMS.ViewModel
{

    public delegate void OnLoginHandler(object sender, EventArgs e);

    public class LoginViewModel : ViewModelBase
    {
        IAPI _api;
        IUnityContainer _container;

        public event OnLoginHandler EventLogin;

        public User LoggedUser { get; set; }

        public LoginViewModel(IAPI api, IUnityContainer container)
        {
            _api = api;
            _container = container;

            this.LoginCommand = new DelegateCommand((o) => this.ExecuteLogin(), (o) => this.CanExecuteLogin());
            this.DisplayDatabaseErrMsg = false;
            this.DisplayConnexionErrMsg = false;

            this.ConnectDatabaseCommand = new DelegateCommand((o) => this.ConnectDatabase());
            this.Host = api.Orm.Server;
            this.Database = api.Orm.Database;
            this.Port = api.Orm.Port;
            this.DbLogin = api.Orm.Uid;
            this.DbPassword = api.Orm.Password;
            this.DisplayDatabaseParameter = false;

            this.DisplayDatabaseParamCommand = new DelegateCommand((o) => this.DisplayDatabaseParamExecute());
        }

        protected virtual void OnLogin(EventArgs e)
        {
            if (EventLogin != null)
                EventLogin(this, e);
        }


        private bool _displayDatabaseErrMsg;
        public bool DisplayDatabaseErrMsg
        {
            get
            {
                return _displayDatabaseErrMsg;
            }
            set
            {
                if (_displayDatabaseErrMsg == value) return;
                _displayDatabaseErrMsg = value;
                this.OnPropertyChanged("DisplayDatabaseErrMsg");
            }
        }

        private bool _displayConnexionErrMsg;
        public bool DisplayConnexionErrMsg
        {
            get
            {
                return _displayConnexionErrMsg;
            }
            set
            {
                if (_displayConnexionErrMsg == value) return;
                _displayConnexionErrMsg = value;
                this.OnPropertyChanged("DisplayConnexionErrMsg");
            }
        }

        private bool _displayConnexionSuccMsg;
        public bool DisplayConnexionSuccMsg
        {
            get
            {
                return _displayConnexionSuccMsg;
            }
            set
            {
                if (_displayConnexionSuccMsg == value) return;
                _displayConnexionSuccMsg = value;
                this.OnPropertyChanged("DisplayConnexionSuccMsg");
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


        string _realPassword;
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
                if (_password == null)
                    _password = "";
                if (value.Length > _password.Length)
                {
                    for (int i = value.Length - (value.Length - _password.Length); i < value.Length; ++i)
                        _realPassword += value[i];
                    _password = "";
                    foreach (char c in value)
                    {
                        _password += '*';
                    }
                }
                else if (value.Length < _password.Length)
                {
                    _realPassword = _realPassword.Remove(_realPassword.Length - (_password.Length - value.Length));
                    _password = _password.Remove(_password.Length - (_password.Length - value.Length));  
                }

//                _password = value;
                this.OnPropertyChanged("Password");
            }
        }

        private void ExecuteLogin()
        {
            IEnumerable<User> res = _api.Orm.ObjectQuery<User>("select * from user where login=@login and pwd=@password", new { login = this.Login, password = _api.CalculateMD5Hash(this._realPassword) });
            if (res != null)
            {
                int count = 0;
                foreach (User user in res)
                    count++;
                if (count > 0)
                {
                    this.DisplayConnexionErrMsg = false;
                    this.DisplayDatabaseErrMsg = false;
                    this.DisplayConnexionSuccMsg = true;
                    this._api.LoggedUser = res.First();
                    this.OnLogin(EventArgs.Empty);
                }
                else
                {
                    this.DisplayDatabaseErrMsg = false;
                    this.DisplayConnexionSuccMsg = false;
                    this.DisplayConnexionErrMsg = true;
                    this.Password = "";
                }
            }
            else
            {
                this.DisplayConnexionErrMsg = false;
                this.DisplayConnexionSuccMsg = false;
                this.DisplayDatabaseErrMsg = true;
            }
        }

        private bool CanExecuteLogin()
        {
            //if (!string.IsNullOrWhiteSpace(_login) && !string.IsNullOrWhiteSpace(_password))
            //   return true;
            return true;
        }

        public ICommand LoginCommand { get; set; }


        private void ConnectDatabase()
        {
            System.Console.Error.WriteLine("Change setting de la bdd plz");
            _api.Orm.Initialize(this.Host, this.Database, this.Port, this.DbLogin, this._realDbPassword);
        }

        private void DisplayDatabaseParamExecute()
        {
            this.DisplayDatabaseParameter = true;
        }

        public ICommand ConnectDatabaseCommand { get; set; }
        public ICommand DisplayDatabaseParamCommand { get; set; }


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

        string _dbLogin;
        public string DbLogin
        {
            get
            {
                return _dbLogin;
            }
            set
            {
                if (_dbLogin == value) return;
                _dbLogin = value;
                this.OnPropertyChanged("DbLogin");
            }
        }

        string _realDbPassword;
        string _dbPassword;
        public string DbPassword
        {
            get
            {
                return _dbPassword;
            }
            set
            {
                if (_dbPassword == value) return;
                if (_dbPassword == null)
                    _dbPassword = "";
                if (value.Length > _dbPassword.Length)
                {
                    for (int i = value.Length - (value.Length - _dbPassword.Length); i < value.Length; ++i)
                        _realDbPassword += value[i];
                    _dbPassword = "";
                    foreach (char c in value)
                    {
                        _dbPassword += '*';
                    }
                }
                else if (value.Length < _dbPassword.Length)
                {
                    _realDbPassword = _realDbPassword.Remove(_realDbPassword.Length - (_dbPassword.Length - value.Length));
                    _dbPassword = _dbPassword.Remove(_dbPassword.Length - (_dbPassword.Length - value.Length));
                }

                //                _password = value;
                this.OnPropertyChanged("DbPassword");
            }
        }

        bool _displayDatabaseParameter;
        public bool DisplayDatabaseParameter
        { 
            get
            {
                return _displayDatabaseParameter;
            }
            set
            {
                if (_displayDatabaseParameter == value) return;
                _displayDatabaseParameter = value;
                OnPropertyChanged("DisplayDatabaseParameter");
            }
        }

   }
}
