using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Model
{
    public class ContactDetail
    {
        public int id_contact_detail { get; set; }
        public int employee_id { get; set; }
        public String usage { get; set; }
    }
}
