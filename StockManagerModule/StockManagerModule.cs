﻿using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockManagerModule
{
    public class StockManagerModule : IModule
    {

        IAPI        _api;
        IUnityContainer _container;

        public StockManagerModule(IAPI api, IUnityContainer container)
        {
            _api = api;
            _container = container;
        }

        public void Initialize()
        {
            var viewModel = new ViewModel.StockManagerModuleViewModel(_api);
            var view = new View.StockManagerModuleView();
            view.DataContext = viewModel;

            _container.RegisterInstance(typeof(object), "StockManagerModuleView", view);

        }
    }
}