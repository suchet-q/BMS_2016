using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Model
{
    public enum AgendaStatus { IMPORTANT, MANDATORY, OPTIONAL }
    public class AgendaEvent
    {
        public int id { get; set; }
        public int idgroupe { get; set; }
        public DateTime date { get; set; }
        public string startevent { get; set; }
        public string endevent { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string location { get; set; }
        public int userid { get; set; }
        public string color { get; set; }
        public string status { get; set; }
        public string name { get; set; }
    }
}
