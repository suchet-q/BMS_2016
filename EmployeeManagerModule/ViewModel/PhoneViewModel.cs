using System;
using EmployeeManagerModule.Model;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Service;

namespace EmployeeManagerModule.ViewModel
{
    /// <summary>
    /// ViewModel of an individual Phone
    /// </summary>
    public class PhoneViewModel : ContactDetailViewModel
    {
        /// <summary>
        /// The Phone object backing this ViewModel
        /// </summary>
        private Phone phone;
        private IAPI _api;

        /// <summary>
        /// Initializes a new instance of the PhoneViewModel class.
        /// </summary>
        /// <param name="detail">The underlying Phone this ViewModel is to be based on</param>
        public PhoneViewModel(Phone detail)
        {
            if (detail == null)
            {
                throw new ArgumentNullException("detail");
            }

            IUnityContainer container = ServiceLocator.Current.GetInstance<IUnityContainer>();
            _api = container.Resolve<IAPI>();

            this.phone = detail;
        }

        /// <summary>
        /// The underlying Phone this ViewModel is based on
        /// </summary>
        public override ContactDetail Model
        {
            get { return this.phone; }
        }

        /// <summary>
        /// Gets or sets the actual number
        /// </summary>
        public string Number
        {
            get
            {
                return this.phone.number;
            }

            set
            {
                if (this.phone.number == value) return;
                this.phone.number = value;
                this.OnPropertyChanged("number");
                _api.Orm.Update("update phone set number = @number where Id = @Id", new { number = value, Id = this.phone.id });
            }
        }

        /// <summary>
        /// Gets or sets the extension to be used with this phone number
        /// </summary>
        public string Extension
        {
            get
            {
                return this.phone.extension;
            }

            set
            {
                if (this.phone.extension == value) return;
                this.phone.extension = value;
                this.OnPropertyChanged("extension");
                _api.Orm.Update("update phone set extension = @extension where Id = @Id", new { extension = value, Id = this.phone.id });
            }
        }
    }
}
