using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public abstract class ABMSModule : IModule
    {
        protected abstract void RegisterType(IUnityContainer container);
        public abstract void Initialize();

        protected ABMSModule(IUnityContainer container)
        {
            this.RegisterType(container);
        }
    }
}
