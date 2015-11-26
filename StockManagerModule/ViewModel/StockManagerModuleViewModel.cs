using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service;
using Service.Model;
using System.Windows.Input;
using Microsoft.Practices.Unity;
using System.Threading;

namespace StockManagerModule.ViewModel
{
    public class StockManagerModuleViewModel : ViewModelBase
    {

        IAPI                                    _api;
        IUnityContainer                         _container;

        private ObservableCollection<StockCategorie> _allCategories;
        private ObservableCollection<Tva> _allTva;

        public ObservableCollection<StockViewModel> AllStocks { get; private set; }
       
        private StockViewModel                  _currentStock;
        public StockViewModel                   CurrentStock
        {
            get
            {
                return _currentStock;
            }
            set
            {
                _currentStock = value;
                OnPropertyChanged("CurrentStock");
            }
        }

        ObservableCollection<Stock> _listStock;

        public StockManagerModuleViewModel(IAPI api, IUnityContainer container)
        {
            _api = api;
            _container = container;
            _allCategories = this.buildCategoriesList();
            _allTva = this.buildTvaList();
            _container.RegisterInstance(typeof(object), "CategoriesList", _allCategories);
            _listStock = this.buildEntryList();

            this.AllStocks = new ObservableCollection<StockViewModel>();
            foreach (Stock stock in _listStock)
            {
                this.AllStocks.Add(new StockViewModel(stock, _listStock, _api, _container));
            }

            CurrentStock = AllStocks.Count > 0 ? AllStocks[0] : null;

            this.AllStocks.CollectionChanged += (sender, e) =>
            {
                if (e.OldItems != null && e.OldItems.Contains(this.CurrentStock))
                {
                    this.CurrentStock = null;
                }
            };

            this.GenerateCsvCommand = new DelegateCommand((o) => this.GenerateCsv());
            this.AddStockCommand = new DelegateCommand((o) => this.AddStock());
            this.DeleteStockCommand = new DelegateCommand((o) => this.DeleteCurrentStock());

        }

        private ObservableCollection<Stock> buildEntryList()
        {

            IEnumerable<dynamic> stockBrutList = _api.Orm.Query("select * from stock");
            if (stockBrutList == null)
            {
                stockBrutList = new Collection<dynamic>();
            }
            ObservableCollection<Stock> res = new ObservableCollection<Stock>();
            
            foreach (dynamic stockBrut in stockBrutList)
            {
                Stock newElem = new Stock();

                newElem.id = (int)stockBrut.id;
                newElem.nom = stockBrut.nom;
                newElem.info = stockBrut.info;
                newElem.achat = stockBrut.achat;
                newElem.vente_ht = stockBrut.vente_ht;
                newElem.vente_ttc = stockBrut.vente_ttc;
                newElem.quantite = stockBrut.quantite;
                newElem.reference = stockBrut.reference;
                newElem.zone = stockBrut.zone;
                newElem.sous_zone = stockBrut.sous_zone;
                newElem.emplacement = stockBrut.emplacement;
                newElem.tva = _api.Orm.ObjectQuery<Tva>("select * from tva where id=@id", new { id = stockBrut.id_tva }).First();
                foreach (StockCategorie category in _allCategories)
                {
                    if (category.id == stockBrut.id_categorie)
                    {
                        newElem.categorie = category;
                        break;
                    }
                }
                res.Add(newElem);
            }
            return res;
        }

        private ObservableCollection<StockCategorie> buildCategoriesList()
        {
            var res = _api.Orm.ObjectQuery<StockCategorie>("select * from stock_categorie");
            if (res == null)
                return new ObservableCollection<StockCategorie>();
            return new ObservableCollection<StockCategorie>(res);
        }

        private ObservableCollection<Tva> buildTvaList()
        {
            var res = _api.Orm.ObjectQuery<Tva>("select * from tva");
            if (res == null)
                return new ObservableCollection<Tva>();
            return new ObservableCollection<Tva>(res);
        }

        async private void showAndHideGeneratedMsg()
        {
            this.DisplayGeneratedMsg = true;
            await Task.Run(() => { Thread.Sleep(5000); });
            this.DisplayGeneratedMsg = false;
        }

        private void GenerateCsv()
        {
            List<CsvStockData> list = new List<CsvStockData>();

            foreach (Stock stock in this._listStock)
            {
                list.Add(new CsvStockData(stock.id, stock.nom, stock.info, stock.categorie.categorie, stock.achat.ToString(), stock.vente_ht.ToString(), stock.vente_ttc.ToString(), stock.tva.rate.ToString(), stock.quantite, stock.reference, stock.zone, stock.sous_zone, stock.emplacement));
            }

            _api.GenerateCsv<CsvStockData>(list, "Stock " + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss"), true);
            this.showAndHideGeneratedMsg();
        }

        private void AddStock()
        {
            Stock stock = new Stock();
            _api.Orm.Insert("insert into stock(id_categorie, id_tva) values (@id_categorie, @id_tva)", new { id_categorie = this._allCategories.First().id, id_tva = this._allTva.First().id});
            IEnumerable<dynamic> res = _api.Orm.Query("select max(id) as maxId from stock");
            if (res != null)
            {
                stock.id = (int)res.First().maxId;
                stock.tva = new Tva();
                stock.tva.id = this._allTva.First().id;
                stock.categorie = new StockCategorie();
                stock.categorie.id = this._allCategories.First().id;
                StockViewModel vm = new StockViewModel(stock, this._listStock, _api, _container);
                this.AllStocks.Add(vm);
                this.CurrentStock = vm;
            }
            else
            {
                //Message d erreur
            }

        }

        private void DeleteCurrentStock()
        {
            _api.Orm.Delete("delete from stock where stock.id=@idStock", new { idStock = this.CurrentStock.Model.id });
            this.AllStocks.Remove(this.CurrentStock);
            this.CurrentStock = this.AllStocks.Count() > 0 ? this.AllStocks.First() : null;
        }

        public ICommand GenerateCsvCommand { get; private set; }
        public ICommand AddStockCommand { get; private set; }
        public ICommand DeleteStockCommand { get; private set; }

        bool _displayGeneratedMsg;
        public bool DisplayGeneratedMsg
        {
            get
            {
                return _displayGeneratedMsg;
            }
            set
            {
                if (_displayGeneratedMsg == value) return;
                _displayGeneratedMsg = value;
                this.OnPropertyChanged("DisplayGeneratedMsg");
            }
        }


        private class CsvStockData
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Info { get; set; }
            public string Category { get; set; }
            public string Purchase { get; set; }
            public string Price { get; set; }
            public string Price_inc_VAT { get; set; }
            public string VAT { get; set; }
            public int Quantity { get; set; }
            public string Reference { get; set; }
            public string Zone { get; set; }
            public string Subzone { get; set; }
            public string Location { get; set; }

            public CsvStockData(int id, string name, string info, string category, string purchase,
                                string price, string price_inc_vat, string vat, int quantity, string reference,
                                string zone, string subzone, string location)
            {
                Id = id;
                Name = name;
                Info = info;
                Category = category;
                Purchase = purchase;
                Price = price;
                Price_inc_VAT = price_inc_vat;
                VAT = vat;
                Quantity = quantity;
                Reference = reference;
                Zone = zone;
                Subzone = subzone;
                Location = location;
            }
        }
    }
}
