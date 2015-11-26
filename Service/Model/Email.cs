using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Model
{
    public class Email : ContactDetail
    {
        public int id { get; set; }
        public String address { get; set; }
        public int contact_detail_id { get; set; }
    }
}
