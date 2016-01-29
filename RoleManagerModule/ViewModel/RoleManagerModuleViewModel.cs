using Microsoft.Practices.Unity;
using RoleManagerModule.ViewModel;
using Service;
using Service.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ViewModel
{
    public class RoleManagerModuleViewModel : ViewModelBase, IModuleMainViewModel
    {

        IAPI _api;
        IUnityContainer _container;
        IMetadataModuleCatalog _moduleCatalog;

        public IEnumerable<RightRoleModule> listRightRoleModules;

        private ObservableCollection<RightRoleModuleViewModel> currentRightRoleModules;

        public ObservableCollection<RightRoleModuleViewModel> CurrentRightRoleModules
        {
            get { return this.currentRightRoleModules; }
            set
            {
                this.currentRightRoleModules = value;
                this.OnPropertyChanged("CurrentRightRoleModules");
            }
        }

        ObservableCollection<Role> _listRole;
        public ObservableCollection<RoleViewModel> AllRoles { get; private set; }

        private SortedDictionary<RoleViewModel, ObservableCollection<RightRoleModuleViewModel>> DicAllRightRoleModuleViewModel;

        private RoleViewModel _currentRole;
        public RoleViewModel CurrentRole
        {
            get { return _currentRole; }
            set
            {
                _currentRole = value;
                if (DicAllRightRoleModuleViewModel.Keys.Contains(value))
                    CurrentRightRoleModules = DicAllRightRoleModuleViewModel[value];
                OnPropertyChanged("CurrentRole");
            }
        }


        public RoleManagerModuleViewModel(IAPI api, IUnityContainer container, IMetadataModuleCatalog moduleCatalog)
        {
            _api = api;
            _container = container;
            _moduleCatalog = moduleCatalog;

            Refresh();

            this.AddRoleCommand = new DelegateCommand((o) => this.AddRole());
            this.DeleteRoleCommand = new DelegateCommand((o) => this.DeleteCurrentRole());
        }

        private void createRoleRightAsso(RoleViewModel RVM)
        {
            Role role = RVM.Model;
            //Get RightRoleModules for this Role
            listRightRoleModules = _api.Orm.ObjectQuery<RightRoleModule>(" select * from right_role_module where role_id=@role_id ", new { role_id = role.id });
            //Create table if not exist
            if (listRightRoleModules == null)
                _api.Orm.Execute("CREATE TABLE right_role_module (id INT PRIMARY KEY NOT NULL, role_id INT NOT NULL, nom_module VARCHAR(100) NOT NULL, right_read BOOLEAN NOT NULL, right_create BOOLEAN NOT NULL, right_update BOOLEAN NOT NULL, right_delete BOOLEAN NOT NULL) ");

            List<string> listModule = _moduleCatalog.ModuleMetadata.Select(x => x.Name).ToList();
            ObservableCollection<RightRoleModuleViewModel> listRightRoleModuleViewModel = new ObservableCollection<RightRoleModuleViewModel>();
            foreach (var module in listModule)
            {
                //RightRoleModule rightRoleModule = listRightRoleModules.SingleOrDefault(x => x.nom_module == module);
                //If module nexiste pas dans la DB crée le
                //if (rightRoleModule == null)
                //{
                    RightRoleModule rightRoleModule = new RightRoleModule { nom_module=module, role_id=role.id, right_create=false, right_delete=false, right_read=false, right_update=false };
                    //_api.Orm.InsertObject<RightRoleModule>(rightRoleModule);
                    //IEnumerable<dynamic> res = _api.Orm.Query("select max(id) as maxId from right_role_module");
                    //if (res != null)
                    //    rightRoleModule.id = (int)res.First().maxId;
                //}
                //Create ViewModel
                RightRoleModuleViewModel vm = new RightRoleModuleViewModel(role, _api, rightRoleModule);
                listRightRoleModuleViewModel.Add(vm);
            }
            if (!DicAllRightRoleModuleViewModel.Keys.Contains(RVM))
                DicAllRightRoleModuleViewModel.Add(RVM, listRightRoleModuleViewModel);
        }

        private ObservableCollection<Role> buildEntryList()
        {

            IEnumerable<dynamic> roleBrutList = _api.Orm.Query("select * from role");
            if (roleBrutList == null)
            {
                roleBrutList = new Collection<dynamic>();
            }
            ObservableCollection<Role> res = new ObservableCollection<Role>();

            foreach (dynamic roleBrut in roleBrutList)
            {
                Role newElem = new Role();

                newElem.id = (int)roleBrut.id;
                newElem.nom = roleBrut.nom;
                res.Add(newElem);
            }
            return res;
        }

        private void AddRole()
        {
            Role role = new Role();
            _api.Orm.InsertObject(role);
            IEnumerable<dynamic> res = _api.Orm.Query("select max(id) as maxId from role");
            if (res != null)
            {
                role.id = (int)res.First().maxId;
                RoleViewModel vm = new RoleViewModel(role, this._listRole, _api, _container);
                this.AllRoles.Add(vm);
                createRoleRightAsso(vm);
                this.CurrentRole = vm;
            }
            else
            {
                //Message d erreur
            }

        }

        private void DeleteCurrentRole()
        {
            _api.Orm.Delete("delete from role where role.id=@idRole", new { idRole = this.CurrentRole.Model.id });
            DicAllRightRoleModuleViewModel.Remove(this.CurrentRole);
            this.AllRoles.Remove(this.CurrentRole);
            this.CurrentRole = this.AllRoles.Count() > 0 ? this.AllRoles.First() : null;
        }

        public void Refresh()
        {
            _listRole = this.buildEntryList();

            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                DicAllRightRoleModuleViewModel = new SortedDictionary<RoleViewModel, ObservableCollection<RightRoleModuleViewModel>>();
                this.AllRoles = new ObservableCollection<RoleViewModel>();
                foreach (Role role in _listRole)
                {
                    RoleViewModel RVM = new RoleViewModel(role, _listRole, _api, _container);
                    this.AllRoles.Add(RVM);
                    createRoleRightAsso(RVM);
                }

                CurrentRole = AllRoles.Count > 0 ? AllRoles[0] : null;

                this.AllRoles.CollectionChanged += (sender, e) =>
                {
                    if (e.OldItems != null && e.OldItems.Contains(this.CurrentRole))
                    {
                        this.CurrentRole = null;
                    }
                };
            });
        }

        public ICommand AddRoleCommand { get; private set; }
        public ICommand DeleteRoleCommand { get; private set; }

    }
}