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
                     ZipFile.ExtractToDirectory(dlg.FileName, resultEnv);
                 }
                 else
                 {
                     FolderBrowserDialog fbd = new FolderBrowserDialog();
                     DialogResult resultFBD = fbd.ShowDialog();
                     ZipFile.ExtractToDirectory(dlg.FileName, fbd.SelectedPath);
                 }
             }
         }
    }
}
