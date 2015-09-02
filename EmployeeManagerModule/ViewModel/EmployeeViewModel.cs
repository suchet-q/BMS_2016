using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using EmployeeManagerModule.Model;
using Service;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using Microsoft.Practices.ServiceLocation;

namespace EmployeeManagerModule.ViewModel
{
    /// <summary>
    /// ViewModel of an individual <see cref="Employee"/>
    /// </summary>
    public class EmployeeViewModel : BasicEmployeeViewModel
    {
        /// <summary>
        /// The department currently assigned to this Employee
        /// </summary>
        //private DepartmentViewModel department;

        /// <summary>
        /// The manager currently assigned to this Employee
        /// </summary>
        private EmployeeViewModel manager;

        /// <summary>
        /// The contact detail currently selected
        /// </summary>
        private ContactDetailViewModel currentContactDetail;
        private Employee currentEmployee;

        IAPI _api;

        private void buildContactDetail()
        {
            //Build Phone
            IEnumerable<Phone> listPhone = _api.Orm.ObjectQuery<Phone>(" select phone.id, number, extension, contact_detail_id from phone inner join contactdetail on contactdetail.id_contact_detail=phone.contact_detail_id where contactdetail.employee_id=@employee_id ", new { employee_id = this.currentEmployee.id });
            if (listPhone != null)
            {
                foreach (Phone phone in listPhone)
                {
                    this.currentEmployee.ContactDetails.Add(phone);
                }
            }
            else
            {
                //Message d erreur
            }

            //Build Address
            IEnumerable<Address> listAddress = _api.Orm.ObjectQuery<Address>(" select address.id, lineone, linetwo, city, state, zipcode, country, contact_detail_id from address inner join contactdetail on contactdetail.id_contact_detail=address.contact_detail_id where contactdetail.employee_id=@employee_id ", new { employee_id = this.currentEmployee.id });
            if (listAddress != null)
            {
                foreach (Address address in listAddress)
                {
                    this.currentEmployee.ContactDetails.Add(address);
                }
            }
            else
            {
                //Message d erreur
            }

            //Build Email
            IEnumerable<Email> listEmail = _api.Orm.ObjectQuery<Email>(" select email.id, address, contact_detail_id from email inner join contactdetail on contactdetail.id_contact_detail=email.contact_detail_id where contactdetail.employee_id=@employee_id ", new { employee_id = this.currentEmployee.id });
            if (listEmail != null)
            {
                foreach (Email email in listEmail)
                {
                    this.currentEmployee.ContactDetails.Add(email);
                }
            }
            else
            {
                //Message d erreur
            }
        }

        /// <summary>
        /// Initializes a new instance of the EmployeeViewModel class.
        /// </summary>
        /// <param name="employee">The underlying Employee this ViewModel is to be based on</param>
        /// <param name="managerLookup">Existing collection of employees to use as a manager lookup</param>
        /// <param name="departmentLookup">Existing collection of departments to use as a department lookup</param>
        /// <param name="unitOfWork">UnitOfWork for managing changes</param>
        public EmployeeViewModel(Employee employee, IAPI api)
            : base(employee, api)
        {
            _api = api;
            if (employee == null)
            {
                throw new ArgumentNullException("employee");
            }
            currentEmployee = employee;

            //this.unitOfWork = unitOfWork;
            //this.ManagerLookup = managerLookup;
            //this.DepartmentLookup = departmentLookup;

            buildContactDetail();

            // Build data structures for contact details
            this.ContactDetails = new ObservableCollection<ContactDetailViewModel>();
            foreach (var detail in employee.ContactDetails)
            {
                ContactDetailViewModel vm = ContactDetailViewModel.BuildViewModel(detail);
                if (vm != null)
                {
                    this.ContactDetails.Add(vm);
                }
            }
/*
            // Re-act to any changes from outside this ViewModel
            this.DepartmentLookup.CollectionChanged += (sender, e) =>
            {
                if (e.OldItems != null && e.OldItems.Contains(this.Department))
                {
                    this.Department = null;
                }
            };
            */


            this.AddEmailAddressCommand = new DelegateCommand((o) => this.AddContactDetail<Email>());
            this.AddPhoneNumberCommand = new DelegateCommand((o) => this.AddContactDetail<Phone>());
            this.AddAddressCommand = new DelegateCommand((o) => this.AddContactDetail<Address>());
            this.DeleteContactDetailCommand = new DelegateCommand((o) => this.DeleteCurrentContactDetail(), (o) => this.CurrentContactDetail != null);
        }

        public void setManagerLookup(IUnityContainer container)
        {
            this.ManagerLookup = container.Resolve(typeof(object), "EmployeeList") as ObservableCollection<EmployeeViewModel>;
            this.ManagerLookup.CollectionChanged += (sender, e) =>
            {
                if (e.OldItems != null && e.OldItems.Contains(this.Manager))
                {
                    this.Manager = null;
                }
            };
        }

        /// <summary>
        /// Gets the command for adding a new Email address
        /// </summary>
        public ICommand AddEmailAddressCommand { get; private set; }

        /// <summary>
        /// Gets the command for adding a new phone number
        /// </summary>
        public ICommand AddPhoneNumberCommand { get; private set; }

        /// <summary>
        /// Gets the command for adding a new address
        /// </summary>
        public ICommand AddAddressCommand { get; private set; }

        /// <summary>
        /// Gets the command for deleting the current employee
        /// </summary>
        public ICommand DeleteContactDetailCommand { get; private set; }

        /// <summary>
        /// Gets or sets the currently selected contact detail
        /// </summary>

