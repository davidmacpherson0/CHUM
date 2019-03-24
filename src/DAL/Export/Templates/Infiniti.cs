using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Data.FileMapping.Export;
using DAL.Export.Data;
using DAL.Database.Model;

namespace DAL.Export.Templates
{
    public class Infiniti : IExportTemplate
    {
        public Infiniti()
        {

        }

        public ExportPayload ProcessFile(string Folder, ExportFile exportcontext)
        {


            ExportParamater UserType = (from type in exportcontext.Paramaters
                                        where type.Name == "User_Type"
                                        select type).FirstOrDefault();

            ExportParamater DefaultPassword = (from pas in exportcontext.Paramaters
                                               where pas.Name == "Default_Password"
                                               select pas).FirstOrDefault();



            if (UserType == null)
            {
                throw new Exception("Paramater <User_Type> must be set");
            }

            if (DefaultPassword == null)
            {
                throw new Exception("Paramater <Default_Password> must be set");
            }

            List<object> Data = null;
            Data = RunQuery(UserType, DefaultPassword);



            if (Data == null)
            {
                throw new Exception("Paramater Data set incorrectly\r\nUser_Type Data: " + UserType.Data);
            }

            return new ExportPayload()
            {
                FilePath = Folder + "\\" + exportcontext.FileName,
                ShowHeader = exportcontext.ShowHeader,
                Data = Data
            };
        }

        private List<object> RunQuery(ExportParamater UserType, ExportParamater DefaultPassword)
        {
            List<object> returnvalue = new List<object>();
            using (CHUMDB context = new CHUMDB())
            {
                DBGeneric<User> Users = new DBGeneric<User>(context);
                DBGeneric<User_Type> UserTypes = new DBGeneric<User_Type>(context);

                User_Type UT = (from ut in UserTypes.Read()
                                where ut.Label.ToString() == UserType.Data.ToString()
                                select ut).FirstOrDefault();

                if (UT == null)
                {
                    throw new Exception("Can't Find User Type Paramater in DB: " + UserType.Data.ToString());
                }

                if (UT.Label == "Student")
                {
                    returnvalue = (from usr in Users.Read()
                                   where usr.Exit_Date == null
                                      && usr.UserName != null
                                      && !string.IsNullOrWhiteSpace(usr.UserName)
                                      && usr.User_Type_ID == UT.ID
                                   orderby usr.User_Type_ID, usr.Year_Level
                                   select new
                                   {
                                       username = usr.UserName + "@eq.edu.au",
                                       password = DefaultPassword.Data.ToString(),
                                       primary_email = usr.UserName + "@eq.edu.au",
                                       given_name = usr.Preferred_First_Name,
                                       surname = usr.Preferred_Last_Name,
                                       library_barcode = usr.ID,
                                       admin_system_id = usr.ID,
                                       enabled = "Y",
                                       staff_member = (UT.Label == "Student" ? "N" : "Y"),
                                       gender = usr.Sex,
                                       form_class = usr.Form_Class ?? " ",
                                       graduation_year = CalcGradYear(usr.Year_Level),
                                   }).ToList<object>();
                }

                if (UT.Label == "Teacher")
                {
                    returnvalue = (from usr in Users.Read()
                                   where usr.Exit_Date == null
                                      && usr.UserName != null
                                      && !string.IsNullOrWhiteSpace(usr.UserName)
                                      && usr.User_Type_ID == UT.ID
                                   orderby usr.User_Type_ID, usr.Year_Level
                                   select new
                                   {
                                       username = usr.UserName + "@eq.edu.au",
                                       password = DefaultPassword.Data.ToString(),
                                       primary_email = usr.UserName + "@eq.edu.au",
                                       given_name = usr.Preferred_First_Name,
                                       surname = usr.Preferred_Last_Name,
                                       library_barcode = usr.Barcode,
                                       admin_system_id = usr.Barcode,
                                       enabled = "Y",
                                       staff_member = (UT.Label == "Student" ? "N" : "Y"),
                                   }).ToList<object>();
                }
            }

            return returnvalue;
        }

        private string CalcGradYear(string year_Level)
        {
            long data = 0;
            long.TryParse(year_Level, out data);

            switch (data)
            {
                case 7:
                    return (DateTime.Now.Year + 5).ToString();
                case 8:
                    return (DateTime.Now.Year + 4).ToString();
                case 9:
                    return (DateTime.Now.Year + 3).ToString();
                case 10:
                    return (DateTime.Now.Year + 2).ToString();
                case 11:
                    return (DateTime.Now.Year + 1).ToString();
                case 12:
                    return (DateTime.Now.Year + 0).ToString();
                default:
                    return DateTime.Now.Year.ToString();
            }
        }
    }
}
