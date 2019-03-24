using Common.Interfaces;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.ModuleDef
{
    public class Main : IModule
    {
        public void Initialize()
        {
            IUnityContainer container = ServiceLocator.Current.GetInstance<IUnityContainer>();
            container.RegisterType<ICSVReader, Reader>();

            System.Diagnostics.Debug.WriteLine("Initialised: IO");
        }
    }
}