        public ContactDetailViewModel CurrentContactDetail
        {
            get
            {
                return this.currentContactDetail;
            }

            set
            {
                this.currentContactDetail = value;
                this.OnPropertyChanged("CurrentContactDetail");
            }
        }
/*   
        /// <summary>
        /// Gets or sets the department currently assigned to this Employee
        /// </summary>
        public DepartmentViewModel Department
        {
            get
            {
                // We need to reflect any changes made in the model so we check the current value before returning
                if (this.Model.Department == null)
                {
                    return null;
                }
                else if (this.department == null || this.department.Model != this.Model.Department)
                {
                    this.department = this.DepartmentLookup.Where(d => d.Model == this.Model.Department).SingleOrDefault();
                }

                return this.department;
            }

            set
            {
                this.department = value;
                this.Model.Department = (value == null) ? null : value.Model;
                this.OnPropertyChanged("Department");
            }
        }
*/
        /// <summary>
        /// Gets or sets the manager currently assigned to this Employee
        /// </summary>

        public EmployeeViewModel Manager
        {
            get
            {
                // We need to reflect any changes made in the model so we check the current value before returning
                if (this.Model.Manager == null)
                {
                    return null;
                }
                else if (this.manager == null || this.manager.Model != this.Model.Manager)
                {
                    this.manager = this.ManagerLookup.Where(e => e.Model == this.Model.Manager).SingleOrDefault();
                }

                return this.manager;
            }

            set
            {
                this.manager = value;
                this.Model.Manager = (value == null) ? null : value.Model;
                this.OnPropertyChanged("Manager");
            }
        }

        /// <summary>
        /// Gets a collection of departments this employee could be assigned to
        /// </summary>
        //public ObservableCollection<DepartmentViewModel> DepartmentLookup { get; private set; }

        /// <summary>
        /// Gets a collection of employees who could be this employee's manager
        /// </summary>
        ObservableCollection<EmployeeViewModel> _managerLookup;
        public ObservableCollection<EmployeeViewModel> ManagerLookup 
        {
            get
            {
                return _managerLookup;
            }
            set
            {
                if (_managerLookup == value) return;
                _managerLookup = value;
                this.OnPropertyChanged("ManagerLookup");

                foreach (EmployeeViewModel manager in _managerLookup)
                {
                    if (manager.Model.id == this.Model.manager_id)
                    {
                        this.Manager = manager;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the contact details on file for this employee
        /// </summary>
        public ObservableCollection<ContactDetailViewModel> ContactDetails { get; private set; }

        public ContactDetail BuildModel<T>()
        {
            _api.Orm.Insert("insert into contactdetail (`employee_id`, `usage`) values (@employee_id, @usage)", new { employee_id = currentEmployee.id, usage = "Email" });
            IEnumerable<dynamic> res = _api.Orm.Query("select max(id_contact_detail) as maxId from contactdetail");
            if (res != null)
            {
                Type typeTemplete = typeof(T);
                if (typeTemplete == typeof(Email))
                {
                    Email email = new Email();
                    email.id_contact_detail = (int)res.First().maxId;
                    _api.Orm.Insert("insert into email (`contact_detail_id`) values (@contact_detail_id)", new { contact_detail_id = email.id_contact_detail });
                    res = _api.Orm.Query("select max(id) as maxId from email");
                    email.id = (int)res.First().maxId;
                    return email;
                }
                else if (typeTemplete == typeof(Phone))
                {
                    Phone phone = new Phone();
                    phone.id_contact_detail = (int)res.First().maxId;
                    _api.Orm.Insert("insert into phone (`contact_detail_id`) values (@contact_detail_id)", new { contact_detail_id = phone.id_contact_detail });
                    res = _api.Orm.Query("select max(id) as maxId from phone");
                    phone.id = (int)res.First().maxId;
                    return phone;
                }
                else if (typeTemplete == typeof(Address))
                {
                    Address address = new Address();
                    address.id_contact_detail = (int)res.First().maxId;
                    _api.Orm.Insert("insert into address (`contact_detail_id`) values (@contact_detail_id)", new { contact_detail_id = address.id_contact_detail });
                    res = _api.Orm.Query("select max(id) as maxId from address");
                    address.id = (int)res.First().maxId;
                    return address;
                }
            }
            return null;
        }

        /// <summary>
        /// Handles addition a new contact detail to this employee
        /// </summary>
        /// <typeparam name="T">The type of contact detail to be added</typeparam>

        private void AddContactDetail<T>() where T : ContactDetail
        {
            ContactDetail detail = this.BuildModel<T>();

            ContactDetailViewModel vm = ContactDetailViewModel.BuildViewModel(detail);
            this.ContactDetails.Add(vm);
            this.CurrentContactDetail = vm;
        }

        /// <summary>
        /// Handles deletion of the current employee
        /// </summary>
        private void DeleteCurrentContactDetail()
        {
            EmailViewModel evm = this.CurrentContactDetail as EmailViewModel;
            if (evm != null)
            {
                _api.Orm.Delete("delete from email where contact_detail_id=@contact_detail_id", new { contact_detail_id = this.CurrentContactDetail.Model.id_contact_detail });
            }
            _api.Orm.Delete("delete from contactdetail where id_contact_detail=@id_contact_detail", new { id_contact_detail = this.CurrentContactDetail.Model.id_contact_detail });
            //CurrentContactDetail
            //_api.Orm.Delete("delete from user where user.id=@idUser", new { idUser = this.CurrentContactDetail.Model.id });
            //this.unitOfWork.RemoveContactDetail(this.Model, this.CurrentContactDetail.Model);
            this.ContactDetails.Remove(this.CurrentContactDetail);
            this.CurrentContactDetail = null;
        }

    }
}
