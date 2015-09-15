using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Model
{
    public class User
    {
        public int id { get; set; }
        public string name { get; set; }
        public string last_name { get; set; }
        public string login { get; set; }
        public string pwd { get; set; }
        public string salt { get; set; }
    }
}
