using Common.Data.FileMapping;
using Common.Interfaces;
using DAL.CSV;
using DAL.Helpers;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ImportActions
{
    public class FileUpdater
    {
        private List<FileMap> _ListOfMaps;
        private List<List<object>> _ListOfFileData;
        private List<List<object>> _ListOfUpdates;
        private string _ImportFolder;

        public FileUpdater(string importFolder, List<List<object>> FileData)
        {
            this._ImportFolder = importFolder;
            IResourceManager manager = ServiceLocator.Current.GetInstance<IResourceManager>();
            this._ListOfMaps = (List<FileMap>)manager.GetResourcePayload(Common.Constants.ResourceName.Mapping_Updates);
            this._ListOfUpdates = new List<List<object>>();
            this._ListOfFileData = FileData;
        }

        internal List<List<object>> UpdateData()
        {
            GenerateUpdates();

            foreach (List<object> UpdateRecords in this._ListOfUpdates)
            {
                Type RecordType = UpdateRecords.ElementAt(0).GetType();
                List<object> new_List = new List<object>();
                List<object> FileList = this._ListOfFileData.Where(i => i.ElementAt(0).GetType() == RecordType).FirstOrDefault();

                foreach (FileMap map in this._ListOfMaps.Where(i => i.TableType == RecordType))
                {
                    IEnumerable<PropertyInfo> IndexProperty = PropertyHelper.GetIsIndex(RecordType, map);
                    IEnumerable<PropertyInfo> PropertiesToChange = PropertyHelper.GetPropertiesToChange(RecordType, map);

                    PropertyInfo Index = IndexProperty.FirstOrDefault();

                    if (IndexProperty.Count() >= 2 || Index == null)
                    {
                        throw new Exception("Update JSON File Can only have 1 index");
                    }

                    foreach (object record in FileList)
                    {
                        object recordIndexValue = RecordType.GetProperty(Index.Name).GetValue(record);

                        object filerecord = UpdateRecords.Find(delegate (object obj)
                        {
                            return Index.GetValue(obj).Equals(recordIndexValue);
                        });

                        if (filerecord != null)
                        {
                            foreach (PropertyInfo prop in PropertiesToChange)
                            {
                                prop.SetValue(record, prop.GetValue(filerecord));
                            }
                        }
                        new_List.Add(record);
                    }
                }
                this._ListOfFileData.Remove(FileList);

                this._ListOfFileData.Add(new_List);
            }
            return this._ListOfFileData;
        }

        internal void GenerateUpdates()
        {
            foreach (FileMap map in this._ListOfMaps)
            {
                Reader reader = new Reader(this._ImportFolder + map.FileName, map);
                reader.ReadFile();
                this._ListOfUpdates.Add(reader.ListOfRecords);
            }
        }
    }
}
