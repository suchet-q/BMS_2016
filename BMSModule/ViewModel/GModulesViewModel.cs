using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service;
using System.Collections.ObjectModel;
using Service.Model;
using Service.DataAccess;
using Microsoft.Practices.Prism.Modularity;

namespace BMSModule.ViewModel
{
    public class GModulesViewModel : ViewModelBase
    {
        public ObservableCollection<Module> GModules_ListModule
        {
            get;
            private set;
        }

        public GModulesViewModel(IModuleCatalog catalog)
        {
            ModuleRepository mR = new ModuleRepository(catalog);
            System.Console.Error.WriteLine(mR.getListModule().Count());
            this.GModules_ListModule = new ObservableCollection<Module>(mR.getListModule());
        }

        protected override void OnDispose()
        {
            this.GModules_ListModule.Clear();
        }
    }
}
