﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager
{
    class Order
    {
        public string Date { get; set; }
        public string Client { get; set; }
        public string Ref { get; set; }
        public string Content { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }

        public Order(string p1, string p2, string p3, string p4, string p5, string p6 = "")
        {
            // TODO: Complete member initialization
            this.Date = p1;
            this.Client = p2;
            this.Ref = p3;
            this.Content = p4;
            this.Status = p5;
            this.Comment = p6;
        }
    }
}
