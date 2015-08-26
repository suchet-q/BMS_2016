﻿using Service;
using Service.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
            _stockList = listStock;
            IEnumerable<Tva> tvaRes = _api.Orm.ObjectQuery<Tva>("select * from tva");
            this.AllTvaRate = new ObservableCollection<Tva>(tvaRes);
            this.tvaRate = this.Model.tva;
            IEnumerable<StockCategorie> categorieRes = _api.Orm.ObjectQuery<StockCategorie>("select * from stock_categorie");
            this._allCategories = new ObservableCollection<StockCategorie>(categorieRes);
            this.categorie = this.Model.categorie;
            this.AddCategorieCommand = new DelegateCommand((o) => this.AddCategorie());
            this.DeleteCategorieCommand = new DelegateCommand((o) => this.DeleteCategorie());
            this.DisplayDeleteCategoryErrMsg = false;
        }

        private void    AddCategorie()
        {
            StockCategorie toInsert = new StockCategorie();

            toInsert.categorie = this.newCategory;
            this.newCategory = "";
            _api.Orm.Insert("insert into stock_categorie(categorie) values (@NewCategorie)", new { NewCategorie = toInsert.categorie });
            var res = _api.Orm.Query("select max(id) as maxId from stock_categorie");
            toInsert.id = res.First().maxId;
            System.Console.Error.WriteLine("nouvel ID de la nouvelle categorie : " + toInsert.id);
            this.AllCategories.Add(toInsert);
            this.categorie = toInsert;
        }

        private void    DeleteCategorie()
        {
            if (this.DisplayDeleteCategoryErrMsg)
                this.DisplayDeleteCategoryErrMsg = !this.DisplayDeleteCategoryErrMsg;

            int count = 0;
            foreach (Stock elem in this._stockList)
            {
                if (elem.categorie.id == this.categorie.id)
                    ++count;
                if (count >= 2)
                {
                    this.DisplayDeleteCategoryErrMsg = true;
                    return;
                }
            }
            _api.Orm.Delete("delete from stock_categorie where id = @Id", new { Id = this.categorie.id });
            var tmp = this.categorie;
            this.AllCategories.Remove(tmp);
            this.categorie = this.AllCategories.Count() > 0 ? this.AllCategories.First() : null;
        }

        ObservableCollection<StockCategorie> _allCategories;
        public ObservableCollection<StockCategorie> AllCategories
        {
            get
            {
                return _allCategories;
            }
            set
            {
                if (_allCategories == value) return;
                _allCategories = value;
                this.OnPropertyChanged("AllCategories");
            }
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
                _api.Orm.UpdateObject<Stock>(@"update stock set nom = @nom where id = @id", Model);
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
                _api.Orm.UpdateObject<Stock>(@"update stock set info = @info where id = @id", Model);
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
                _api.Orm.UpdateObject<Stock>(@"update stock set achat = @achat where id = @id", Model);
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
                _api.Orm.UpdateObject<Stock>(@"update stock set vente_ht = @vente_ht where id = @id", Model);
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
                _api.Orm.UpdateObject<Stock>(@"update stock set vente_ttc = @vente_ttc where id = @id", Model);
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
                _api.Orm.UpdateObject<Stock>(@"update stock set quantite = @quantite where id = @id", Model);
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
                _api.Orm.UpdateObject<Stock>(@"update stock set reference = @reference where id = @id", Model);
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
                _api.Orm.Update(@"update stock set id_tva = @tva where id = @Id", new { tva = this.Model.tva.id, Id = this.Model.id });
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
                if (value != null)
                    _api.Orm.Update(@"update stock set id_categorie = @categorie where id = @Id", new { categorie = this.Model.categorie.id, Id = this.Model.id });
            }
        }
        //public Zone zone
        //{
        //    get
        //    {
        //        return this.Model.zone;
        //    }
        //    set
        //    {
        //        if (this.Model.zone == value) return;
        //        this.Model.zone = value;
        //        this.OnPropertyChanged("zone");
        //    }
        //}


        string _newCategorie;
        public string newCategory
        {
            get
            {
                return _newCategorie;
            }
            set
            {
                if (_newCategorie == value) return;
                _newCategorie = value;
                this.OnPropertyChanged("newCategory");
            }
        }

        bool _displayDeleteCategoryErrMsg;
        public bool DisplayDeleteCategoryErrMsg
        {
            get
            {
                return _displayDeleteCategoryErrMsg;
            }
            set
            {
                if (_displayDeleteCategoryErrMsg == value) return;
                _displayDeleteCategoryErrMsg = value;
                this.OnPropertyChanged("DisplayDeleteCategoryErrMsg");
            }
        }

        public ICommand AddCategorieCommand { get; set; }
        public ICommand DeleteCategorieCommand { get; set; }
    }
}
