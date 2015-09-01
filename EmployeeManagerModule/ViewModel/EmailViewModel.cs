using EmployeeManagerModule.Model;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Service;
using System;

namespace EmployeeManagerModule.ViewModel
{
    /// <summary>
    /// ViewModel of an individual Email
    /// </summary>
    public class EmailViewModel : ContactDetailViewModel
    {
        /// <summary>
        /// The Email object backing this ViewModel
        /// </summary>
        private Email email;
        private IAPI _api;

        /// <summary>
        /// Initializes a new instance of the EmailViewModel class.
        /// </summary>
        /// <param name="detail">The underlying Email this ViewModel is to be based on</param>
        public EmailViewModel(Email detail)
        {
            if (detail == null)
            {
                throw new ArgumentNullException("detail");
            }

            IUnityContainer container = ServiceLocator.Current.GetInstance<IUnityContainer>();
            _api = container.Resolve<IAPI>();
            this.email = detail;
        }

        /// <summary>
        /// Gets the underlying Email this ViewModel is based on
        /// </summary>
        public override ContactDetail Model
        {
            get { return this.email; }
        }

        /// <summary>
        /// Gets or sets the actual email address
        /// </summary>
        public string Address
        {
            get
            {
                return this.email.address;
            }

            set
            {
                if (this.email.address == value) return;
                this.email.address = value;
                this.OnPropertyChanged("address");
                _api.Orm.Update("update email set address = @address where Id = @Id", new { address = value, Id = this.email.id });
            }
        }
    }
}
