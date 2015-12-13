using Service.Model;
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
        User LoggedUser { get; set; }

        bool Initialize();
        string CalculateMD5Hash(string input);
        void GenerateCsv<T>(IEnumerable<T> data, string fileName = null, bool openInDirectory = false);
        string ComputeSaltHashSHA256(string plainText, byte[] salt = null);
        byte[] GenerateRandomSalt();
    }
}
