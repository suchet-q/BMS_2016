// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;

namespace BMSModule
{
    public class BMSModule : IModule
    {
        private readonly IRegionViewRegistry regionViewRegistry;

        public BMSModule(IRegionViewRegistry registry)
        {
            this.regionViewRegistry = registry;
        }

        public void Initialize()
        {
            regionViewRegistry.RegisterViewWithRegion("MainRegion", typeof(View.BMSView));
        }
    }
}
