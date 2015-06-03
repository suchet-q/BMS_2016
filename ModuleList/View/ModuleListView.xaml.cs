using ModuleList.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ModuleList.View
{
    /// <summary>
    /// Interaction logic for ModuleListView.xaml
    /// </summary>
    public partial class ModuleListView : UserControl
    {
        public ModuleListView(IModuleListViewModel viewModel)
        {
            this.DataContext = viewModel;
            InitializeComponent();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1)
            displayStock.Text = ((Stock)e.AddedItems[0]).ToString();
        }
    }
}
