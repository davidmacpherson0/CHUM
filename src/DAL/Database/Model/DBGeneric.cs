using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Database.Model
{
    public class DBGeneric<T> where T : class
    {
        private CHUMDB _Context;


        public DBGeneric(CHUMDB context)
        {
            this._Context = context;
        }

        public List<T> Read()
        {
            System.Diagnostics.Debug.WriteLine(this._Context.Set<T>().GetType());

            return this._Context.Set<T>().ToList<T>();
        }








    }
}
