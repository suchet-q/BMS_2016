using EmployeeManagerModule.Model;
using System;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Service;
using Service.Model;

namespace EmployeeManagerModule.ViewModel
{
    /// <summary>
    /// ViewModel of an individual Address
    /// </summary>
    public class AddressViewModel : ContactDetailViewModel
    {
        /// <summary>
        /// The Address object backing this ViewModel
        /// </summary>
        private Address address;
        private IAPI _api;
        
        /// <summary>
        /// Initializes a new instance of the AddressViewModel class.
        /// </summary>
        /// <param name="detail">The underlying Address this ViewModel is to be based on</param>
        public AddressViewModel(Address detail)
        {
            if (detail == null)
            {
                throw new ArgumentNullException("detail");
            }

            IUnityContainer container = ServiceLocator.Current.GetInstance<IUnityContainer>();
            _api = container.Resolve<IAPI>();

            this.address = detail;
        }

        /// <summary>
        /// The underlying Address this ViewModel is based on
        /// </summary>
        public override ContactDetail Model
        {
            get { return this.address; }
        }

        /// <summary>
        /// Gets or sets the first address line
        /// </summary>
        public string LineOne
        {
            get
            {
                return this.address.lineone;
            }

            set
            {
                if (this.address.lineone == value) return;
                this.address.lineone = value;
                this.OnPropertyChanged("lineone");
                _api.Orm.Update("update address set lineone = @lineone where Id = @Id", new { lineone = value, Id = this.address.id });
            }
        }

        /// <summary>
        /// Gets or sets the second address line
        /// </summary>
        public string LineTwo
        {
            get
            {
                return this.address.linetwo;
            }

            set
            {
                if (this.address.linetwo == value) return;
                this.address.linetwo = value;
                this.OnPropertyChanged("linetwo");
                _api.Orm.Update("update address set linetwo = @linetwo where Id = @Id", new { linetwo = value, Id = this.address.id });
            }
        }

        /// <summary>
        /// Gets or sets the city of this address
        /// </summary>
        public string City
        {
            get
            {
                return this.address.city;
            }

            set
            {
                if (this.address.city == value) return;
                this.address.city = value;
                this.OnPropertyChanged("city");
                _api.Orm.Update("update address set city = @city where Id = @Id", new { city = value, Id = this.address.id });
            }
        }

        /// <summary>
        /// Gets or sets the state of this address
        /// </summary>
        public string State
        {
            get
            {
                return this.address.state;
            }

            set
            {
                if (this.address.state == value) return;
                this.address.state = value;
                this.OnPropertyChanged("state");
                _api.Orm.Update("update address set state = @state where Id = @Id", new { state = value, Id = this.address.id });
            }
        }

        /// <summary>
        /// Gets or sets the zip code of this address
        /// </summary>
        public string ZipCode
        {
            get
            {
                return this.address.zipcode;
            }

            set
            {
                if (this.address.zipcode == value) return;
                this.address.zipcode = value;
                this.OnPropertyChanged("zipcode");
                _api.Orm.Update("update address set zipcode = @zipcode where Id = @Id", new { zipcode = value, Id = this.address.id });
            }
        }

        /// <summary>
        /// Gets or sets the country of this address
        /// </summary>
        public string Country
        {
            get
            {
                return this.address.country;
            }

            set
            {
                if (this.address.country == value) return;
                this.address.country = value;
                this.OnPropertyChanged("country");
                _api.Orm.Update("update address set country = @country where Id = @Id", new { country = value, Id = this.address.id });
            }
        }
    }
}
