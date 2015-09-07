using Microsoft.Practices.Unity;
using Service;
using Service.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace OrdersManagerModule.ViewModel
{
    public class OrderDetailViewModel : ViewModelBase
    {
        Orders _order;

        ObservableCollection<Orders> _listOrder;

        ObservableCollection<Client> _allClient;
        
        IAPI _api;

        IUnityContainer _container;

        System.Windows.Visibility _newReceiverVisibility = Visibility.Collapsed;

        public Orders Model { get; private set; }
        
        public ICommand ValidateOrderCommand { get; private set; }

        public ICommand AddClientCommand { get; private set; }

        public ICommand ConfirmClientCommand { get; private set; }
        
        public ICommand DeleteClientCommand { get; private set; }
        
        public Array EnumCol { get; set; }

        public string newClientName { get; set; }

        public string newClientAddress { get; set; }

        public OrderDetailViewModel(Orders order, ObservableCollection<Orders> listOrder, IAPI api, IUnityContainer container)
        {
            _api = api;
            _container = container;

            if (order == null)
            {
                throw new ArgumentNullException("Order");
            }
            _order = Model = order;
            _listOrder = listOrder;
            _allClient = _container.Resolve(typeof(object), "ClientList") as ObservableCollection<Client>;
            ValidateOrderCommand = new DelegateCommand((o) => this.ValidateOrder());
            AddClientCommand = new DelegateCommand((o) => this.AddClient());
            ConfirmClientCommand = new DelegateCommand((o) => this.ConfirmClient());
            DeleteClientCommand = new DelegateCommand((o) => this.DeleteClient());
            this.OnPropertyChanged("newReceiverVisibility");
            var enum_names = Enum.GetValues(typeof(OrderStatus));
            EnumCol = enum_names;
        }

        public System.Windows.Visibility NewReceiverVisibility
        {
            get
            {
                return this._newReceiverVisibility;
            }

            set
            {
                this._newReceiverVisibility = value;
                this.OnPropertyChanged("NewReceiverVisibility");
            }
        }

        public ObservableCollection<Client> AllClient
        {
            get
            {
                return this._allClient;
            }
            set
            {
                if (this._allClient == value) return;
                this._allClient = value;
                this.OnPropertyChanged("AllClient");
            }
        }

        public int Id
        {
            get
            {
                return this.Model.id;
            }

            set
            {
                this.Model.id = value;
                this.OnPropertyChanged("id");
            }
        }

        public DateTime DateOrdered
        {
            get
            {
                return this.Model.dateordered;
            }

            set
            {
                this.Model.dateordered = value;             
            }
        }

        public string Content
        {
            get
            {
                return this.Model.content;
            }

            set
            {
                this.Model.content = value;
            }
        }

        public OrderStatus Status
        {
            get
            {
                return (OrderStatus)this.Model.status;
            }

            set
            {
                this.Model.status = (int)value;
            }
        }

        public Client Receiver
        {
            get
            {
                return this.Model.receiver;
            }

            set
            {
                if (this.Model.receiver == value) return;
                this.Model.receiver = value;
            }
        }

        public DateTime DateReceived
        {
            get
            {
                return this.Model.datereceived;
            }

            set
            {
                this.Model.datereceived = value;
            }
        }

        private void ValidateOrder()
        {
            this.OnPropertyChanged("DateOrdered");
            _api.Orm.UpdateObject<Orders>(@"update orders set dateordered = @dateordered where Id = @Id", Model);
            this.OnPropertyChanged("Content");
            _api.Orm.UpdateObject<Orders>(@"update orders set content = @content where Id = @Id", Model);
            this.OnPropertyChanged("Status");
            _api.Orm.UpdateObject<Orders>(@"update orders set status = @status where Id = @Id", Model);
            this.OnPropertyChanged("Receiver");
            _api.Orm.Update(@"update orders set id_client = @client where id = @Id", new { client = this.Model.receiver.id, Id = this.Model.id });
            this.OnPropertyChanged("DateReceived");
            _api.Orm.UpdateObject<Orders>(@"update orders set datereceived = @datereceived where Id = @Id", Model);
        }

        private void AddClient()
        {
            NewReceiverVisibility = Visibility.Visible;        }

        private void ConfirmClient()
        {
            Client toInsert = new Client();

            toInsert.name = this.newClientName;
            toInsert.address = this.newClientAddress;
            this.newClientAddress = "";
            this.newClientName = "";
            _api.Orm.Insert("insert into client(name, address) values (@newname, @newaddress)", new { newname = toInsert.name, newaddress = toInsert.address });
            var res = _api.Orm.Query("select max(id) as maxId from client");
            if (res != null)
            {
                toInsert.id = res.First().maxId;
                this.AllClient.Add(toInsert);
                this.Receiver = toInsert;
                this.OnPropertyChanged("Receiver");
            }
            else
            {
                System.Console.Error.WriteLine("Cannot create new client");
            }
            NewReceiverVisibility = Visibility.Collapsed;
        }

        private void DeleteClient()
        {

        }

    }
}
