using Common.Constants;
using DAL.Interfaces;
using DAL.Queries.Groups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Queries
{
    public class Query
    {
        private List<IQueries> _ListOfQueries;

        public Query()
        {
            Dictionary<string, string> Staff = new Dictionary<string, string>
            {
                { "User_Type", "Teacher" }
            };

            Dictionary<string, string> Student = new Dictionary<string, string>
            {
                { "User_Type", "Student" }
            };


            Dictionary<string, string> Infiniti = new Dictionary<string, string>
            {
                { "Enabled", "true" },
                {"Password","cairnsshs2005" }
            };


            this._ListOfQueries = new List<IQueries>()
            {
                new ClickView(Staff,QueryBank.ClickView_Staff),
                new ClickView(Student,QueryBank.ClickView_Students),
                new Infiniti(Infiniti,QueryBank.Infiniti)
            };

        }

        public void PreLoadData()
        {
            System.Diagnostics.Debug.WriteLine("Generating Query Payloads");
            List<Task> tasks = new List<Task>();

            foreach (IQueries query in this._ListOfQueries)
            {
                tasks.Add(Task.Factory.StartNew(() => query.LoadData()));
            }

            Task.WaitAll(tasks.ToArray());
            System.Diagnostics.Debug.WriteLine("All Query Payloads Generated");

        }

        internal List<object> GetQueryData(QueryBank id)
        {
            IQueries data = (from quer in this._ListOfQueries
                             where quer.GetQueryID() == id
                             select quer).FirstOrDefault();
            return data.GetQueryData();
        }

    }
}
