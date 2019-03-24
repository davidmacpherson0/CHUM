using Common.Data.FileMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Helpers
{
    public class PropertyHelper
    {
        public static IEnumerable<PropertyInfo> GetIsIndex(Type type, FileMap map)
        {
            IEnumerable<PropertyInfo> returndata = from prop in type.GetProperties()
                                                   from key in map.FieldMap
                                                   where key.TableFieldName == prop.Name && key.IsIndex == true
                                                   select prop;

            return returndata;



        }
        public static IEnumerable<PropertyInfo> GetPropertiesToChange(Type type, FileMap map)
        {
            return from prop in type.GetProperties()
                   from key in map.FieldMap
                   where prop.Name == key.TableFieldName && key.IsIndex == false
                   select prop;
        }




    }
}
