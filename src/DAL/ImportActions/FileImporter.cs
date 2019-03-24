using Common.Constants;
using Common.Data.FileMapping;
using Common.Interfaces;
using DAL.CSV;
using DAL.Database.Model;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ImportActions
{
    public class FileImporter
    {
        private IEnumerable<FileMap> _ListofFileMaps;
        private static readonly object lockobj = new object();
        private List<List<object>> _FileData;

        public FileImporter()
        {
            IResourceManager Manager = ServiceLocator.Current.GetInstance<IResourceManager>();
            this._ListofFileMaps = (IEnumerable<FileMap>)Manager.GetResourcePayload(ResourceName.Mapping_Import);
            this._FileData = new List<List<object>>();
        }

        public IEnumerable<Type> GetTypes()
        {
            return (from data in this._FileData.OrderBy(i => i.Count())
                    from tye in data
                    select tye.GetType()).Distinct();


        }

        public List<List<object>> GetFileObjs(string ImportFolder)
        {
            System.Diagnostics.Debug.WriteLine("Running imports");
            RunTasks(ImportFolder);
            System.Diagnostics.Debug.WriteLine("Running Done");
            this._FileData = Combiner();
            return this._FileData;
        }

        private void RunTasks(string importFolder)
        {
            List<Task> tasks = new List<Task>();
            foreach (FileMap fileMap in this._ListofFileMaps)
            {
                //CreateTask(importFolder + fileMap.FileName, fileMap);


                tasks.Add(Task.Factory.StartNew(() => CreateTask(importFolder + fileMap.FileName, fileMap)));
            }

            System.Diagnostics.Debug.WriteLine("All Tasks started");
            Task.WaitAll(tasks.ToArray());

            System.Diagnostics.Debug.WriteLine("All Tasks Completed");
            System.Diagnostics.Debug.WriteLine("Saving Context");
        }

        private void CreateTask(string Filepath, FileMap fileMap)
        {

            System.Diagnostics.Debug.WriteLine("Reading File: " + fileMap.FileName);
            Reader reader = new Reader(Filepath, fileMap);
            System.Diagnostics.Debug.WriteLine("\tFile has been Mapped Correctly");
            reader.ReadFile();
            System.Diagnostics.Debug.WriteLine("Done Reading File: " + fileMap.FileName + " Mapped to obj: " + fileMap.TableType.Name + " Total Records Read: " + reader.ListOfRecords.Count());
            this._FileData.Add(reader.ListOfRecords);
        }

        private List<List<object>> Combiner()
        {

            IEnumerable<Type> ListOfTypes = GetTypes();

            List<List<object>> newData = new List<List<object>>();

            foreach (Type type in ListOfTypes)
            {
                IEnumerable<object> data = from list in this._FileData
                                           from inner in list
                                           where inner.GetType() == type
                                           select inner;
                newData.Add(data.ToList());
            }


            newData = (from lis in newData
                       orderby lis.Count() ascending
                       select lis).ToList();

            foreach (List<object> list in newData)
            {
                System.Diagnostics.Debug.WriteLine(list.ElementAt(0).GetType().Name + " Count: " + list.Count());
            }

            return newData;
        }



    }
}
