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

        private OrderDetailViewModel _currentOrder;
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

        private ObservableCollection<Order> _listAllOrders;

        public ObservableCollection<OrderDetailViewModel> ListAllOrders { get; private set; }

        public ICommand AddOrderCommand { get; private set; }

        private void AddOrder()
        {
            Order order = new Order();
            _api.Orm.InsertObject(order);
            IEnumerable<dynamic> res = _api.Orm.Query("select max(id) as maxId from Order");
            order.id = (int)res.First().maxId;

            OrderDetailViewModel vm = new OrderDetailViewModel(order, _listAllOrders, _api);
            this.ListAllOrders.Add(vm);
            this.CurrentOrder = vm;
        }

         public OrdersManagerModuleViewModel(IAPI api)
        {
            _api = api;

            IEnumerable<Order> listOrder = _api.Orm.ObjectQuery<Order>("select * from orders");

            _listAllOrders = new ObservableCollection<Order>(listOrder);
            this.ListAllOrders = new ObservableCollection<OrderDetailViewModel>();
            foreach (Order order in _listAllOrders)
            {
                this.ListAllOrders.Add(new OrderDetailViewModel(order, _listAllOrders, _api));
            }

            _currentOrder = ListAllOrders.Count > 0 ? ListAllOrders[0] : null;

            this.ListAllOrders.CollectionChanged += (sender, e) =>
            {
                if (e.OldItems != null && e.OldItems.Contains(this.CurrentOrder))
                {
                    this.CurrentOrder = null;
                }
            };

        }
    }
}
