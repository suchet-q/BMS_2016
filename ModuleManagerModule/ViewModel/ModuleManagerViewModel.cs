using Microsoft.Practices.Unity;
using Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Prism.Modularity;

namespace ModuleManagerModule.ViewModel
{
   

    public class ModuleManagerModuleViewModel : ViewModelBase
    {
        IMetadataModuleCatalog _metadataCatalog;
        IUnityContainer _container;

        List<ModuleMetadata> _toBeDeleted;

        public ModuleManagerModuleViewModel(IMetadataModuleCatalog metadataCatalog, IUnityContainer container)
        {
            ModuleViewModel tmp;
            _metadataCatalog = metadataCatalog;
            _container = container;
            _toBeDeleted = _container.Resolve(typeof(object), "toBeDeleted") as List<ModuleMetadata>;

            _metadataCatalog.Changed += this.UpdateModuleList;

            this.ListAllModules = new ObservableCollection<ModuleViewModel>();
            foreach (ModuleMetadata elem in _metadataCatalog.ModuleMetadata)
            {
                this.ListAllModules.Add(new ModuleViewModel(elem));
            }


            this.CurrentModule = this.ListAllModules.Count() > 0 ? this.ListAllModules.First() : null;

            this.ListAllModules.CollectionChanged += (sender, e) =>
            {
                if (e.OldItems != null && e.OldItems.Contains(this.CurrentModule))
                {
                    this.ListAllModules = null;
                }
            };

            foreach (ModuleViewModel elem in this.ListAllModules)
            {
                foreach (ModuleMetadata module in _toBeDeleted)
                {
                    if (module == elem.Metadata)
                    {
                        elem.State = ModuleStatus.ToBeDeleted;
                    }
                }
            }

            this.ActivateCommand = new DelegateCommand((o) => this.Activate());
            this.ToBeDeletedCommand = new DelegateCommand((o) => this.ToBeDeleted());        
        }

        ObservableCollection<ModuleViewModel> _listAllModules;
        public ObservableCollection<ModuleViewModel> ListAllModules
        {
            get
            {
                return _listAllModules;
            }
            set
            {
                if (_listAllModules == value) return;
                _listAllModules = value;
                this.OnPropertyChanged("ListAllModules");
            }
        }

        ModuleViewModel _currentModule;
        public ModuleViewModel CurrentModule
        {
            get
            {
                return _currentModule;
            }
            set
            {
                if (_currentModule == value) return;
                _currentModule = value;
                this.OnPropertyChanged("CurrentModule");
                if (_currentModule == null) return;
                if (_currentModule.State == ModuleStatus.Activated)
                {
                    this.IsCurrentModuleActivate = true;
                }
                else if (_currentModule.State == ModuleStatus.ToBeDeleted)
                {
                    this.IsCurrentModuleActivate = false;
                }
            }
        }

        bool _isCurrentModuleActivate;
        public bool IsCurrentModuleActivate
        {
            get
            {
                return _isCurrentModuleActivate;
            }
            set
            {
                if (_isCurrentModuleActivate == value) return;
                _isCurrentModuleActivate = value;
                this.OnPropertyChanged("IsCurrentModuleActivate");
            }
        }


        public ICommand ActivateCommand { get; set; }
        public ICommand ToBeDeletedCommand { get; set; }


        void Activate()
        {

            ModuleViewModel tmp = this.CurrentModule;

            this.CurrentModule.State = ModuleStatus.Activated;
            foreach (ModuleMetadata elem in _metadataCatalog.ModuleMetadata)
            {
                if (elem.Name == CurrentModule.Metadata.Name)
                    elem.State = CurrentModule.Metadata.State;
            }
            System.Console.Error.WriteLine("J'active le module maggle");
            _toBeDeleted.Remove(this.CurrentModule.Metadata);
            this.IsCurrentModuleActivate = true;
            _metadataCatalog.OnChange(null);
            this.CurrentModule = this.ListAllModules[this.ListAllModules.IndexOf(tmp)];
        }

        void ToBeDeleted()
        {
            ModuleViewModel tmp = this.CurrentModule;

            this.CurrentModule.State = ModuleStatus.ToBeDeleted;
            foreach (ModuleMetadata elem in _metadataCatalog.ModuleMetadata)
            {
                if (elem.Name == CurrentModule.Metadata.Name)
                    elem.State = CurrentModule.Metadata.State;
            }
            _toBeDeleted.Add(this.CurrentModule.Metadata);
            System.Console.Error.WriteLine("Je dois supprimer le module maggle");
            this.IsCurrentModuleActivate = false;
            _metadataCatalog.OnChange(null);
            this.CurrentModule = this.ListAllModules[this.ListAllModules.IndexOf(tmp)];
        }

        void UpdateModuleList(object sender, EventArgs e)
        {
            this.ListAllModules.Clear();
            foreach (ModuleMetadata elem in _metadataCatalog.ModuleMetadata)
            {
                if (elem.ModuleName != "ModuleManagerModule")
                    this.ListAllModules.Add(new ModuleViewModel(elem));
            }
        }
    }
}
