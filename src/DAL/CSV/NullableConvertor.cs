using CsvHelper.TypeConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;

namespace DAL.CSV
{
    public class NullableConvertor  
    {
        //private Type _ConvertTo;

        //public NullableLongConvertor(Type convertTo)
        //{
        //    this._ConvertTo = convertTo;
        //}


        public static object ConvertFromString(object text)
        {
            long retur;
            try
            {
                if (long.TryParse(text.ToString(), out retur))
                {
                    return (long?)retur;
                }
            }
            catch (Exception)
            {
                throw new Exception("Can not convert Default value to type: " + typeof(long?));
            }
            return null;
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            throw new NotImplementedException();
        }

        //private object ChangeType(object value, Type ConvertTo)
        //{
        //    if (ConvertTo.IsGenericType && ConvertTo.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
        //    {
        //        if (value == null)
        //        {
        //            return null;
        //        }
        //        ConvertTo = Nullable.GetUnderlyingType(ConvertTo);
        //    }
        //    return Convert.ChangeType(value, ConvertTo);
        //}

    }

}

