using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Service;
using Service.Model;

namespace BMSModule.ViewModel
{
    public class UserListViewModel : ViewModelBase
    {
        IAPI _api;

        public ObservableCollection<User> ListUser
        {
            get;
            private set;
        }

        public UserListViewModel(IAPI api)
        {
            this._api = api;
            IEnumerable<User> listUser = _api.Orm.ObjectQuery<User>("select * from user");
            this.ListUser = new ObservableCollection<User>(listUser);
        }

        protected override void OnDispose()
        {
            this.ListUser.Clear();
        }
    }
}
