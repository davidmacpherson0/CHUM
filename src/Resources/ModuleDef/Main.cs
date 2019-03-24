using Common.Interfaces;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Resources.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resources.ModuleDef
{
    public class Main : IModule
    {
        public void Initialize()
        {
            IUnityContainer container = ServiceLocator.Current.GetInstance<IUnityContainer>();

            IResourceManager resourceManager = new ResourceManager(); ;

            container.RegisterInstance<IResourceManager>(resourceManager);
            //container.RegisterType<IMakeFile, MakeImportJSON>();
        }
    }
}
