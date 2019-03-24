using GUI.Views;
using Microsoft.Practices.ServiceLocation;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ModuleDef
{
    public class Main : IModule
    {
        public void Initialize()
        {
            System.Diagnostics.Debug.WriteLine("Initialised: GUI");

            IRegionManager regionmanager = ServiceLocator.Current.GetInstance<IRegionManager>();

            regionmanager.RegisterViewWithRegion("MainRegion", typeof(MainView));
            regionmanager.RegisterViewWithRegion("LoggerRegion", typeof(LogView));
        }
    }
}
