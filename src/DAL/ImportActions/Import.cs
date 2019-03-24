using Common.Constants;
using Common.Data;
using Common.Interfaces;
using DAL.Database.Model;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Diagnostics;

namespace DAL.ImportActions
{
    public class Import : IImport
    {
        private FileImporter _FileImporter;
        private DbImporter _DbImporter;

        public object StopWatch { get; private set; }

        public Import()
        {
            this._FileImporter = new FileImporter();
            this._DbImporter = new DbImporter();
        }

        public void GoImport(string ImportFolder)
        {


            System.Diagnostics.Debug.WriteLine("Generating objs from File");
            List<List<object>> FileData = this._FileImporter.GetFileObjs(ImportFolder);
            System.Diagnostics.Debug.WriteLine("Generating objs from File - Done");
            System.Diagnostics.Debug.WriteLine("Getting Updates to File Data");

            FileUpdater updater = new FileUpdater(ImportFolder, FileData);


            FileData = updater.UpdateData();

            System.Diagnostics.Debug.WriteLine("Getting Updates to File Data - Done");

            System.Diagnostics.Debug.WriteLine("Getting Db data");
            List<List<object>> DbData = this._DbImporter.GenerateDbobj(this._FileImporter.GetTypes());
            System.Diagnostics.Debug.WriteLine("Getting Db data - Done ");

            System.Diagnostics.Debug.WriteLine("\r\nSTATS\r\n");
            VerboseLog(FileData, DbData);
            System.Diagnostics.Debug.WriteLine("\r\nProcessing To DB\r\n");

            foreach (Type type in this._FileImporter.GetTypes())
            {
                IEnumerable<object> fileTableData = GetData(type, FileData);
                IEnumerable<object> dbTableData = GetData(type, DbData);

                System.Diagnostics.Debug.WriteLine("Processing Table: " + type.Name);

                if (fileTableData.Count() >= 1)
                {
                    CompareData(fileTableData, dbTableData);
                }

            }
            System.Diagnostics.Debug.WriteLine("-------------------       Import Complete       -------------------");
        }

        private void CompareData(IEnumerable<object> fileData, IEnumerable<object> dbData)
        {
            Type RecordType = fileData.ElementAt(0).GetType();

            System.Diagnostics.Debug.WriteLine("Distinct File Count: " + fileData.Count());

            IEnumerable<PropertyInfo> KeyProperties = (from prop in RecordType.GetProperties()
                                                       from att in prop.CustomAttributes
                                                       where att.AttributeType == typeof(KeyAttribute)
                                                       select prop);

            IEnumerable<PropertyInfo> PropertiesToChagne = from pro in RecordType.GetProperties()
                                                           from not in KeyProperties
                                                           where pro.Name != not.Name
                                                           select pro;

            Stopwatch SW = new Stopwatch();

            using (CHUMDB context = new CHUMDB())
            {
                ILog Logger = ServiceLocator.Current.GetInstance<ILog>();

                List<object> RangeAdd = new List<object>();
                int count = 1;
                foreach (object fileRecord in fileData)
                {
                    if ((count % 1000) == 0)
                    {
                        Logger.LogMessage("Processed: " + RecordType.Name + "\tRecord: " + count + "/" + fileData.Count() + " in: " + SW.ElapsedMilliseconds + " ms");
                        SW.Restart();
                    }

                    //CHEKING db
                    object Record = GetRecordFromDB(fileRecord, dbData);

                    if (Record == null)
                    {
                        object[] Propvales = KeyProperties.Select(i => i.GetValue(fileRecord)).ToArray();

                        if (Propvales.Where(i => string.IsNullOrWhiteSpace(i.ToString())).Count() == 0)
                        {
                            RangeAdd.Add(fileRecord);
                        }
                    }
                    else
                    {
                        object[] Propvales = KeyProperties.Select(i => i.GetValue(Record)).ToArray();

                        object DBRecord = context.Set(Record.GetType()).Find(Propvales);
                        foreach (PropertyInfo prop in PropertiesToChagne)
                        {
                            if (prop.GetValue(DBRecord) != prop.GetValue(fileRecord))
                            {
                                prop.SetValue(DBRecord, prop.GetValue(fileRecord));
                            }
                        }

                    }
                    count++;
                }
                System.Diagnostics.Debug.WriteLine("Adding range data for: " + RecordType.Name);

                context.Set(RecordType).AddRange(RangeAdd);
                System.Diagnostics.Debug.WriteLine("Adding range data Done");
                Logger.LogMessage("Saving Data");
                Logger.LogMessage("Data Saved: " + context.SaveChanges());

            }
        }

