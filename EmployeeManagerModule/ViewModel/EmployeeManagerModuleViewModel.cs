using EmployeeManagerModule.Model;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Service;
using Service.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EmployeeManagerModule.ViewModel
{
    public class EmployeeManagerModuleViewModel : ViewModelBase
    {
        /// <summary>
        /// Gets the command for adding a new employee
        /// </summary>
        public ICommand AddEmployeeCommand { get; private set; }

        /// <summary>
        /// Gets the command for deleting the current employee
        /// </summary>
        public ICommand DeleteEmployeeCommand { get; private set; }

        /// <summary>
        /// Gets all employees whithin the company
        /// </summary>
        public ObservableCollection<EmployeeViewModel> AllEmployees { get; private set; }

        /// <summary>
        /// The employee currently selected in this workspace
        /// </summary>
        private EmployeeViewModel currentEmployee;
        /// <summary>
        /// Gets or sets the employee currently selected in this workspace
        /// </summary>
        public EmployeeViewModel CurrentEmployee
        {
            get
            {
                return this.currentEmployee;
            }

            set
            {
                this.currentEmployee = value;
                this.OnPropertyChanged("CurrentEmployee");
            }
        }

        IAPI                    _api;

        private ObservableCollection<Employee>          _listAllEmployees;

        /// <summary>
        /// Departements to be used for lookups
        /// </summary>
        //private ObservableCollection<DepartementViewModel> departmentLookup;

        /// <summary>
        /// Initializes a new instance of the EmployeeWorkspaceViewModel class.
        /// </summary>
        /// <param name="employees">Employees to be managed</param>
        /// <param name="departmentLookup">The departments to be used for lookups</param>
        /// <param name="unitOfWork">UnitOfWork for managing changes</param>
        public EmployeeManagerModuleViewModel(IAPI api, IUnityContainer container)
        {
            _api = api;

            // Get list de la BD
            IEnumerable<Employee> listEmployee = _api.Orm.ObjectQuery<Employee>("select * from basicemployee");
            if (listEmployee != null)
            {
                this.AllEmployees = new ObservableCollection<EmployeeViewModel>();
                foreach (Employee employee in listEmployee)
                {
                    this.AllEmployees.Add(new EmployeeViewModel(employee, _api));
                }

                if (AllEmployees == null)
                {
                    throw new ArgumentNullException("employees");
                }
                //this.departmentLookup = departmentLookup;
                this.CurrentEmployee = AllEmployees.Count > 0 ? AllEmployees[0] : null;

                container.RegisterInstance(typeof(object), "EmployeeList", AllEmployees);

                foreach (EmployeeViewModel employeeVM in AllEmployees)
                {
                    employeeVM.setManagerLookup(container);
                }

                // Re-act to any changes from outside this ViewModel
                this.AllEmployees.CollectionChanged += (sender, e) =>
                {
                    if (e.OldItems != null && e.OldItems.Contains(this.CurrentEmployee))
                    {
                        this.CurrentEmployee = null;
                    }
                };

                this.AddEmployeeCommand = new DelegateCommand((o) => this.AddEmployee());
                this.DeleteEmployeeCommand = new DelegateCommand((o) => this.DeleteCurrentEmployee(), (o) => this.CurrentEmployee != null);
            }
            else
            {
                //Message d erreur
            }
        }

        /// <summary>
        /// Handles addition a new employee to the workspace and model
        /// </summary>
        private void AddEmployee()
        {
            Employee employee = new Employee();
            _api.Orm.InsertObject(employee as BasicEmployee);
            IEnumerable<dynamic> res = _api.Orm.Query("select max(id) as maxId from basicemployee");
            if (res != null)
            {
                employee.id = (int)res.First().maxId;

                EmployeeViewModel vm = new EmployeeViewModel(employee, _api);
                this.AllEmployees.Add(vm);
                this.CurrentEmployee = vm;
            }
            else
            {
                //Message d erreur
            }
        }

        /// <summary>
        /// Handles deletion of the current employee
        /// </summary>
        private void DeleteCurrentEmployee()
        {
            //Delete de la base 
            _api.Orm.Delete("delete from basicemployee where basicemployee.id=@idEmployee", new { idEmployee = this.CurrentEmployee.Model.id });
            this.AllEmployees.Remove(this.CurrentEmployee);
            this.CurrentEmployee = null;
        }
    }
}
