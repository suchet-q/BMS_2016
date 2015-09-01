using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagerModule.Model
{
    public class Department
    {
        /// <summary>
        /// The Employees that belong to this Department
        /// </summary>
        private ICollection<Employee> employees;

        /// <summary>
        /// Initializes a new instance of the Department class.
        /// </summary>
        public Department()
        {
            // Wire up the employees collection to sync references
            // NOTE: When running against Entity Framework with change tracking proxies this logic will not get executed
            //       because the Employees property will get over-ridden and replaced with an EntityCollection<Employee>.
            //       The EntityCollection will perform this fixup instead.
            ObservableCollection<Employee> emps = new ObservableCollection<Employee>();
            this.employees = emps;
            emps.CollectionChanged += (sender, e) =>
            {
                // Set the reference on any employees being added to this department
                if (e.NewItems != null)
                {
                    foreach (Employee item in e.NewItems)
                    {
                        if (item.Department != this)
                        {
                            item.Department = this;
                        }
                    }
                }

                // Clear the reference on any employees being removed that still points to this department
                if (e.OldItems != null)
                {
                    foreach (Employee item in e.OldItems)
                    {
                        if (item.Department == this)
                        {
                            item.Department = null;
                        }
                    }
                }
            };
        }

        public int id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string last_audited { get; set; }

        /// <summary>
        /// Gets or sets the employees that belong to this Department
        /// Adding or removing will fixup the department property on the affected employee
        /// </summary>
        public virtual ICollection<Employee> Employees
        {
            get { return this.employees; }
            set { this.employees = value; }
        }
    }
}
