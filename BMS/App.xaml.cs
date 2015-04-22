using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BMS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            ConsoleManager.Show();
            BmsBootstrapper bootStrapper = new BmsBootstrapper();
            bootStrapper.Run();
        }
    }
}
