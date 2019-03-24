using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ImportActions
{
    public class Comparer : IEqualityComparer<object>
    {
        private Type _type;
        private List<string> _KeyNames;
        private IEnumerable<PropertyInfo> _Props;

        public Comparer(Type type, List<string> Keynames)
        {
            this._type = type;
            this._KeyNames = Keynames;
            this._Props = from prop in this._type.GetProperties()
                          where _KeyNames.Contains(prop.Name)
                          select prop;

        }

        public new bool Equals(object x, object y)
        {
            bool returnvalue = false;
            foreach (PropertyInfo prop in this._Props)
            {
                
                returnvalue = prop.GetValue(x).Equals(prop.GetValue(y));
            }

            return returnvalue;
        }

        public int GetHashCode(object obj)
        {
            int returnval = 0;
            foreach (PropertyInfo prop in this._Props)
            {
                returnval = returnval ^ prop.GetValue(obj).GetHashCode();
            }
            return returnval;
        }
    }
}
