using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Model
{
    public class Stock
    {
        public int id { get; set; }
        public string nom { get; set; }
        public string info { get; set; }
        public float achat { get; set; }
        public float vente_ht { get; set; }
        public float vente_ttc { get; set; }
        public int quantite { get; set; }
        public string reference { get; set; }
        public string zone { get; set; }
        public string sous_zone { get; set; }
        public string emplacement { get; set; }
        public Tva tva { get; set; }
        public StockCategorie categorie { get; set; }
    }
}
