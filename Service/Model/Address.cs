using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Model
{
    public class Address : ContactDetail
    {
        public int id { get; set; }
        public String lineone { get; set; }
        public String linetwo { get; set; }
        public String city { get; set; }
        public String state { get; set; }
        public String zipcode { get; set; }
        public String country { get; set; }
        public int contact_detail_id { get; set; }
    }
}
