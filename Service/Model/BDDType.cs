using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Model
{
    public enum BDDTypeEnum
    {
        MySQL = 1,
        Oracle = 2,
        MicrosoftSQL = 3,
        SQLite = 4,
        OLEDB = 5,
        ODBC = 6
    }

    public class BDDType
    {
        public string Name { get; set; }
        public BDDTypeEnum Type { get; set; }

        public BDDType(string name, BDDTypeEnum type)
        {
            Name = name;
            Type = type;
        }

        public static IEnumerable<BDDType> generateListOfBDD()
        {
            List<BDDType> tmp = new List<BDDType>();
            tmp.Add(new BDDType("MySQL", BDDTypeEnum.MySQL));
            tmp.Add(new BDDType("Microsoft SQL Server", BDDTypeEnum.MicrosoftSQL));
            tmp.Add(new BDDType("Oracle", BDDTypeEnum.Oracle));
            tmp.Add(new BDDType("SQLite", BDDTypeEnum.SQLite));
            tmp.Add(new BDDType("OLEDB", BDDTypeEnum.OLEDB));
            tmp.Add(new BDDType("ODBC", BDDTypeEnum.ODBC));
            return tmp;
        }
    }
}
