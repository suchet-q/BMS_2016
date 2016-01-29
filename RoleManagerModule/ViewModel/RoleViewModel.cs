using Microsoft.Practices.Unity;
using Service;
using Service.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoleManagerModule.ViewModel
{
    public class RoleViewModel : ViewModelBase, IComparable
    {

        public Role Model { get; private set; }
        IAPI _api;
        IUnityContainer _container;
        ObservableCollection<Role> _roleList;


        public RoleViewModel(Role role, ObservableCollection<Role> listRole, IAPI api, IUnityContainer container)
        {
            _api = api;
            _container = container;
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }
            Model = role;
            _roleList = listRole;
        }

        public int Id
        {
            get
            {
                return this.Model.id;
            }

            set
            {
                if (this.Model.id == value) return;
                this.Model.id = value;
                this.OnPropertyChanged("id");
            }
        }

        public string nom
        {
            get
            {
                return this.Model.nom;
            }

            set
            {
                if (this.Model.nom == value) return;
                this.Model.nom = value;
                this.OnPropertyChanged("nom");
                _api.Orm.UpdateObject<Role>(@"update role set nom = @nom where id = @id", Model);
            }
        }

        public int CompareTo(object obj)
        {
            if (obj is RoleViewModel)
            {
                var casted = (RoleViewModel)obj;
                return String.Compare(nom, casted.nom);
            }
            return 0;
        }
    }
}
