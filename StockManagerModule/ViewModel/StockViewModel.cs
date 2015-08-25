using Service;
using Service.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockManagerModule.ViewModel
{
    public class StockViewModel : ViewModelBase
    {

        public Stock Model { get; private set; }
        IAPI                            _api;
        ObservableCollection<Stock>     _stockList;
 

        public StockViewModel(Stock stock, ObservableCollection<Stock> listStock, IAPI api)
        {
            _api = api;
            if (stock == null)
            {
                throw new ArgumentNullException("stock");
            }
            Model = stock;
            this.tvaRate = this.Model.tva;
            _stockList = listStock;
            IEnumerable<Tva> tvaRes = _api.Orm.ObjectQuery<Tva>("select * from tva");
            this.AllTvaRate = new ObservableCollection<Tva>(tvaRes);
            IEnumerable<StockCategorie> categorieRes = _api.Orm.ObjectQuery<StockCategorie>("select * from stock_categorie");
            this.AllCategories = new ObservableCollection<StockCategorie>(categorieRes);
        }

        public ObservableCollection<StockCategorie> AllCategories
        {
            get;
            set;
        }

        public ObservableCollection<Tva> AllTvaRate
        {
            get;
            set;
        }

        public int Id
        {
            get
            {
                return this.Model.id;
            }

            set
            {
                if (this.Model.id == value) return;
                this.Model.id = value;
                this.OnPropertyChanged("id");
            }
        }

        public string nom
        {
            get
            {
                return this.Model.nom;
            }

            set
            {
                if (this.Model.nom == value) return;
                this.Model.nom = value;
                this.OnPropertyChanged("nom");
            }
        }
        public string info
        {
            get
            {
                return this.Model.info;
            }

            set
            {
                if (this.Model.info == value) return;
                this.Model.info = value;
                this.OnPropertyChanged("info");
            }
        }

        public float achat
        {
            get
            {
                return this.Model.achat;
            }
            set
            {
                if (this.Model.achat == value) return;
                this.Model.achat = value;
                this.OnPropertyChanged("achat");
            }
        }
        public float vente_ht
        {
            get
            {
                return this.Model.vente_ht;
            }
            set
            {
                if (this.Model.vente_ht == value) return;
                this.Model.vente_ht = value;
                this.Model.vente_ttc *= (1 + this.Model.tva.rate / 100);
                this.OnPropertyChanged("vente_ht");
                this.OnPropertyChanged("vente_ttc");
            }
        }
        public float vente_ttc
        {
            get
            {
                return this.Model.vente_ttc;
            }
            set
            {
                if (this.Model.vente_ttc == value) return;
                this.Model.vente_ttc = value;
                this.Model.vente_ht *= (1 - this.Model.tva.rate / 100);
                this.OnPropertyChanged("vente_ttc");
                this.OnPropertyChanged("vente_ht");
            }
        }
        public int quantite
        {
            get
            {
                return this.Model.quantite;
            }
            set
            {
                if (this.Model.quantite == value) return;
                this.Model.quantite = value;
                this.OnPropertyChanged("quantite");
            }
        }
        public string reference
        {
            get
            {
                return this.Model.reference;
            }
            set
            {
                if (this.Model.reference == value) return;
                this.Model.reference = value;
                this.OnPropertyChanged("reference");
            }
        }
        public Tva tvaRate
        {
            get
            {
                return this.Model.tva;
            }
            set
            {
                if (this.Model.tva == value) return;
                this.Model.tva = value;
                this.OnPropertyChanged("tvaRate");
            }
        }
        public StockCategorie categorie
        {
            get
            {
                return this.Model.categorie;
            }
            set
            {
                if (this.Model.categorie == value) return;
                this.Model.categorie = value;
                this.OnPropertyChanged("categorie");
            }
        }

    }
}
