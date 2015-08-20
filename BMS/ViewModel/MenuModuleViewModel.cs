using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service;
using Service.Model;
using System.Collections.ObjectModel;
using Service.DataAccess;
using Microsoft.Practices.Prism.Regions;

namespace BMS.ViewModel
{
    public class MenuModuleViewModel : ViewModelBase
    {
        IRegionManager _manager;

        private Module _selectedModuleInTheList;

        public Module SelectedModuleInTheList
        {
            get { return _selectedModuleInTheList; }
            //setter appelé quand le focus change dans la listBox
            set
            {
                if (_selectedModuleInTheList == value)
                    return;
                _selectedModuleInTheList = value;
                string moduleHomeView = value.name + "View";
                if (value.name == "BMSModule") // Sera retiré par la suite, on met ca en attendant car le nom de sa "home" view n'est pas composé comme ceci : NomDuModuleView
                    moduleHomeView = "MainGModulesView";
                Uri destination = new Uri(moduleHomeView, UriKind.Relative);
                IRegion regionToNavigate = this._manager.Regions["MainModuleRegion"];
                regionToNavigate.RequestNavigate(destination, MenuModuleViewModel.CheckForNavigationError); // TODO pour le futur : Implementer le IConfirmNavigation et ce genre de bordel
            }
        }

        public ObservableCollection<Module> ListModule
        {
            get;
            private set;
        }

        public MenuModuleViewModel(IModuleRepository moduleRepository, IRegionManager manager)
        {
            _manager = manager;
            this.ListModule = new ObservableCollection<Module>(moduleRepository.getListModule());
        }

        protected override void OnDispose()
        {
            this.ListModule.Clear();
        }

        public static void CheckForNavigationError(NavigationResult result)
        {
            if (result.Result == false)
            {
                throw new Exception(result.Error.Message);
            }
        }
    }
}
