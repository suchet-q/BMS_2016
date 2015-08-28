using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Model
{
    public class Orders
    {
        public int id { get; set; }
        public DateTime dateordered { get; set; }
        public string content { get; set; }
        public int status { get; set; }
        public string receiver { get; set; }
        public DateTime datereceived { get; set; }
    }
}
