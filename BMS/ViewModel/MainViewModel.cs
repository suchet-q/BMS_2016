using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.ViewModel
{
    public class MainViewModel : BindableBase, IMainViewModel
    {
        //   private readonly IModuleCatalog _catalog;
        //   private readonly IRegionManager _manager;

        //   private string _selectedModuleInTheList;


        private readonly DelegateCommand<string> _exitClickCommand;
        private readonly DelegateCommand<string> _loginClickCommand;

        public MainViewModel()
        {
            _exitClickCommand = new DelegateCommand<string>(
          (s) => { this.ExecuteOnexitClickCommand(); }, //Execute
          (s) => { return true; } //CanExecute
          );

            _loginClickCommand = new DelegateCommand<string>(
          (s) => { this.ExecuteOnloginClickCommand(); }, //Execute
          (s) => { return true; } //CanExecute
          );

        }
        public void ExecuteOnexitClickCommand()
        {
            System.Console.Error.WriteLine("On capte l exit");

           // Login = string.Empty;
           // _login = Login;
        }

        public void ExecuteOnloginClickCommand()
        {
            System.Console.Error.WriteLine("On capte l event du login");
            System.Console.Error.WriteLine(Login);
            System.Console.Error.WriteLine(Password);
            System.Console.Error.WriteLine("En attente de l ORM pour faire les reauetes en base et check si le log/mdp est ok");

          //  Login = string.Empty;
          //  _login = Login;
        }

        public DelegateCommand<string> ExitClickCommand
        {
            get { return _exitClickCommand; }
        }


        public DelegateCommand<string> LoginClickCommand
        {
            get { return _loginClickCommand; }
        }
        private string _login;
        private string _password;

        public string Login
        {
            get { return _login; }
            set
            {
                if (_login != value)
                {
                    _login = value;
                    OnPropertyChanged("Input");
                }
                _login = value;
                _exitClickCommand.RaiseCanExecuteChanged();
            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged("Input");
                }
                _password = value;
                _exitClickCommand.RaiseCanExecuteChanged();
            }
        }
    }
}
