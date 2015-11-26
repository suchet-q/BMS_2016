using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Model
{
    public enum OrderStatus { SHIPPING=0, RECEIVED, CANCELED, RETURNED}

    public enum OrderType { DISPATCH=0, RESTOCKING}

    public class Orders
    {
        public int id { get; set; }
        public DateTime dateordered { get; set; }
        public string content { get; set; }
        public int status { get; set; }
        public int type { get; set; }
        public Client client { get; set; }
        public DateTime datereceived { get; set; }
    }
}
