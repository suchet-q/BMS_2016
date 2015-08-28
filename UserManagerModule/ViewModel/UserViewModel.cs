using Service;
using Service.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagerModule.ViewModel
{
    public class UserViewModel : ViewModelBase
    {
        User                        _user;
        public User                 Model { get; private set; }
        ObservableCollection<User>  _listUser;
        IAPI                        _api;

        public UserViewModel(User user, ObservableCollection<User> listUser, IAPI api)
        {
            _api = api;
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            System.Console.Error.WriteLine("User ID ===> " + user.id);
            _user = Model = user;
            _listUser = listUser;
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

        public string name
        {
            get
            {
                return this.Model.name;
            }

            set
            {
                this.Model.name = value;
                this.OnPropertyChanged("name");
                _api.Orm.UpdateObject<User>(@"update user set name = @name where Id = @Id", Model);
            }
        }
        public string last_name
        {
            get
            {
                return this.Model.last_name;
            }

            set
            {
                this.Model.last_name = value;
                this.OnPropertyChanged("last_name");
                _api.Orm.UpdateObject<User>(@"update user set last_name = @last_name where Id = @Id", Model);
            }
        }
        public string login
        {
            get
            {
                return this.Model.login;
            }

            set
            {
                this.Model.login = value;
                this.OnPropertyChanged("login");
                _api.Orm.UpdateObject<User>(@"update user set login = @login where Id = @Id", Model);
            }
        }
        public string pwd
        {
            get
            {
                return this.Model.pwd;
            }

            set
            {
                this.Model.pwd = value;
                this.OnPropertyChanged("pwd");

                _api.Orm.Update(@"update user set pwd = @Pwd where Id = @Id", new { Pwd = _api.CalculateMD5Hash(pwd), @Id = this.Id});
            }
        }

        /// <summary>
        /// Gets the text to display when referring to this User
        /// </summary>
        public string DisplayName
        {
            get { return string.Format(CultureInfo.InvariantCulture, "{0}, {1}", this.Model.last_name, this.Model.name); }
        }
    }
}
