using EmployeeManagerModule.Model;
using Service;
using System;
using System.Globalization;

namespace EmployeeManagerModule.ViewModel
{
    /// <summary>
    /// ViewModel of an individual Employee without associations
    /// EmployeeViewModel should be used if associations need to be displayed or edited
    /// </summary>
    public class BasicEmployeeViewModel : ViewModelBase
    {
        IAPI                        _api;
        /// <summary>
        /// Initializes a new instance of the BasicEmployeeViewModel class.
        /// </summary>
        /// <param name="employee">The underlying Employee this ViewModel is to be based on</param>
        public BasicEmployeeViewModel(Employee employee, IAPI api)
        {
            _api = api;
            if (employee == null)
            {
                throw new ArgumentNullException("employee");
            }

            this.Model = employee;
        }

        /// <summary>
        /// Gets the underlying Employee this ViewModel is based on
        /// </summary>
        public Employee Model { get; private set; }

        /// <summary>
        /// Gets or sets the first name of this employee
        /// </summary>
        public string FirstName
        {
            get
            {
                return this.Model.firstname;
            }

            set
            {
                this.Model.firstname = value;
                this.OnPropertyChanged("firstname");
                _api.Orm.UpdateObject<Employee>(@"update basicemployee set firstname = @firstname where Id = @Id", Model);
            }
        }

        /// <summary>
        /// Gets or sets the title of this employee
        /// </summary>
        public string Title
        {
            get
            {
                return this.Model.title;
            }

            set
            {
                this.Model.title = value;
                this.OnPropertyChanged("title");
                _api.Orm.UpdateObject<Employee>(@"update basicemployee set title = @title where Id = @Id", Model);
            }
        }

        /// <summary>
        /// Gets or sets the last name of this employee
        /// </summary>
        public string LastName
        {
            get
            {
                return this.Model.lastname;
            }

            set
            {
                this.Model.lastname = value;
                this.OnPropertyChanged("lastname");
                _api.Orm.UpdateObject<Employee>(@"update basicemployee set lastname = @lastname where Id = @Id", Model);
            }
        }

        /// <summary>
        /// Gets or sets the position this employee holds in the company
        /// </summary>
        public string Position
        {
            get
            {
                return this.Model.position;
            }

            set
            {
                this.Model.position = value;
                this.OnPropertyChanged("position");
                _api.Orm.UpdateObject<Employee>(@"update basicemployee set position = @position where Id = @Id", Model);
            }
        }

        /// <summary>
        /// Gets or sets this employees date of birth
        /// </summary>
        public DateTime BirthDate
        {
            get
            {
                return this.Model.birthdate;
            }

            set
            {
                this.Model.birthdate = value;
                this.OnPropertyChanged("birthdate");
                _api.Orm.UpdateObject<Employee>(@"update basicemployee set birthdate = @birthdate where Id = @Id", Model);
            }
        }

        /// <summary>
        /// Gets or sets the date this employee was hired by the company
        /// </summary>
        public DateTime HireDate
        {
            get
            {
                return this.Model.hiredate;
            }

            set
            {
                this.Model.hiredate = value;
                this.OnPropertyChanged("hiredate");
                _api.Orm.UpdateObject<Employee>(@"update basicemployee set hiredate = @hiredate where Id = @Id", Model);
            }
        }

        /// <summary>
        /// Gets or sets the date this employee left the company
        /// </summary>
        public DateTime TerminationDate
        {
            get
            {
                return this.Model.terminationdate;
            }

            set
            {
                this.Model.terminationdate = value;
                this.OnPropertyChanged("terminationdate");
                _api.Orm.UpdateObject<Employee>(@"update basicemployee set terminationdate = @terminationdate where Id = @Id", Model);
            }
        }

        /// <summary>
        /// Gets or sets the manager of the employee
        /// </summary>
        /*
        public Employee Manager
        {
            get
            {
                return this.Manager;
            }

            set
            {
                System.Console.Error.WriteLine("Value => " + value);
                System.Console.Error.WriteLine("Value => " + value.id);
                System.Console.Error.WriteLine("Value => " + value.manager_id);
                this.Model.manager_id = value.id;
                this.OnPropertyChanged("manager_id");
                _api.Orm.UpdateObject<Employee>(@"update basicemployee set manager_id = @manager_id where Id = @Id", Model);
            }
        }
         * */
        /// <summary>
        /// Gets the text to display when referring to this employee
        /// </summary>
        public string DisplayName
        {
            get { return string.Format(CultureInfo.InvariantCulture, "{0}, {1}", this.Model.lastname, this.Model.firstname); }
        }

        /// <summary>
        /// Gets the text to display for a readonly version of this employees hire date
        /// </summary>
        public string DisplayHireDate
        {
            get { return this.Model.hiredate.ToShortDateString(); }
        }
    }
}
