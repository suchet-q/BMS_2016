using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Model
{
    public class BasicEmployee
    {
        public int id { get; set; }
        public String title { get; set; }
        public String firstname { get; set; }
        public String lastname { get; set; }
        public String position { get; set; }
        public int departement_id { get; set; }
        public int manager_id { get; set; }
        public DateTime hiredate { get; set; }
        public DateTime terminationdate { get; set; }
        public DateTime birthdate { get; set; }
    }
}
