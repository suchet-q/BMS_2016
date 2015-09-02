using Microsoft.Win32;
using System;
using System.Collections;
using System.Windows;

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
            string exists = "MODULE_BMS";

            ConsoleManager.Show();
            base.OnStartup(e);
            if (Environment.GetEnvironmentVariable(myVar) == null)
            {
                Environment.SetEnvironmentVariable(myVar, exists, EnvironmentVariableTarget.User);
            }
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }
    }
}
