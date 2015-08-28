using Service;
using Service.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersManagerModule.ViewModel
{
    public class OrderDetailViewModel : ViewModelBase
    {
        Orders                        _order;
        public Orders                 Model { get; private set; }
        ObservableCollection<Orders>  _listOrder;
        IAPI                        _api;

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
                this.OnPropertyChanged("DateOrdered");
                _api.Orm.UpdateObject<Orders>(@"update orders set dateordered = @dateordered where Id = @Id", Model);
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
                this.OnPropertyChanged("Content");
                _api.Orm.UpdateObject<Orders>(@"update orders set content = @content where Id = @Id", Model);
            }
        }

        public int Status
        {
            get
            {
                return this.Model.status;
            }

            set
            {
                this.Model.status = value;
                this.OnPropertyChanged("Status");
                _api.Orm.UpdateObject<Orders>(@"update orders set status = @status where Id = @Id", Model);
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
                this.OnPropertyChanged("Receiver");
                _api.Orm.UpdateObject<Orders>(@"update orders set receiver = @receiver where Id = @Id", Model);
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
                this.OnPropertyChanged("DateReceived");
                _api.Orm.UpdateObject<Orders>(@"update orders set datereceived = @datereceived where Id = @Id", Model);
            }
        }
    }
}
