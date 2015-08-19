using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class API : IAPI
    {
        public OrmBms Orm { get; set; }
   
        public API()
        {
            this.Orm = new OrmBms();
        }

        public bool Initialize()
        {
            this.Orm.Initialize("localhost", "bms", "root", "");
            return true;
        }
    }
}
