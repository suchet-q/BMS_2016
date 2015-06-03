using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager
{
    class Order
    {
        private string p1;
        private string p2;
        private string p3;
        private string p4;
        private string p5;
        private string p6;

        public Order(string p1, string p2, string p3, string p4, string p5)
        {
            // TODO: Complete member initialization
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
            this.p4 = p4;
            this.p5 = p5;
        }

        public Order(string p1, string p2, string p3, string p4, string p5, string p6)
        {
            // TODO: Complete member initialization
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
            this.p4 = p4;
            this.p5 = p5;
            this.p6 = p6;
        }
    }
}
