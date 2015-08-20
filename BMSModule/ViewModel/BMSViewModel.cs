using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMSModule.ViewModel
{
    class BMSViewModel : BindableBase
    {
        private readonly DelegateCommand<string> _clickCommand;
        private readonly IModuleCatalog _catalog;
        protected List<String> _moduleListString;
        public List<String> ModuleListString
        {
            get
            {
                return this._moduleListString;
            }
            set { return; }
        }

        public BMSViewModel()
        {
            _clickCommand = new DelegateCommand<string>(
          (s) => { this.ExecuteOnClickCommand(); }, //Execute
          (s) => { return !string.IsNullOrEmpty(_input); } //CanExecute
          );
        }
        public void ExecuteOnClickCommand()
        {
            Input = string.Empty;
            _input = Input;
        }

        public DelegateCommand<string> ButtonClickCommand
        {
            get { return _clickCommand; }
        }
        private string _input;
        public string Input
        {
            get { return _input; }
            set
            {
                if (_input != value)
                {
                    _input = value;
                    OnPropertyChanged("Input");
                }
                _input = value;
                _clickCommand.RaiseCanExecuteChanged();
            }
        }
    }
}
