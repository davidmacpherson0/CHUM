using Common.Interfaces;
using DAL.CSV;
using DAL.Database;
using DAL.ImportActions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DAL.ModuleDef
{
    public class Main : IModule
    {
        public void Initialize()
        {
            System.Diagnostics.Debug.WriteLine("Initialised: DAL");


            IUnityContainer container = ServiceLocator.Current.GetInstance<IUnityContainer>();
            //container.RegisterType<IImport, Import>();
            container.RegisterType<IImport, Import>();
            container.RegisterType<IExport, Export.Actions.Export>();
            container.RegisterType<IDBActions, DBActions>();

            //ntainer.RegisterType<ICSVReader, Reader>();




        }
    }
}
