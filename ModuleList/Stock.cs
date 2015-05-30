using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleList
{
    public class Stock
    {
        public string Name{get;set;}
        public int Quantity{get; set;}
        public int Price{get; set;}

        public Stock(string _name, string _quantity, string _price)
        {
            Name = _name;
            Quantity = Convert.ToInt32(_quantity);
            Price = Convert.ToInt32(_price);
            //ToString();
        }
        
        public override string ToString()
        {
            Debug.WriteLine("Name : " + Name);
            Debug.WriteLine("Quantity : " + Quantity);
            Debug.WriteLine("Price : " + Price);
            return (Name + " " + Quantity.ToString() + " " + Price.ToString());
        }
    }
}
