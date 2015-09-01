using System;
using System.Collections.Generic;
using Service;
using EmployeeManagerModule.Model;

namespace EmployeeManagerModule.ViewModel
{
    /// <summary>
    /// Common functionality for ViewModels of an individual ContactDetail
    /// </summary>
    public abstract class ContactDetailViewModel : ViewModelBase
    {
        /// <summary>
        /// Gets the values that can be assigned to the Usage property of this ViewModel
        /// </summary>
        public IEnumerable<string> ValidUsageValues
        {
            get { return new string[] { "Business", "Home", "Cell" }; }
        }

        /// <summary>
        /// Gets the underlying ContactDetail this ViewModel is based on
        /// </summary>
        public abstract ContactDetail Model { get; }

        /// <summary>
        /// Gets or sets how this detail should be used, i.e. Home/Business etc.
        /// Possible values are available from the ValidUsageValues property
        /// </summary>
        public string Usage
        {
            get
            {
                return this.Model.usage;
            }

            set
            {
                this.Model.usage = value;
                this.OnPropertyChanged("usage");
            }
        }

        /// <summary>
        /// Constructs a view model to represent the supplied ContactDetail
        /// </summary>
        /// <param name="detail">The detail to build a ViewModel for</param>
        /// <returns>The constructed ViewModel, null if one can't be built</returns>
        public static ContactDetailViewModel BuildViewModel(ContactDetail detail)
        {
            if (detail == null)
            {
                throw new ArgumentNullException("detail");
            }

            Email e = detail as Email;
            if (e != null)
            {
                return new EmailViewModel(e);
            }

            Phone p = detail as Phone;
            if (p != null)
            {
                return new PhoneViewModel(p);
            }

            Address a = detail as Address;
            if (a != null)
            {
                return new AddressViewModel(a);
            }

            return null;
        }
    }
}
