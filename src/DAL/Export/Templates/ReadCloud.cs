using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Data.FileMapping.Export;
using DAL.Export.Data;
using DAL.Database.Model;
using Newtonsoft.Json.Linq;
using DAL.Helpers;
using Common.Constants;
using Newtonsoft.Json;
using System.Dynamic;

namespace DAL.Export.Templates
{
    public class ReadCloud : IExportTemplate
    {
        private CHUMDB _Context;
        private JObject _ModifyClassDetails;
        private string _NotByName;

        public ReadCloud()
        {
            this._Context = new CHUMDB();
        }

        public ExportPayload ProcessFile(string Folder, ExportFile exportFile)
        {
            //Get options form The json file

            string Institution = GetParamaterData(exportFile, "Institution").ToString();
            string TeacherPassword = GetParamaterData(exportFile, "TeacherPassword").ToString();
            string StudentPassword = GetParamaterData(exportFile, "StudentPassword").ToString();
            bool FilterStudents = GetParamaterData(exportFile, "FilterStudents") != null ? (bool)GetParamaterData(exportFile, "FilterStudents") : false;
            bool FilterClasses = GetParamaterData(exportFile, "FilterClasses") != null ? (bool)GetParamaterData(exportFile, "FilterClasses") : false; ;
            bool FilterTeachers = GetParamaterData(exportFile, "FilterTeachers") != null ? (bool)GetParamaterData(exportFile, "FilterTeachers") : false; ;


            bool Modifyclassesflag = GetParamaterData(exportFile, "ModifyClassCodes") != null ? true : false; ;

            bool NotThisClassName = GetParamaterData(exportFile, "NotThisClassName") != null ? true : false; ;


            if (Modifyclassesflag == true)
            {
                string json = GetParamaterData(exportFile, "ModifyClassCodes").ToString();
                this._ModifyClassDetails = JObject.Parse(json);
            }

            if (NotThisClassName == true)
            {
                this._NotByName = GetParamaterData(exportFile, "NotThisClassName").ToString();
            }
            if (string.IsNullOrWhiteSpace(Institution))
            {
                throw new Exception("Paramater <Instituion> Must Be Set");
            }

            List<object> Data = new List<object>();
            Data.Add(new { Title = "Institution,\"" + Institution + "\"" });

            /*---- Get Data ---*/
            List<User> VaildUsers = GenerateUsers(FilterTeachers, 2).ToList();
            VaildUsers.AddRange(GenerateUsers(FilterStudents, 1));

            var VaildClasses = GenerateClasses(FilterClasses);
            /*-----------------*/

            /*---- Format Data ---*/
            Data.AddRange(FormatData(VaildUsers));
            Data.AddRange(FormatData(VaildClasses));

            Data.AddRange(FormatMemberships(VaildUsers, VaildClasses));
            /*--------------------*/

            return new ExportPayload()
            {
                FilePath = Folder + "\\" + exportFile.FileName,
                ShowHeader = exportFile.ShowHeader,
                QuoteField = exportFile.QuoteNoFields,
                Data = Data
            };


        }

        private IEnumerable<User> GenerateUsers(bool ApplyFilter, int User_Type)
        {

            if (ApplyFilter)
            {
                return from user in this._Context.Users
                       from exp in this._Context.Filter_Users_Bridge
                       where user.ID == exp.Users_ID
                        && user.UserName != ""
                        && user.UserName != null
                        && user.UserName != " "
                        && (user.Enrolment_Status == "A"
                        || user.Enrolment_Status == "F"
                        || user.Enrolment_Status == null)
                       orderby user.UserName ascending
                       select user;
            }
            else
            {
                return from user in this._Context.Users
                       where user.User_Type_ID == User_Type
                        && user.UserName != ""
                        && user.UserName != null
                        && user.UserName != " "
                        && (user.Enrolment_Status == "A"
                        || user.Enrolment_Status == "F"
                        || user.Enrolment_Status == null)
                       orderby user.UserName ascending
                       select user;
            }
        }

        private IEnumerable<Class> GenerateClasses(bool ApplyFilter)
        {
            if (ApplyFilter)
            {
                return from clas in this._Context.Classes.Where(clas => clas.Name != this._NotByName)
                       from fil in this._Context.Filter_Classes_Bridge
                       where clas.Class_Code == fil.Classes_Class_Code
                       select clas;
            }
            else
            {
                return this._Context.Classes.Where(clas => clas.Name != this._NotByName);

            }
        }

        private IEnumerable<object> FormatData(IEnumerable<object> Data)
        {
            var datatype = Data.ElementAt(0);



            if (datatype.GetType() == typeof(User))
            {
                return from user in (IEnumerable<User>)Data
                       from UT in this._Context.User_Type
                       where user.User_Type_ID == UT.ID
                       select new
                       {
                           Title = "Person" + "," +
                                  "\"" + user.UserName.ToLower() + "@eq.edu.au" + "\"," +
                                  "\"" + UT.Label + "\"," +
                                  "\"" + user.Preferred_First_Name + "\"," +
                                  "\"" + user.Preferred_Last_Name + "\"" +
                                  (user.Year_Level == null ? string.Empty : ",\"Year " + user.Year_Level + "\"")
                       };
            }

            if (datatype.GetType() == typeof(Class))
            {

                if (this._ModifyClassDetails != null)
                {

                    string Type = (string)this._ModifyClassDetails.GetValue("Type");

                    if (Type == "Remove")
                    {
                        int Pos = (int)this._ModifyClassDetails.GetValue("Position");
                        int count = (int)this._ModifyClassDetails.GetValue("count");

                        return from clas in (IEnumerable<Class>)Data
                               select new
                               {
                                   Title = "Class" + "," +
                                            "\"" + clas.Class_Code.Remove(Pos, count) + "\"," +
                                            "\"" + clas.Name + "\""
                               };
                    }
                }
                else
                {
                    return from clas in (IEnumerable<Class>)Data
                           select new
                           {
                               Title = "Class" + "," +
                                        "\"" + clas.Class_Code + "\"," +
                                        "\"" + clas.Name + "\""
                           };
                }


            }

            return null;
        }

        private IEnumerable<object> FormatMemberships(List<User> vaildUsers, IEnumerable<Class> vaildClasses)
        {
            IEnumerable<object> returndata;


            IEnumerable<Classes_Users_Bridge> bridge = this._Context.Classes_Users_Bridge.ToList();

            returndata = from brig in bridge
                         join usr in vaildUsers on brig.Users_ID equals usr.ID
                         join cls in vaildClasses on brig.Classes_Class_Code equals cls.Class_Code
                         select new
                         {
                             Title = "Membership" + "," +
                                    "\"" + usr.UserName.ToString() + "@eq.edu.au" + "\"," +
                                    "\"" + brig.Classes_Class_Code.Remove(brig.Classes_Class_Code.Length - 2, 1) + "\""
                         };

            return returndata;
        }


        private object GetParamaterData(ExportFile exportFile, string paramaterName)
        {
            return (from para in exportFile.Paramaters
                    where para.Name == paramaterName
                    select para.Data).FirstOrDefault();
        }
    }
}