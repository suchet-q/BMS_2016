﻿using Microsoft.Practices.Unity;
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

        public LoginViewModel(IAPI api, IUnityContainer container)
        {
            _api = api;
            _container = container;

            this.LoginCommand = new DelegateCommand((o) => this.ExecuteLogin(), (o) => this.CanExecuteLogin());
            this.DisplayDatabaseErrMsg = false;
            this.DisplayConnexionErrMsg = false;
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

        private void ExecuteLogin()
        {
            IEnumerable<User> res = _api.Orm.ObjectQuery<User>("select * from user where login=@login and pwd=@password", new { login = this.Login, password = _api.CalculateMD5Hash(this.Password) });
            if (res != null)
            {
                int count = 0;
                foreach (User user in res)
                    count++;
                if (count > 0)
                {
                    this.OnLogin(EventArgs.Empty);   
                }
                else
                {
                    this.DisplayDatabaseErrMsg = false;
                    this.DisplayConnexionErrMsg = true;
                    this.Password = "";
                }
            }
            else
            {
                this.DisplayDatabaseErrMsg = true;
                this.DisplayConnexionErrMsg = false;
            }
        }

        private bool CanExecuteLogin()
        {
            //if (!string.IsNullOrWhiteSpace(_login) && !string.IsNullOrWhiteSpace(_password))
            //   return true;
            return true;
        }

        public ICommand LoginCommand { get; set; }
    }
}
