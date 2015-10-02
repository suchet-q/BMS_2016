using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Model
{
    public class AgendaName
    {
        public int id { get; set; }
        public int idgroupe { get; set; }
        public string name { get; set; }
        public string participants { get; set; }
        public int userid { get; set; }
    }
}
