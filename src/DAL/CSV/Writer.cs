using CsvHelper;
using DAL.Export.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.CSV
{
    public class Writer
    {
        private ExportPayload _ExportPayload;

        public Writer(ExportPayload Payload)
        {
            this._ExportPayload = Payload;

        }

        public void WriteFile()
        {
            using (TextWriter writer = new StreamWriter(this._ExportPayload.FilePath))
            {
                CsvWriter CSV = new CsvWriter(writer);
                CSV.Configuration.HasHeaderRecord = this._ExportPayload.ShowHeader;
                CSV.Configuration.QuoteNoFields = this._ExportPayload.QuoteField;

                System.Diagnostics.Debug.WriteLine("Writing file: " + this._ExportPayload.FilePath + " number of Lines: " + this._ExportPayload.Data.Count());
                CSV.WriteRecords(this._ExportPayload.Data);
            }

        }
    }
}
