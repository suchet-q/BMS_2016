using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
            this.Orm.Initialize("mysql-bms-market.alwaysdata.net", "bms-market_logiciel", 3306 , "110624_bms", "655957ab");
            return true;
        }

        public string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            if (input == null)
                return "";
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
