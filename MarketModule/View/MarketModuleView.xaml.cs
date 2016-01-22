using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MarketModule.View
{
    /// <summary>
    /// Interaction logic for MarketModuleView.xaml
    /// </summary>
    public partial class MarketModuleView : UserControl
    {
        //public DelegateCommand RefreshCommand;
        public MarketModuleView()
        {
            InitializeComponent();
            //RefreshCommand = new DelegateCommand(Refresh_Executed, () => { return Refresh_CanExecute(); });
        }

        //private void BrowseBack_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        //{
        //    e.CanExecute = ((wbSample != null) && (wbSample.CanGoBack));
        //}

        //private void BrowseBack_Executed(object sender, ExecutedRoutedEventArgs e)
        //{
        //    wbSample.GoBack();
        //}

        //private void BrowseForward_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        //{
        //    e.CanExecute = ((wbSample != null) && (wbSample.CanGoForward));
        //}

        //private void BrowseForward_Executed(object sender, ExecutedRoutedEventArgs e)
        //{
        //    wbSample.GoForward();
        //}

        //bool Refresh_CanExecute(/*object sender, CanExecuteRoutedEventArgs e*/)
        //{
        //    //e.CanExecute = true;
        //    return true;
        //}

        //void Refresh_Executed(/*object sender, ExecutedRoutedEventArgs e*/)
        //{
        //    wbSample.Refresh();
        //}

        private void ButtonBrowseBack_Click(object sender, RoutedEventArgs e)
        {
            if (((wbSample != null) && (wbSample.CanGoBack)))
                wbSample.GoBack();
        }

        private void ButtonBrowseForward_Click(object sender, RoutedEventArgs e)
        {
            if (((wbSample != null) && (wbSample.CanGoForward)))
                wbSample.GoForward();
        }

        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            wbSample.Refresh();
        }
    }
}
