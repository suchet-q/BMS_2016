using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service;
using Service.Model;
using System.Windows.Input;

namespace StockManagerModule.ViewModel
{
    public class StockManagerModuleViewModel : ViewModelBase
    {

        IAPI                                    _api;

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

        public StockManagerModuleViewModel(IAPI api)
        {
            _api = api;
            _listStock = this.buildEntryList();

            this.AllStocks = new ObservableCollection<StockViewModel>();
            foreach (Stock stock in _listStock)
            {
                this.AllStocks.Add(new StockViewModel(stock, _listStock, _api));
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
                newElem.tva = _api.Orm.ObjectQuery<Tva>("select * from tva where id=@id", new { id = stockBrut.id_tva }).First();
                newElem.zone = _api.Orm.ObjectQuery<Zone>("select * from zone where id=@id", new { id = stockBrut.id_zone }).First();
                newElem.categorie = _api.Orm.ObjectQuery<StockCategorie>("select * from stock_categorie where id=@id", new { id = stockBrut.id_categorie }).First();
                res.Add(newElem);
            }
            return res;
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
