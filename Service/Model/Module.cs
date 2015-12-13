using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Model
{
    public class Module
    {
        public static Module CreateModule(string Name)
        {
            return new Module { name = Name };
        }
        public string name { get; set; }
    }
}
