using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Model
{
    public class RightRoleModule
    {
        public int id { get; set; }
        public int role_id { get; set; }
        public string nom_module { get; set; }
        public bool right_read { get; set; }
        public bool right_create { get; set; }
        public bool right_update { get; set; }
        public bool right_delete { get; set; }
    }
}
