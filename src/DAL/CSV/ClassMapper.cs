using Common.Constants;
using Common.Data.FileMapping;
using Common.Interfaces;
using CsvHelper.Configuration;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAL.CSV
{
    public class ClassMapper : ClassMap<object>
    {
        public ClassMapper(FileMap fileMap)
        {
            foreach (Map item in fileMap.FieldMap)
            {
                MemberInfo MI = fileMap.TableType.GetMember(item.TableFieldName).FirstOrDefault();

                if (MI == null || item.TableFieldName == null)
                {
                    throw new Exception("JSON FILE Member can not be Null: " + item.TableFieldName);
                }


                Type data = (from ty in fileMap.TableType.GetProperties()
                             where ty.Name == MI.Name
                             select ty.PropertyType).FirstOrDefault();

                if (item.FileFieldname != null || item.DefaultData == null)
                {
                    base.Map(fileMap.TableType, MI).Name(item.FileFieldname);
                    //System.Diagnostics.Debug.WriteLine("\t" + MI.Name + " -> " + item.TableFieldName + " -> " + item.DefaultData);
                }
            }
        }


    }
}
