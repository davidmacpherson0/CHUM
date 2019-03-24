using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Data.FileMapping.Export;
using DAL.Database.Model;
using DAL.CSV;
using DAL.Export.Data;
using Common.Constants;

namespace DAL.Export.Templates
{
    public class ClickView : IExportTemplate
    {
        private ExportFile _ExportContext;

        public ClickView()
        {

        }

        public ExportPayload ProcessFile(string Folder, ExportFile exportcontext)
        {
            this.ExportContext = exportcontext;
            List<object> Data = null;
            ExportParamater queryType = (from type in exportcontext.Paramaters
                                         where type.Name == "User_Type"
                                         select type).FirstOrDefault();

            if (queryType == null)
            {
                throw new Exception("Paramater User_Type must be set");

            }

            if (queryType.Data == null || string.IsNullOrWhiteSpace(queryType.Data.ToString()))
            {
                throw new Exception("User_Type Data set incorrectly\r\nData: " + queryType);
            }

            Data = RunQuery(queryType.Data.ToString());

            if (Data == null)
            {
                throw new Exception("User_Type Data set incorrectly\r\nData: " + queryType);
            }

            return new ExportPayload()
            {
                FilePath = Folder + "\\" + this._ExportContext.FileName,
                ShowHeader = exportcontext.ShowHeader,
                Data = Data
            };
        }

        private List<object> RunQuery(string data)
        {
            using (CHUMDB context = new CHUMDB())
            {
                DBGeneric<User> Users = new DBGeneric<User>(context);
                var test = Users.Read();

                List<object> returnvalue = null;

                User_Type UT = (from ut in context.User_Type
                                where ut.Label == data
                                select ut).FirstOrDefault();
                if (UT == null)
                {
                    throw new Exception("Can't Find User Type Paramater in DB: " + data);
                }

                if (UT.Label == "Teacher")
                {
                    returnvalue = (from usr in Users.Read()
                                   where usr.User_Type_ID == UT.ID
                                        && usr.UserName != null
                                        && !string.IsNullOrWhiteSpace(usr.UserName)
                                   orderby usr.User_Type_ID, usr.Year_Level
                                   select new
                                   {
                                       First_Name = usr.Preferred_First_Name,
                                       Surname = usr.Preferred_Last_Name,
                                       Email = usr.UserName + "@eq.edu.au"
                                   }).ToList<object>();
                }

                if (data == "Student")
                {
                    returnvalue = (from usr in Users.Read()
                                   where usr.User_Type_ID == UT.ID
                                        && usr.Exit_Date == null
                                        && usr.UserName != null
                                        && !string.IsNullOrWhiteSpace(usr.UserName)
                                   orderby usr.User_Type_ID, usr.Year_Level
                                   select new
                                   {
                                       First_Name = usr.Preferred_First_Name,
                                       Surname = usr.Preferred_Last_Name,
                                       Email = usr.UserName + "@eq.edu.au",
                                       YearGroup = "Year " + usr.Year_Level
                                   }).ToList<object>();
                }
                return returnvalue ?? new List<object>();

            }
        }

        public ExportFile ExportContext
        {
            get { return _ExportContext; }
            set { _ExportContext = value; }
        }



    }
}
