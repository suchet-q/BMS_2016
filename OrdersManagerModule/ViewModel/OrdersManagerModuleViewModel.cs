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
        private ObservableCollection<Orders> _listAllOrders;
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

        public OrdersManagerModuleViewModel(IAPI api)
        {
            _api = api;

            IEnumerable<Orders> listOrder = _api.Orm.ObjectQuery<Orders>("select * from orders");

            _listAllOrders = new ObservableCollection<Orders>(listOrder);
            this.ListAllOrders = new ObservableCollection<OrderDetailViewModel>();
            foreach (Orders order in _listAllOrders)
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
            AddOrderCommand = new DelegateCommand((o) => this.AddOrder());
            DeleteOrderCommand = new DelegateCommand((o) => this.DeleteOrder());
        }

        private void AddOrder()
        {
            Orders order = new Orders();
            _api.Orm.InsertObject(order);
            IEnumerable<dynamic> res = _api.Orm.Query("select max(id) as maxId from orders");
            order.id = (int)res.First().maxId;
            order.dateordered = DateTime.Now;

            _listAllOrders.Add(order);
            OrderDetailViewModel vm = new OrderDetailViewModel(order, _listAllOrders, _api);
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
