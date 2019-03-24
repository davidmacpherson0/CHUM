using Common.Interfaces;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO
{
    public class Reader : ICSVReader
    {
        public void Read(string FilePath)
        {
            System.Diagnostics.Debug.WriteLine(FilePath);

            TextReader TR = File.OpenText(FilePath);



            CsvReader csv = new CsvReader(TR);
            csv.GetField()

            



        }
    }
}
