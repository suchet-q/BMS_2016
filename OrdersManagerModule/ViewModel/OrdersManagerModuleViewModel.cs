using Microsoft.Practices.Unity;
using Service;
using Service.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public ICommand AddOrderCommand { get; private set; }
        
        public ICommand DeleteOrderCommand { get; private set; }

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
                foreach (Client client in _listAllClients)
                {
                    if (client.id == stockBrut.id_client)
                    {
                        newElem.receiver = client;
                        break;
                    }
                }
                res.Add(newElem);
            }
            return res;
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
            _api.Orm.InsertObject(order);
            IEnumerable<dynamic> res = _api.Orm.Query("select max(id) as maxId from orders");
            order.id = (int)res.First().maxId;
            order.dateordered = DateTime.Now;

            _listAllOrders.Add(order);
            OrderDetailViewModel vm = new OrderDetailViewModel(order, _listAllOrders, _api, _container);
            this.ListAllOrders.Add(vm);
            this.CurrentOrder = vm;
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
