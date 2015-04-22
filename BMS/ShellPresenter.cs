using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS
{
    class ShellPresenter
    {
        public IShellView View { get; private set; }

        public ShellPresenter(IShellView view)
        {
            this.View = view;
        }
    }
}
