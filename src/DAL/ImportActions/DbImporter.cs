using DAL.Database.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace DAL.ImportActions
{
    internal class DbImporter
    {


        public DbImporter()
        {

        }


        internal List<List<object>> GenerateDbobj(IEnumerable<Type> Types)
        {
            List<List<object>> data = new List<List<object>>();


            foreach (Type type in Types)
            {
                List<object> outrecord = new List<object>();

                using (CHUMDB context = new CHUMDB())
                {
                    var records = context.Set(type);
                    foreach (var record in records)
                    {
                        outrecord.Add(record);
                    }
                }
                data.Add(outrecord);
            }
            return data;
        }
    }
}