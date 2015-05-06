using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.ViewModel
{
    public class ListModuleViewModel : BindableBase, IListModuleViewModel
    {
        private readonly IModuleCatalog _catalog;
        private readonly IRegionManager _manager;

        private string _selectedModuleInTheList;
        public string SelectedModuleInTheList
        {
            get { return _selectedModuleInTheList; }
            // ce setter est appelé quand le focus change dans la listBox, on peut donc trigger la navigation
            set
            {
                if (_selectedModuleInTheList == value)
                    return;
                _selectedModuleInTheList = value;
                System.Console.Error.WriteLine("Module séléctionné : " + value);
                string moduleHomeView = value + "View";
                if (value == "BMSModule") // Sera retiré par la suite, on met ca en attendant car le nom de sa "home" view n'est pas composé comme ceci : NomDuModuleView
                   moduleHomeView = "BMSView";
                Uri destination = new Uri(moduleHomeView, UriKind.Relative);
                IRegion regionToNavigate = this._manager.Regions["MainContentRegion"];
                regionToNavigate.RequestNavigate(destination, ListModuleViewModel.CheckForNavigationError); // TODO pour le futur : Implementer le IConfirmNavigation et ce genre de bordel
            }
        }

        protected List<string> _moduleListString;
        public List<string> ModuleListString
        {
            get
            {
                return this._moduleListString;
            }
            set
            {
                OnPropertyChanged("ModuleListString");
                this._moduleListString = value;
                System.Console.Error.WriteLine("Je passe dans le Seter maggle");
            }
        }


        public ListModuleViewModel(IModuleCatalog moduleCatalog, IRegionManager manager) 
        {
            this._catalog = moduleCatalog;
            this._manager = manager;

            // On recupere la liste des modules chargé et on les ajoute a la list qui sera display dans le menu de navigations
//            IList<ModuleInfo> ListModule = this._catalog.Modules.ToList<ModuleInfo>();
//            this._moduleListString = new List<String>();
//            foreach (var elem in ListModule)
//            {
//                this._moduleListString.Add(elem.ModuleName);
//                System.Console.Error.WriteLine("Le module se nomme : " + elem.ModuleName);
//            }

            this._moduleListString = this.ListOfModuleBuilder(this._catalog);
            // Permet qu'au lancement du soft, le premier module dans la list soit séléctionné et sa view display dans la MainContentRegion
            this.SelectedModuleInTheList = this._moduleListString.First<string>();

        }

        public static void CheckForNavigationError(NavigationResult result)
        {
            if (result.Result == false)
            {
                throw new Exception(result.Error.Message);
            }
        }

        public void HandleNewModule(object sender, EventArgs e)
        {
            System.Console.Error.WriteLine("Ajout de module detecté, mise ajour du panel de navigation.......");
            this.ModuleListString = this.ListOfModuleBuilder(this._catalog);

        }

        public List<string> ListOfModuleBuilder(IModuleCatalog moduleCatalog)
        {
            IList<ModuleInfo> ListModule = moduleCatalog.Modules.ToList<ModuleInfo>();
            List<string> moduleList = new List<string>();
            foreach (var elem in ListModule)
            {
                moduleList.Add(elem.ModuleName);
                System.Console.Error.WriteLine("Module dans la liste : " + elem.ModuleName);
            }
            return moduleList;
        }
    }
}
