using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
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

namespace BMS.View
{
    /// <summary>
    /// Interaction logic for ModuleWorkBench.xaml
    /// </summary>
    public partial class ModuleWorkBench : UserControl
    {
        public ModuleWorkBench()
        {
            InitializeComponent();

            IRegionManager regionManager = ServiceLocator.Current.GetInstance<IRegionManager>();
            regionManager.Regions.Remove("MainModuleRegion");
            RegionManager.SetRegionManager(this, regionManager);
            RegionManager.SetRegionName(this.MainModuleRegion, "MainModuleRegion");

        }

    }
}
