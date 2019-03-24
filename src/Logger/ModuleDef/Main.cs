using Common.Interfaces;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger.ModuleDef
{
    public class Main : IModule
    {
        public void Initialize()
        {
            System.Diagnostics.Debug.WriteLine("Initialised: Logger");
            IUnityContainer container = ServiceLocator.Current.GetInstance<IUnityContainer>();
            container.RegisterType<ILog, Log>();
        }
    }
}
