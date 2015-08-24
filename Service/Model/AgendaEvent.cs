﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Model
{
    public class AgendaEvent
    {
        public int id { get; set; }
        public DateTime date { get; set; }
        public string startevent { get; set; }
        public string endevent { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string location { get; set; }
        public int userid { get; set; }
        public string color { get; set; }
        public int status { get; set; }
        //public List<int> participants { get; set; }
    }
}