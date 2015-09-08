using Microsoft.Practices.Prism.Modularity;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ModuleManagerModule.ViewModel
{
    public class ModuleViewModel : ViewModelBase
    {
        ModuleMetadata _metadata;
        public ModuleMetadata Metadata
        {
            get
            {
                return _metadata;
            }
            private set
            {
                return;
            }
        }

        public ModuleViewModel(ModuleMetadata metadata)
        {
            _metadata = metadata;
        }


        public string Name
        {
            get
            {
                return _metadata.Name;
            }
        }

        public string Version
        {
            get
            {
                return _metadata.Version;
            }
        }

        public string Description
        {
            get
            {
                return _metadata.Description;
            }
        }

        public string CreatedBy
        {
            get
            {
                return _metadata.CreatedBy;
            }
        }

        public string StateString
        {
            get
            {
                if (_metadata.State == ModuleStatus.Activated)
                    return "Activated";
                else if (_metadata.State == ModuleStatus.ToBeDeleted)
                    return "Will be deleted";
                return "Unknown state";
            }
        }
        public ModuleStatus State
        {
            get
            {
                return _metadata.State;
            }
            set
            {
                if (_metadata.State == value) return;
                _metadata.State = value;
                this.OnPropertyChanged("State");
                this.OnPropertyChanged("StateString");
            }
        }   


    }
}
