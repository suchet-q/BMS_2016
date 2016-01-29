using Microsoft.Practices.Unity;
using Service;
using Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoleManagerModule.ViewModel
{
    public class RightRoleModuleViewModel : ViewModelBase
    {
        /// <summary>
        /// The Phone object backing this ViewModel
        /// </summary>
        private RightRoleModule rightRoleModule;
        private IAPI _api;
        private Role currentRole;

        /// <summary>
        /// Initializes a new instance of the PhoneViewModel class.
        /// </summary>
        /// <param name="rightRoleModule">The underlying Phone this ViewModel is to be based on</param>
        public RightRoleModuleViewModel(Role role, IAPI api, RightRoleModule rightRoleModule)
        {
            _api = api;
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }
            currentRole = role;

            this.rightRoleModule = rightRoleModule;
        }

        public RightRoleModule Model
        {
            get { return this.rightRoleModule; }
        }

        public string NomModule
        {
            get { return this.rightRoleModule.nom_module; }
            set
            {
                if (this.rightRoleModule.nom_module == value) return;
                this.rightRoleModule.nom_module = value;
                this.OnPropertyChanged("nom_module");
               // _api.Orm.Update("update right_role_module set nom_module = @nom_module where id = @id", new { nom_module = value, id = this.rightRoleModule.id });
            }
        }

        public bool RightRead
        {
            get {return this.rightRoleModule.right_read; }
            set
            {
                if (this.rightRoleModule.right_read == value) return;
                this.rightRoleModule.right_read = value;
                this.OnPropertyChanged("RightRead");
                //_api.Orm.Update("update right_role_module set right_read = @right_read where id = @id", new { right_read = value, id = this.rightRoleModule.id });
            }
        }

        public bool RightCreate
        {
            get { return this.rightRoleModule.right_create; }
            set
            {
                if (this.rightRoleModule.right_create == value) return;
                this.rightRoleModule.right_create = value;
                this.OnPropertyChanged("RightCreate");
                //_api.Orm.Update("update right_role_module set right_create = @right_create where id = @id", new { right_create = value, id = this.rightRoleModule.id });
            }
        }

        public bool RightUpdate
        {
            get { return this.rightRoleModule.right_update; }
            set
            {
                if (this.rightRoleModule.right_update == value) return;
                this.rightRoleModule.right_update = value;
                this.OnPropertyChanged("RightUpdate");
                //_api.Orm.Update("update right_role_module set right_update = @right_update where id = @id", new { right_update = value, id = this.rightRoleModule.id });
            }
        }

        public bool RightDelete
        {
            get { return this.rightRoleModule.right_delete; }
            set
            {
                if (this.rightRoleModule.right_delete == value) return;
                this.rightRoleModule.right_delete = value;
                this.OnPropertyChanged("RightDelete");
                //_api.Orm.Update("update right_role_module set right_delete = @right_delete where id = @id", new { right_delete = value, id = this.rightRoleModule.id });
            }
        }
    }
}
