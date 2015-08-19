using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service;

namespace BMS.ViewModel
{
    public class LoginViewModel : BindableBase, ILoginViewModel
    {
        //   private readonly IModuleCatalog _catalog;
           private readonly IRegionManager _manager;
           private  IUnityContainer _container;
           private IAPI _api;   
        //   private string _selectedModuleInTheList;


        private readonly DelegateCommand<string> _exitClickCommand;
        private readonly DelegateCommand<string> _loginClickCommand;
        

        public LoginViewModel(IRegionManager RegionManager, IUnityContainer Container, IAPI api)
        {
            this._manager = RegionManager;
            this._container = Container;
            this._api = api;

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
//            System.Console.Error.WriteLine("On capte l event du login");
            System.Console.Error.WriteLine(Login);
            System.Console.Error.WriteLine(Password);
            System.Console.Error.WriteLine("En attente de l ORM pour faire les reauetes en base et check si le log/mdp est ok");
            var caca =          this._api.Orm.Query("SELECT LOGIN, PWD FROM user WHERE LOGIN = @login AND PWD = @pwd", new {login = Login, pwd = Password});
            int count = 0;
            foreach (dynamic toto in caca)
            {
                count++;
                System.Console.Error.WriteLine(toto.LOGIN);
                System.Console.Error.WriteLine(toto.PWD);
            }
            if (count > 0)
            {
                var theView = this._manager.Regions["MainContentRegion"].GetView("LoginView");
                this._manager.Regions["MainContentRegion"].Remove(theView);
                var view = this._container.Resolve<View.MainView>();
                this._manager.Regions["MainContentRegion"].Add(view);
            }
            else
            {
                System.Console.Error.WriteLine("Afficher un message d'erreur");
            }
            
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
