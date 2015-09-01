using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Service;

namespace EmployeeManagerModule.Model
{
    public class Employee : BasicEmployee
    {
        /// <summary>
        /// Contact details belonging to this Employee
        /// </summary>
        private ICollection<ContactDetail> details;

        /// <summary>
        /// The Employees that report to this Employee
        /// </summary>
        private ICollection<Employee> reports;

        /// <summary>
        /// The Department this Employee belongs to
        /// </summary>
        private Department department;

        /// <summary>
        /// The manager of this Employee
        /// </summary>
        private Employee manager;

        /// <summary>
        /// Initializes a new instance of the Employee class.
        /// </summary>
        public Employee()
        {
            Initialized();
        }

        private void Initialized()
        {
            // NOTE: No fixup is required as this is a uni-directional navigation
            this.details = new ObservableCollection<ContactDetail>();

            // Wire up the reports collection to sync references
            // NOTE: When running against Entity Framework with change tracking proxies this logic will not get executed
            //       because the Reports property will get over-ridden and replaced with an EntityCollection<Employee>.
            //       The EntityCollection will perform this fixup instead.
            ObservableCollection<Employee> reps = new ObservableCollection<Employee>();
            this.reports = reps;
            reps.CollectionChanged += (sender, e) =>
            {
                // Set the reference on any employees being added to this manager
                if (e.NewItems != null)
                {
                    foreach (Employee item in e.NewItems)
                    {
                        if (item.Manager != this)
                        {
                            item.Manager = this;
                        }
                    }
                }

                // Clear the reference on any employees being removed that still points to this manager
                if (e.OldItems != null)
                {
                    foreach (Employee item in e.OldItems)
                    {
                        if (item.Manager == this)
                        {
                            item.Manager = null;
                        }
                    }
                }
            };
        }
        /*
        public Employee(BasicEmployee be)
        {
            this.id = be.id;
            this.title = be.title;
            this.firstname = be.firstname;
            this.lastname = be.lastname;
            this.position = be.position;
            this.departement_id = be.departement_id;
            this.manager_id = be.manager_id;
            //this.hiredate = be.hiredate;
            Initialized();
        }
        */

       /// <summary>
        /// Gets or sets the contact details of this Employee
        /// No fixup is performed as this is a uni-directional navigation
        /// </summary>
        public virtual ICollection<ContactDetail> ContactDetails
        {
            get { return this.details; }
            set { this.details = value; }
        }

        /// <summary>
        /// Gets or sets the employees that report to this Employee
        /// Adding or removing will fixup the manager property on the affected employee
        /// </summary>
        public virtual ICollection<Employee> Reports
        {
            get { return this.reports; }
            set { this.reports = value; }
        }

        /// <summary>
        /// Gets or sets the Department this Employees belongs to
        /// Setting this property will fixup the collection on the original and new department
        /// </summary>
        public virtual Department Department
        {
            get
            {
                return this.department;
            }

            set
            {
                if (value != this.department)
                {
                    Department original = this.department;
                    this.department = value;

                    // Remove from old collection
                    if (original != null && original.Employees.Contains(this))
                    {
                        original.Employees.Remove(this);
                    }

                    // Add to new collection
                    if (value != null && !value.Employees.Contains(this))
                    {
                        value.Employees.Add(this);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets this Employees manager
        /// Setting this property will fixup the collection on the original and new manager
        /// </summary>
        public virtual Employee Manager
        {
            get
            {
                return this.manager;
            }

            set
            {
                if (value != this.manager && value != null)
                {
                    Employee original = this.manager;
                    this.manager = value;
                    this.manager_id = value.id;

                    // Remove from old collection
                    if (original != null && original.Reports.Contains(this))
                    {
                        original.Reports.Remove(this);
                    }

                    // Add to new collection
                    if (value != null && !value.Reports.Contains(this))
                    {
                        value.Reports.Add(this);
                    }
                    IUnityContainer container = ServiceLocator.Current.GetInstance<IUnityContainer>();
                    IAPI api = container.Resolve<IAPI>();
                    api.Orm.Update("update basicemployee set manager_id = @manager_id where Id = @Id", new { manager_id = this.manager.id, Id = this.id });
                }
            }
        }
    }
}