        private object GetRecordFromDB(object fileRecord, IEnumerable<object> dbData)
        {
            IResourceManager manager = ServiceLocator.Current.GetInstance<IResourceManager>();

            List<DBKeyData> Keydata = (List<DBKeyData>)manager.GetResourcePayload(ResourceName.Mapping_DBSchema);

            DBKeyData Keys = (from key in Keydata
                              where key.Table == fileRecord.GetType()
                              select key).FirstOrDefault();

            IEnumerable<object> data = dbData.ToList<object>();
            if (Keys != null)
            {
                foreach (string KeyName in Keys.KeyNames)
                {
                    data = SearchList(data, KeyName, fileRecord.GetType().GetProperty(KeyName).GetValue(fileRecord));
                }
            }
            if (data.Count() == 0)
            {
                return null;
            }

            return data.ElementAt(0);
        }

        private IEnumerable<object> SearchList(IEnumerable<object> ListData, string propertyname, object value)
        {
            return from data in ListData
                   where data.GetType().GetProperty(propertyname).GetValue(data).Equals(value)
                   select data;
        }

        private void VerboseLog(List<List<object>> FileData, List<List<object>> DbData)
        {
            System.Diagnostics.Debug.WriteLine("Type Count -> File: " + FileData.Count() + " DBCount: " + DbData.Count());

            System.Diagnostics.Debug.WriteLine("--------   File Count   --------");

            foreach (Type type in this._FileImporter.GetTypes())
            {
                int count = (GetData(type, FileData)).Count();
                System.Diagnostics.Debug.WriteLine("Type: " + type.Name + " -> " + count);
            }
            System.Diagnostics.Debug.WriteLine("--------------------------------");

            System.Diagnostics.Debug.WriteLine("");

            System.Diagnostics.Debug.WriteLine("------   Database Count   ------");

            foreach (Type type in this._FileImporter.GetTypes())
            {
                int count = (GetData(type, DbData)).Count();

                System.Diagnostics.Debug.WriteLine("Type: " + type.Name + " -> " + count);
            }
            System.Diagnostics.Debug.WriteLine("--------------------------------");
        }

        private IEnumerable<object> GetData(Type type, List<List<object>> Data)
        {

            IResourceManager manager = ServiceLocator.Current.GetInstance<IResourceManager>();

            List<DBKeyData> Keydata = (List<DBKeyData>)manager.GetResourcePayload(ResourceName.Mapping_DBSchema);



            DBKeyData Keys = (from key in Keydata
                              where key.Table == type
                              select key).FirstOrDefault();

            if (Keys != null)
            {
                Comparer comp = new Comparer(type, Keys.KeyNames);

                return (from data in Data
                        from da in data
                        where da.GetType() == type
                        select da).Distinct(comp);
            }
            else
            {
                return (from data in Data
                        from da in data
                        where da.GetType() == type
                        select da).Distinct();
            }



        }



    }
}
//foreach (DBKeyData DBKey in Keydata)
//{
//    if (DBKey.KeyNames.Count() == 0)
//    {
//        throw new Exception("No Keys Mapped in JSON: " + DBKey.Table.Name);
//    }
//    else if (DBKey.KeyNames.Count == 1)
//    {
//        ProcessSingleKeySearch(fileData, dbData, DBKey.KeyNames.ElementAt(0));
//    }
//    else
//    {
//        ProcessMultiKeySearch(fileData, dbData, DBKey.KeyNames);
//    }


//}        //private void ProcessSingleKeySearch(IEnumerable<object> fileData, IEnumerable<object> dbData, string KeyName)
//{
//    List<object> AddList = new List<object>();

//    foreach (object fileRecord in fileData)
//    {
//        object record = from db in dbData
//                        where db.GetType().GetProperty(KeyName).GetValue(db) == fileRecord.GetType().GetProperty(KeyName).GetValue(fileRecord)
//                        select db;

//        if (record == null)
//        {
//            AddList.Add(record);
//        }
//    }

//    using (CHUMDB context = new CHUMDB())
//    {
//        context.Set(fileData.ElementAt(0).GetType()).AddRange(AddList);
//        System.Diagnostics.Debug.WriteLine("Saveing added List: " + context.SaveChanges());
//    }
//}

//private void ProcessMultiKeySearch(IEnumerable<object> fileData, IEnumerable<object> dbData, List<string> keyNames)
//{

//}