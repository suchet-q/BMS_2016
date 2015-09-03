using Microsoft.Win32;
using System;
using System.Collections;
using System.Windows;
using System.IO;

namespace BMS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    { 
        protected override void OnStartup(StartupEventArgs e)
        {
            string myVar = "BMS";
            string value = Path.GetFullPath("./Modules");

            ConsoleManager.Show();
            base.OnStartup(e);
            if (Environment.GetEnvironmentVariable(myVar) == null)
            {
                Environment.SetEnvironmentVariable(myVar, value, EnvironmentVariableTarget.Machine);
            }
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }
    }
}
