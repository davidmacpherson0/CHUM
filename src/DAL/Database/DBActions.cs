using Common.Constants;
using Common.Interfaces;
using DAL.Database.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Database
{
    public class DBActions : IDBActions
    {
        public void FlushDatabase()
        {
            using (CHUMDB context = new CHUMDB())
            {
                
                string Data = File.ReadAllText(FileLocations.CONFIG_FOLDER_PATH + "Schema.sql");
                context.Database.ExecuteSqlCommand(Data);
            }
        }
    }
}
