﻿using System;
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
        string CalculateMD5Hash(string input);
        void GenerateCsv<T>(IEnumerable<T> data, string fileName = null, bool openInDirectory = false);
    }
}
