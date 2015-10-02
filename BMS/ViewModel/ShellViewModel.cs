using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service;
using Service.DataAccess;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Regions;
using BMS.ViewModel;
using BMS.View;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Prism.UnityExtensions;
using System.IO;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using mscoree;
using Service;



namespace BMS.ViewModel
{
  
    class ShellViewModel : ViewModelBase
    {
        ObservableCollection<ViewModelBase> _viewModels;

        readonly IModuleRepository          _moduleRepository;

        readonly IModuleCatalog             _catalog;
        IUnityContainer                     _container;
        IRegionManager                      _manager;
        IMetadataModuleCatalog              _metadataCatalog;

        List<ModuleMetadata>                _toBeDeleted;

        IAPI _api;

        public ShellViewModel(IModuleCatalog catalog, IUnityContainer container, IRegionManager manager, IAPI api, IMetadataModuleCatalog metadataCatalog)
        {
            _catalog = catalog;
            _container = container;
            _manager = manager;
            _metadataCatalog = metadataCatalog;
            _api = api;
            _toBeDeleted = new List<ModuleMetadata>();
            _container.RegisterInstance(typeof(object), "toBeDeleted", _toBeDeleted);
            //On le met en comentaire pour les tests pour eviter de ce log a chaque test
            this.WindowHeight = 600;
            this.WindowWidth = 800;
            //var viewModel = new LoginViewModel(api, _container);
            //viewModel.EventLogin += this.NavigateToModuleWorkBench;

            // Test pour le form de database
            //var viewModel = new DatabaseParametersViewModel(api);

            //Bypass du login

            //NavigateToModuleWorkBenchAsync();
            //this.LoginMenu.Add(new BasicMenuViewModel());
            //this.NavigateToModuleWorkBenchAsync();
            //this.ViewModels.Add(viewModel);
            this.NavigateToModuleWorkBenchAsync();
        }

        //public ObservableCollection<ViewModelBase> MenuModule
        //{
        //    get
        //    {
        //        if (_viewModels == null)
        //        {
        //            _viewModels = new ObservableCollection<ViewModelBase>();
        //        }
        //        return _viewModels;
        //    }
        //}

        //private ObservableCollection<BasicMenuViewModel> _loginMenu;
        //public ObservableCollection<BasicMenuViewModel> LoginMenu
        //{
        //    get
        //    {
        //        if (_loginMenu == null)
        //        {
        //            _loginMenu = new ObservableCollection<BasicMenuViewModel>();
        //        }
        //        return _loginMenu;
        //    }
        //    set
        //    {
        //        if (_loginMenu == value) return;
        //        _loginMenu = value;
        //        OnPropertyChanged("LoginMenu");
        //    }
        //}

        private ObservableCollection<CoreMenuViewModel> _coreMenu;
        public ObservableCollection<CoreMenuViewModel> CoreMenu
        {
            get
            {
                if (_coreMenu == null)
                {
                    _coreMenu = new ObservableCollection<CoreMenuViewModel>();
                }
                return _coreMenu;
            }
            set
            {
                if (_coreMenu == value) return;
                _coreMenu = value;
                OnPropertyChanged("CoreMenu");
            }
        }



        public ObservableCollection<ViewModelBase> ViewModels
        {
            get
            {
                if (_viewModels == null)
                {
                    _viewModels = new ObservableCollection<ViewModelBase>();
                }
                return _viewModels;
            }
            set
            {
                if (_viewModels == value) return;
                _viewModels = value;
                this.OnPropertyChanged("ViewModels");
            }
        }

        async void NavigateToModuleWorkBenchAsync()
        {
            var catalog = _catalog as DynamicDirectoryModuleCatalog;
            await Task.Run(() => { catalog.LoadAllModulesInTheDirectory(); });
//            this.LoginMenu.Clear();
            this.WindowHeight = 900;
            this.WindowWidth = 1500;
            this.CoreMenu.Add(new CoreMenuViewModel());
            var viewModel = new ModuleWorkBenchViewModel(_catalog, _container, _manager, _metadataCatalog);
            LoginViewModel tmp = null;
            if (this.ViewModels.Count() > 0) tmp = this.ViewModels.First() as LoginViewModel;
            if (tmp != null) this._api.LoggedUser = tmp.LoggedUser;

            this.ViewModels.Clear();
            this.ViewModels.Add(viewModel);
        }

        void NavigateToModuleWorkBench(object sender, EventArgs args)
        {
            this.NavigateToModuleWorkBenchAsync();
        }
        
        int _windowHeigh;
        public int WindowHeight
        {
            get
            {
                return _windowHeigh;
            }
            set
            {
                if (_windowHeigh == value) return;
                _windowHeigh = value;
                this.OnPropertyChanged("WindowHeight");
            }
        }

        int _windowWidth;
        public int WindowWidth
        {
            get
            {
                return _windowWidth;
            }
            set
            {
                if (_windowWidth == value) return;
                _windowWidth = value;
                this.OnPropertyChanged("WindowWidth");
            }
        }

        public void WindowIsClosing(object sender, EventArgs args)
        {
            List<ModuleMetadata> toBeDeleted = _container.Resolve(typeof(object), "toBeDeleted") as List<ModuleMetadata>;
            IMetadataModuleCatalog metadataCatalog = _container.Resolve<IMetadataModuleCatalog>();


            foreach (ModuleMetadata elem in toBeDeleted)
            {
                if (File.Exists("./Modules/" + elem.ModuleName + ".dll"))
                {
                    if (elem.State == ModuleStatus.ToBeDeletedNotKeepingData)
                    {
                        bool deleteThisTable = true;

                        foreach (string tableUsedByElem in elem.BDDTableUsed)
                        {
                            deleteThisTable = true;

                            foreach (ModuleMetadata module in metadataCatalog.ModuleMetadata)
                            {
                                if (module != elem)
                                {
                                    foreach (string table in module.BDDTableUsed)
                                    {
                                        if (table == tableUsedByElem)
                                            deleteThisTable = false;
                                    }
                                }
                            }

                            if (deleteThisTable == true)
                            {
                                foreach (ModuleMetadata module in toBeDeleted)
                                {
                                    if (module != elem && module.State == ModuleStatus.ToBeDeletedKeepingData)
                                    {
                                        foreach (string table in module.BDDTableUsed)
                                        {
                                            if (table == tableUsedByElem)
                                                deleteThisTable = false;
                                        }
                                    }
                                }
                            }
                            if (deleteThisTable == true)
                            {
                                System.Console.Error.WriteLine("On supprime la table : " + tableUsedByElem);
                                // on est censé drop la table la mais on le fais pas pendant le dev' histoire de pas faire chier ceux qui dev' sur leur modules
                            }
                        }
                    }
                    System.Console.Error.WriteLine("Et BIM JE SUPPRIME LE MODULE " + elem.ModuleName + ".dll");
                    File.Delete("./Modules/" + elem.ModuleName + ".dll");
                }
            }
            System.Console.Error.WriteLine("Et c'est la fin");
        }
    }
}
