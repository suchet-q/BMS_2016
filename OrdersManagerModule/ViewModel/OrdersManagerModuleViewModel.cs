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

         public OrdersManagerModuleViewModel(IAPI api)
        {
            _api = api;

            IEnumerable<Order> listOrder = _api.Orm.ObjectQuery<Order>("select * from orders");

        }
    }
}
