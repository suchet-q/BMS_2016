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

namespace StockManagerModule.ViewModel
{
    public class StockManagerModuleViewModel : ViewModelBase
    {

        IAPI                                    _api;
        IUnityContainer                         _container;

        private ObservableCollection<StockCategorie> _allCategories;

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

        private void AddStock()
        {

        }

        private void DeleteCurrentStock()
        {

        }

        public ICommand AddStockCommand { get; private set; }
        public ICommand DeleteStockCommand { get; private set; }

    }
}
