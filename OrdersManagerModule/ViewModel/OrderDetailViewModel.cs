using Service;
using Service.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OrdersManagerModule.ViewModel
{
    public class OrderDetailViewModel : ViewModelBase
    {
        Orders                        _order;
        public Orders                 Model { get; private set; }
        ObservableCollection<Orders>  _listOrder;
        IAPI                        _api;
        public ICommand             ValidateOrderCommand { get; private set; }
        public Array EnumCol { get; set; }

        public OrderDetailViewModel(Orders order, ObservableCollection<Orders> listOrder, IAPI api)
        {
            _api = api;
            if (order == null)
            {
                throw new ArgumentNullException("Order");
            }
            System.Console.Error.WriteLine("Order ID ===> " + order.id);
            _order = Model = order;
            _listOrder = listOrder;
            ValidateOrderCommand = new DelegateCommand((o) => this.ValidateOrder());
            var enum_names = Enum.GetValues(typeof (OrderStatus));
            EnumCol = enum_names;
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

        public string Receiver
        {
            get
            {
                return this.Model.receiver;
            }

            set
            {
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
            _api.Orm.UpdateObject<Orders>(@"update orders set receiver = @receiver where Id = @Id", Model);
            this.OnPropertyChanged("DateReceived");
            _api.Orm.UpdateObject<Orders>(@"update orders set datereceived = @datereceived where Id = @Id", Model);
        }

    }
}
