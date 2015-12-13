using Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.IO;
using System.IO.Compression;

namespace BMS.ViewModel
{
    public class CoreMenuViewModel : BasicMenuViewModel
    {

        public CoreMenuViewModel()
        {      
            this.AddModuleByPathCommand = new DelegateCommand((o) => this.AddModuleByPath());
        }

        public ICommand AddModuleByPathCommand { get; private set; }
        public ICommand NavigateToModuleManagerCommand { get; private set; }


        async void UnzipModule(Microsoft.Win32.OpenFileDialog dlg, FolderBrowserDialog fbd)
        {
            await Task.Run(() => { ZipFile.ExtractToDirectory(dlg.FileName, fbd.SelectedPath); });
        }
        async void UnzipModuleEnv(Microsoft.Win32.OpenFileDialog dlg, string resultEnv)
        {
            await Task.Run(() => { ZipFile.ExtractToDirectory(dlg.FileName, resultEnv); });
        }

        public void AddModuleByPath()
        {
            System.Console.WriteLine("creation de la class popup");
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".mod";
            dlg.Filter = "Module Files (*.mod)|*.mod";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string resultEnv = null;
                resultEnv = Environment.GetEnvironmentVariable("MODULE_PATH");
                if (resultEnv != null)
                {
                    UnzipModuleEnv(dlg, resultEnv);
                }
                else
                {
                    FolderBrowserDialog fbd = new FolderBrowserDialog();
                    DialogResult resultFBD = fbd.ShowDialog();
                    UnzipModule(dlg, fbd);
                }
            }
        }
    }
}
