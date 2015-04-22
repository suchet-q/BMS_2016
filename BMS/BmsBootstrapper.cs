using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BMS
{
    internal class BmsBootstrapper : UnityBootstrapper
    {
        protected override void ConfigureContainer()
        {
            this.Container.RegisterType<IShellView, Shell>();

            base.ConfigureContainer();
        }
        protected override DependencyObject CreateShell()
        {
            ShellPresenter presenter = Container.Resolve<ShellPresenter>();
            IShellView view = presenter.View;
            view.ShowView();
            return view as DependencyObject;
        }
    }
}
