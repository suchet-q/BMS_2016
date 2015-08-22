using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Model
{
    public class AgendaEvent
    {
        public DateTime date { get; set; }
        public DateTime startevent { get; set; }
        public DateTime endevent { get; set; }
        public string title { get; set; }
        public string descrpition { get; set; }
        public string location { get; set;  }
        public int userid { get; set; }
        public string color { get; set; }
        public int status { get; set; }
        public List<int> participants { get; set; }
    }
}
