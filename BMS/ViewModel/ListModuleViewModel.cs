using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.ViewModel
{
    public class ListModuleViewModel : BindableBase, IListModuleViewModel
    {
        private readonly IModuleCatalog _catalog;
        private readonly IRegionManager _manager;

        
        protected List<String> _moduleListString;
        public List<String> ModuleListString
        {
            get
            {
                return this._moduleListString;
            }
            set { this._moduleListString = value; }
        }

        private readonly DelegateCommand<string> _clickCommand;
        public DelegateCommand<string> ButtonClickCommand
        {
            get { return _clickCommand; }
        }

        public ListModuleViewModel(IModuleCatalog moduleCatalog, IRegionManager manager) 
        {
            this._catalog = moduleCatalog;
            this._manager = manager;
            IList<ModuleInfo> ListModule = this._catalog.Modules.ToList<ModuleInfo>();
            this._moduleListString = new List<String>();
            foreach (var elem in ListModule)
            {
                this._moduleListString.Add(elem.ModuleName);
                System.Console.Error.WriteLine("Le module se nomme : " + elem.ModuleName);
            }

            _clickCommand = new DelegateCommand<string>(
            (s) => { this.ClickCommand(); }, //Execute
            (s) => { return true; } //CanExecute
         );
        }
        public void ClickCommand()
        {
            System.Console.Error.WriteLine("Click fait maggle");
        }
    }
}
