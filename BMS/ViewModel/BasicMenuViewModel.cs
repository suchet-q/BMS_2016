using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BMS.ViewModel
{
    public class BasicMenuViewModel : ViewModelBase
    {
        public BasicMenuViewModel()
        {

            this.NavigateToDatabaseParametersCommand = new DelegateCommand((o) => this.NavigateToDatabaseParameters());
        }

        private void NavigateToDatabaseParameters()
        {
            System.Console.Error.WriteLine("ET ON LANCE LA NAVIGATION VERS LE DATABASE PARAMETERS");
        }

        public ICommand NavigateToDatabaseParametersCommand { get; set; }
    }
}
