﻿using Service;
using Service.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UserManagerModule.ViewModel
{
    public class UserManagerModuleViewModel : ViewModelBase
    {
        IAPI                                        _api;

        private UserViewModel                       _currentUser;
        public UserViewModel                        CurrentUser
        {
            get
            {
                return _currentUser;
            }

            set
            {
                _currentUser = value;
                this.OnPropertyChanged("CurrentUser");
            }
        }

        public ObservableCollection<UserViewModel>  AllUsers { get; private set; }

        private ObservableCollection<User>          _listAllUsers;


        public UserManagerModuleViewModel(IAPI api)
        {
            _api = api;

            // Get list de la BD
            IEnumerable<User> listUser = _api.Orm.ObjectQuery<User>("select * from user");
            if (listUser == null)
            {
                listUser = new Collection<User>();                                
            }

            _listAllUsers = new ObservableCollection<User>(listUser);
            this.AllUsers = new ObservableCollection<UserViewModel>();
            foreach (User user in listUser)
            {
                this.AllUsers.Add(new UserViewModel(user, _listAllUsers, _api));
                System.Console.Error.WriteLine(user.name);
            }
            _currentUser = AllUsers.Count > 0 ? AllUsers[0] : null;

            this.AllUsers.CollectionChanged += (sender, e) =>
            {
                if (e.OldItems != null && e.OldItems.Contains(this.CurrentUser))
                {
                    this.CurrentUser = null;
                }
            };
            System.Console.Error.WriteLine("UserManagerModuleViewModel");
            this.GenerateCsvCommand = new DelegateCommand((o) => this.GenerateCsv());
            this.AddUserCommand = new DelegateCommand((o) => this.AddUser());
            this.DeleteUserCommand = new DelegateCommand((o) => this.DeleteCurrentUser(), (o) => this.CurrentUser != null);
            System.Console.Error.WriteLine("End UserManagerModuleViewModel");
        }

        async private void showAndHideGeneratedMsg()
        {
            this.DisplayGeneratedMsg = true;
            await Task.Run(() => { Thread.Sleep(5000); });
            this.DisplayGeneratedMsg = false;
        }

        private void GenerateCsv()
        {
            _api.GenerateCsv<User>(this._listAllUsers, null, true);
            this.showAndHideGeneratedMsg();
        }

        private void AddUser()
        {
            User user = new User();
            _api.Orm.InsertObject(user);
            IEnumerable<dynamic> res = _api.Orm.Query("select max(id) as maxId from user");
            if (res != null)
            {
                user.id = (int)res.First().maxId;
                UserViewModel vm = new UserViewModel(user, _listAllUsers, _api);
                this.AllUsers.Add(vm);
                this.CurrentUser = vm;
            }
            else
            {
                //Message d erreur
            }
        }

        private void DeleteCurrentUser()
        {
            //Delete de la base 
            _api.Orm.Delete("delete from user where user.id=@idUser", new { idUser=this.CurrentUser.Model.id });
            this.AllUsers.Remove(this.CurrentUser);
            this.CurrentUser = null;
        }

        public ICommand GenerateCsvCommand { get; private set; }
        public ICommand AddUserCommand { get; private set; }
        public ICommand DeleteUserCommand { get; private set; }


        bool _displayGeneratedMsg;
        public bool DisplayGeneratedMsg
        {
            get
            {
                return _displayGeneratedMsg;
            }
            set
            {
                if (_displayGeneratedMsg == value) return;
                _displayGeneratedMsg = value;
                this.OnPropertyChanged("DisplayGeneratedMsg");
            }
        }
    }
}
