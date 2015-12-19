using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Model
{
    public class Issue
    {
        public int id { get; set; }
        public User creator { get; set; }
        public User assignee { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public IssueType type { get; set; }
    }
}
