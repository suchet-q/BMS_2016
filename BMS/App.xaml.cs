
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
            ConsoleManager.Show();
            base.OnStartup(e);
            Bootstrapper bootstrapper = new Bootstrapper();
            System.Console.Error.WriteLine("GOGOGOGOGOGOGOGOOOOOOOOOOOOOOOOOOOOOO");

            bootstrapper.Run();
        }
    }
}
