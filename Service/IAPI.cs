using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IAPI
    {
        OrmBms Orm { get; set; }

        bool Initialize();
    }
}
