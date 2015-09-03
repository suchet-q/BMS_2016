using Service.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class API : IAPI
    {
        public OrmBms Orm { get; set; }
        public User LoggedUser { get; set; }
   
        public API()
        {
            this.Orm = new OrmBms();
        }

        public bool Initialize()
        {
            this.Orm.Initialize("mysql-bms-market.alwaysdata.net", "bms-market_logiciel", 3306, "110624_bms", "655957ab");
//            this.Orm.Initialize("localhost", "bms", 3306, "root", "");
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

        public void GenerateCsv<T>(IEnumerable<T> data, string fileName = null, bool openInDirectory = false)
        {
            string csv = "";
            PropertyInfo[] propertyInfos;
            propertyInfos = typeof(T).GetProperties();
            var dir = "./generatedDocuments/";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                csv += propertyInfo.Name + "; ";
            }
            csv = csv.Remove(csv.Length - 2);
            csv += Environment.NewLine;

            foreach (T elem in data)
            {
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    var value = propertyInfo.GetValue(elem, null);
                    csv += value + "; ";
                }
                csv = csv.Remove(csv.Length - 2);
                csv += Environment.NewLine;
            }
            if (fileName == null)
            {
                string[] splitedClassName = typeof(T).ToString().Split('.');
                fileName = splitedClassName[splitedClassName.Length - 1];
                fileName += " " + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss");
            }

            File.WriteAllText(Path.Combine(dir,  fileName + ".csv"), csv);

            if (openInDirectory == true)
            {
                Process.Start("explorer.exe", string.Format("/select,\"{0}\"", Path.GetFullPath(Path.Combine(dir, fileName + ".csv"))));
            }
        }
    }
}
