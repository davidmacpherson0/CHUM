using Common.Data.FileMapping;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.CSV
{
    public class Reader
    {
        private CsvReader _CsvReader;
        private FileMap _FileMap;
        private List<object> _ListOfRecords;

        public Reader(string FilePath, FileMap fileMap)
        {
            this.ListOfRecords = new List<object>();
            this._CsvReader = new CsvReader(File.OpenText(FilePath));
            this._FileMap = fileMap;
            this._CsvReader.Configuration.RegisterClassMap(new ClassMapper(this._FileMap));
            this._CsvReader.Read();
            this._CsvReader.ReadHeader();
            this._CsvReader.Configuration.BadDataFound = this.badData;
            //System.Diagnostics.Debug.WriteLine(this._CsvReader.GetField(0).ElementAt(0).ToString());
        }

        private void badData(IReadingContext attr)
        {
            System.Diagnostics.Debug.WriteLine(attr.RawRecord);
        }

        public void ReadFile()
        {
            System.Diagnostics.Debug.WriteLine("About to read the file");
            System.Diagnostics.Debug.WriteLine("\tusing Map: " + this._FileMap.FileName + " to table: " + this._FileMap.TableType.ToString());
            while (this._CsvReader.Read())
            {
                
                object FileRecord = null;
                try
                {
                    FileRecord = this._CsvReader.GetRecord(this._FileMap.TableType);
                }
                catch (Exception)
                {

                    throw;
                }
                
                FileRecord = SetConstants(FileRecord);
                if (FileRecord == null)
                {
                    System.Diagnostics.Debug.WriteLine("Null Record");
                }
                else
                {
                    this.ListOfRecords.Add(FileRecord);
                }
            }
            System.Diagnostics.Debug.WriteLine(this.ListOfRecords);
        }

        private object SetConstants(object fileRecord)
        {
            object temp = fileRecord;
            IEnumerable<Map> defaults = from def in this._FileMap.FieldMap
                                        where def.DefaultData != null
                                        select def;

            foreach (Map map in defaults)
            {
                temp.GetType().GetProperty(map.TableFieldName).SetValue(temp, map.DefaultData);
            }
            return temp;
        }

        public List<object> ListOfRecords
        {
            get { return _ListOfRecords; }
            private set { _ListOfRecords = value; }
        }
    }
}




/*
  CsvReader reader = new CsvReader(File.OpenText(Filepath));
            ClassMapper map = new ClassMapper(fileMap);
            reader.Configuration.RegisterClassMap(map);
            reader.Configuration.HeaderValidated = null;
            reader.Configuration.MissingFieldFound = null;
            reader.Read();
            reader.ReadHeader();
     
     */
