using Microsoft.Practices.Unity;
using Service;
using Service.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace OrdersManagerModule.ViewModel
{
    public class OrdersManagerModuleViewModel : ViewModelBase
    {
        IAPI _api;
        IUnityContainer _container;

        private OrderDetailViewModel _currentOrder;

        private ObservableCollection<Orders> _listAllOrders;
        
        private ObservableCollection<Client> _listAllClients;

        public ObservableCollection<OrderDetailViewModel> ListAllOrders { get; private set; }

        internal CollectionViewSource RestockOrders { get; set; }

        internal CollectionViewSource DispatchOrders { get; set; }

        public ICommand AddOrderCommand { get; private set; }
        
        public ICommand DeleteOrderCommand { get; private set; }

        public ICommand RefreshOrderCommand { get; private set; }


        public OrderDetailViewModel CurrentOrder
        {
            get
            {
                return _currentOrder;
            }

            set
            {
                _currentOrder = value;
                this.OnPropertyChanged("CurrentOrder");
            }
        }

        private void RestockFilter(object sender, FilterEventArgs e)
        {
            OrderDetailViewModel vm = (OrderDetailViewModel)e.Item;
            if (vm.Type == OrderType.RESTOCKING)
                e.Accepted = true;
            else
                e.Accepted = false;
        }

        private void DispatchFilter(object sender, FilterEventArgs e)
        {
            OrderDetailViewModel vm = (OrderDetailViewModel)e.Item;
            if (vm.Type == OrderType.DISPATCH)
                e.Accepted = true;
            else
                e.Accepted = false;
        }

        public OrdersManagerModuleViewModel(IAPI api, IUnityContainer container)
        {
            _api = api;
            _container = container;

            _listAllClients = this.buildClientList();
            _container.RegisterInstance(typeof(object), "ClientList", _listAllClients);
            _listAllOrders = this.buildOrderList();

            this.ListAllOrders = new ObservableCollection<OrderDetailViewModel>();
            
            foreach (Orders order in _listAllOrders)
            {
                this.ListAllOrders.Add(new OrderDetailViewModel(order, _listAllOrders, _api, _container));
            }

            _currentOrder = ListAllOrders.Count > 0 ? ListAllOrders[0] : null;

            this.ListAllOrders.CollectionChanged += (sender, e) =>
            {
                if (e.OldItems != null && e.OldItems.Contains(this.CurrentOrder))
                {
                    this.CurrentOrder = null;
                }
            };
            AddOrderCommand = new DelegateCommand((o) => this.AddOrder());
            DeleteOrderCommand = new DelegateCommand((o) => this.DeleteOrder());
            RefreshOrderCommand = new DelegateCommand((o) => this.RefreshLists());
            RestockOrders = new CollectionViewSource();
            RestockOrders.Source = this.ListAllOrders;
            RestockOrders.Filter += RestockFilter;
            DispatchOrders = new CollectionViewSource();
            DispatchOrders.Source = this.ListAllOrders;
            DispatchOrders.Filter += DispatchFilter;
        }

        public ICollectionView ListRestockOrders
        {
            get { return RestockOrders.View; }
        }

        public ICollectionView ListDispatchOrders
        {
            get { return DispatchOrders.View; }
        }

        private ObservableCollection<Orders> buildOrderList()
        {
            IEnumerable<dynamic> orderBrutList = _api.Orm.Query("select * from orders");
            if (orderBrutList == null)
            {
                orderBrutList = new Collection<dynamic>();
            }
            ObservableCollection<Orders> res = new ObservableCollection<Orders>();

            foreach (dynamic stockBrut in orderBrutList)
            {
                Orders newElem = new Orders();

                newElem.id = (int)stockBrut.id;
                newElem.content = stockBrut.content;
                newElem.dateordered = stockBrut.dateordered;
                newElem.datereceived = stockBrut.datereceived;
                newElem.status = stockBrut.status;
                newElem.type = stockBrut.type;
                foreach (Client client in _listAllClients)
                {
                    if (client.id == stockBrut.id_client)
                    {
                        newElem.client = client;
                        break;
                    }
                }
                res.Add(newElem);
            }
            return res;
        }

        private void RefreshLists()
        {
            ListDispatchOrders.Refresh();
            ListRestockOrders.Refresh();
        }

        private ObservableCollection<Client> buildClientList()
        {
            var res = _api.Orm.ObjectQuery<Client>("select * from client");
            if (res == null)
                return new ObservableCollection<Client>();
            return new ObservableCollection<Client>(res);
        }

        private void AddOrder()
        {
            Orders order = new Orders();
            _api.Orm.Insert("insert into orders(id_client) values (@id_client)", new { id_client = this._listAllClients.First().id});
            IEnumerable<dynamic> res = _api.Orm.Query("select max(id) as maxId from orders");
            if (res != null)
            {
                order.id = (int)res.First().maxId;
                order.dateordered = DateTime.Now;
                order.client = this._listAllClients.First();
                _listAllOrders.Add(order);
                OrderDetailViewModel vm = new OrderDetailViewModel(order, _listAllOrders, _api, _container);
                this.ListAllOrders.Add(vm);
                this.CurrentOrder = vm;
            }
        }

        private void DeleteOrder()
        {
            _api.Orm.Delete("delete from orders where id=@idorder", new { idorder = this._currentOrder.Model.id });
            this._listAllOrders.Remove(this.CurrentOrder.Model);
            this.ListAllOrders.Remove(this.CurrentOrder);
            _currentOrder = ListAllOrders.Count > 0 ? ListAllOrders[0] : null;
            this.OnPropertyChanged("CurrentOrder");
        }
    }
}
