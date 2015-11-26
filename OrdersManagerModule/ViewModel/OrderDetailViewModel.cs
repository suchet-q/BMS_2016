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

        System.Windows.Visibility _displayDeleteClientErrMsg;

        public Orders Model { get; private set; }
        
        public ICommand ValidateOrderCommand { get; private set; }

        public ICommand AddClientCommand { get; private set; }

        public ICommand ConfirmClientCommand { get; private set; }
        
        public ICommand DeleteClientCommand { get; private set; }
        
        public Array EnumStatusCol { get; set; }

        public Array EnumTypeCol { get; set; }

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
            EnumStatusCol = enum_names;
            enum_names = Enum.GetValues(typeof(OrderType));
            EnumTypeCol = enum_names;
            this.DisplayDeleteClientErrMsg = Visibility.Collapsed;
        }

        public System.Windows.Visibility DisplayDeleteClientErrMsg
        {
            get
            {
                return _displayDeleteClientErrMsg;
            }
            set
            {
                _displayDeleteClientErrMsg = value;
                this.OnPropertyChanged("DisplayDeleteClientErrMsg");
            }
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
                this.OnPropertyChanged("Id");
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

        public OrderType Type
        {
            get
            {
                return (OrderType)this.Model.type;
            }

            set
            {
                this.Model.type = (int)value;
            }
        }

        public Client Client
        {
            get
            {
                return this.Model.client;
            }

            set
            {
                if (this.Model.client == value) return;
                this.Model.client = value;
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
            this.OnPropertyChanged("Type");
            _api.Orm.UpdateObject<Orders>(@"update orders set type = @type where Id = @Id", Model);
            this.OnPropertyChanged("Receiver");
            _api.Orm.Update(@"update orders set id_client = @client where id = @Id", new { client = this.Model.client.id, Id = this.Model.id });
            this.OnPropertyChanged("DateReceived");
            _api.Orm.UpdateObject<Orders>(@"update orders set datereceived = @datereceived where Id = @Id", Model);
            if (Model.type == (int)OrderType.RESTOCKING)
            {
                Stock stock = new Stock();
                stock.achat = 0;
                stock.info = Model.content;
                stock.nom = Model.content;
                stock.quantite = Int32.Parse(Model.content.Split(" ".ToCharArray()).First());
                stock.reference = "ORDER15";
                stock.emplacement = "Stockage C";
                _api.Orm.Insert("insert into stock(id_categorie, id_tva) values (@id_categorie, @id_tva)", new { id_categorie = 1, id_tva = 1 });
                IEnumerable<dynamic> res = _api.Orm.Query("select max(id) as maxId from stock");
                if (res != null)
                {
                    stock.id = (int)res.First().maxId;
                    _api.Orm.Update(@"update stock set info = @info where id = @Id", stock);
                    _api.Orm.Update(@"update stock set nom = @nom where id = @Id", stock);
                    _api.Orm.Update(@"update stock set reference = @reference where id = @Id", stock);
                    _api.Orm.Update(@"update stock set emplacement = @emplacement where id = @Id", stock);
                    _api.Orm.Update(@"update stock set quantite = @quantite where id = @Id", stock);
                }
            }
        }

        private void AddClient()
        {
            NewReceiverVisibility = Visibility.Visible;        
        }

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
                this.Client = toInsert;
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
            if (this.DisplayDeleteClientErrMsg == Visibility.Visible)
                this.DisplayDeleteClientErrMsg = Visibility.Collapsed;

            int count = 0;
            foreach (Orders elem in this._listOrder)
            {
                if (elem.client.id == this.Client.id)
                    ++count;
                if (count >= 2)
                {
                    this.DisplayDeleteClientErrMsg = Visibility.Visible;
                    return;
                }
            }
            _api.Orm.Delete("delete from client where id = @Id", new { Id = this.Client.id });
            var tmp = this.Client;
            this.AllClient.Remove(tmp);
            this._allClient.Remove(tmp);
            this.Client = this.AllClient.Count() > 0 ? this.AllClient.First() : null;
        }

    }
}
