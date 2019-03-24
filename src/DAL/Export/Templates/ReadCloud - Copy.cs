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

namespace DAL.Export.Templates
{
    public class OLDReadCloud : IExportTemplate
    {
        private List<string> _ListofClasses;
        private CHUMDB _Context;

        public OLDReadCloud()
        {
            this._ListofClasses = new List<string>();
            this._Context = new CHUMDB();
        }

        public ExportPayload ProcessFile(string Folder, ExportFile exportFile)
        {
            ExportParamater Institution = (from para in exportFile.Paramaters
                                           where para.Name == "Institution"
                                           select para).FirstOrDefault();


            if (Institution == null)
            {
                throw new Exception("Paramater <Institution> must be set");
            }

            object TeacherFlagPara = (from para in exportFile.Paramaters
                                      where para.Name == "ExportAllTeachers"
                                      select para.Data).FirstOrDefault();


            object StudentFlagPara = (from para in exportFile.Paramaters
                                      where para.Name == "ExportAllStudents"
                                      select para.Data).FirstOrDefault();

            object ClassesFlagPara = (from para in exportFile.Paramaters
                                      where para.Name == "ExportAllClasses"
                                      select para.Data).FirstOrDefault();

            object TeacherPassword = (from para in exportFile.Paramaters
                                      where para.Name == "TeacherPassword"
                                      select para.Data).FirstOrDefault();

            object StudentPassword = (from para in exportFile.Paramaters
                                      where para.Name == "StudentPassword"
                                      select para.Data).FirstOrDefault();

            bool TeacherFlag = TeacherFlagPara != null ? (bool)TeacherFlagPara : true;
            bool StudentFlag = StudentFlagPara != null ? (bool)StudentFlagPara : true;
            bool ClassesFlag = ClassesFlagPara != null ? (bool)ClassesFlagPara : true;


            System.Diagnostics.Debug.WriteLine("Export all Teachers: " + TeacherFlag);
            System.Diagnostics.Debug.WriteLine("Export all Students: " + StudentFlag);
            System.Diagnostics.Debug.WriteLine("Export all Classes: " + ClassesFlag);
            System.Diagnostics.Debug.WriteLine("Number of Export Classes: " + this._ListofClasses.Count());
            System.Diagnostics.Debug.WriteLine("Exporting Classes:");

            if (ClassesFlag == false)
            {

                JArray array = (from para in exportFile.Paramaters
                                where para.Name == "Classes"
                                select (JArray)para.Data).FirstOrDefault();

                if (array == null)
                {
                    this._ListofClasses = new List<string>();
                }

                this._ListofClasses = new List<string>(array.Select(i => i.ToString()));


                foreach (string item in this._ListofClasses)
                {
                    System.Diagnostics.Debug.Write(item + ", ");
                }
            }

            List<object> Data = new List<object>();

            Data.AddRange(GenerateInstituionData(Institution.Data.ToString()));

            Data.AddRange(GenerateTeacherUsers(TeacherFlag, (string)TeacherPassword));
            Data.AddRange(GenerateStudentUsers(StudentFlag, (string)StudentPassword));
            Data.AddRange(GenerateClasses(ClassesFlag));

            if (TeacherFlag && StudentFlag && ClassesFlag)
            {
                Data.AddRange(GenerateMemberShips(true));
            }
            else
            {
                Data.AddRange(GenerateMemberShips(false));
            }

            Data = Convert(Data);
            System.Diagnostics.Debug.WriteLine("Total Lines: " + Data.Count());

            return new ExportPayload()
            {
                FilePath = Folder + "\\" + exportFile.FileName,
                ShowHeader = exportFile.ShowHeader,
                QuoteField = exportFile.QuoteNoFields,
                Data = Data
            };
        }

        private List<object> GenerateInstituionData(string Name)
        {
            return new List<object>()
            {
               new {Title = "Institution,\"" + Name + "\"" }
            };
        }

        private List<object> GenerateTeacherUsers(bool AllTeacherFlag, string TeacherPassword)
        {
            List<object> returnvale = new List<object>();

            if (AllTeacherFlag)
            {
                System.Diagnostics.Debug.WriteLine("All Teachers Selected");
                returnvale = (from data in this._Context.Users
                              where data.User_Type_ID == 2
                                   && data.UserName != ""
                                   && data.UserName != null
                                   && data.UserName != " "
                              orderby data.UserName ascending
                              select new
                              {
                                  Title = "Person" + "," +
                                  "\"" + data.UserName.ToLower() + "@eq.edu.au" + "\"," +
                                  "\"" + "Teacher" + "\"," +
                                  "\"" + data.Preferred_First_Name + "\"," +
                                  "\"" + data.Preferred_Last_Name + "\""
                              }).ToList<object>();
            }
            else
            {
                returnvale = (from usr in this._Context.Users
                              from CUB in this._Context.Classes_Users_Bridge
                              from cls in this._Context.Classes
                              from selcla in this._ListofClasses
                              where usr.User_Type_ID == 2
                                   && usr.ID == CUB.Users_ID
                                   && CUB.Classes_Class_Code == cls.Class_Code
                                   && cls.Class_Code.Contains(selcla)
                                   && usr.UserName != ""
                                   && usr.UserName != null
                                   && usr.UserName != " "

                              select new
                              {
                                  Title = "Person" + "," +
                                  "\"" + usr.UserName.ToLower() + "@eq.edu.au" + "\"," +
                                  "\"" + "Teacher" + "\"," +
                                  "\"" + usr.Preferred_First_Name + "\"," +
                                  "\"" + usr.Preferred_Last_Name + "\""
                              }).ToList<object>();
            }
            return returnvale.Distinct().ToList<object>();
        }



        private List<object> GenerateStudentUsers(bool AllStudentsFlag, string StudentPassword)
        {
            List<object> returndata = new List<object>();
            if (AllStudentsFlag)
            {
                System.Diagnostics.Debug.WriteLine("All Students Selected");
                returndata = (from data in this._Context.Users
                              where data.User_Type_ID == 1
                              && data.UserName != "" 
                              && data.UserName != null 
                              && data.UserName != " "
                              && (data.Enrolment_Status == "A" 
                              || data.Enrolment_Status == "F")
                              select new
                              {
                                  Title = "Person" + "," +
                                  "\"" + data.UserName + "@eq.edu.au" + "\"," +
                                  "\"" + "Student" + "\"," +
                                  "\"" + data.Preferred_First_Name + "\"," +
                                  "\"" + data.Preferred_Last_Name + "\"," +
                                  "\"" + "Year " + data.Year_Level + "\""
                              }).ToList<object>();
            }
            else
            {
                IEnumerable<dynamic> Data = GetData();
                returndata = (from usr in Data
                              where usr.User.User_Type_ID == 1
                                   && usr.User.UserName != ""
                                   && usr.User.UserName != null
                                   && usr.User.UserName != " "
                                   && (usr.User.Enrolment_Status == "A"
                                   || usr.User.Enrolment_Status == "F")
                              orderby usr.User.UserName ascending
                              select new
                              {
                                  Title = "Person" + "," +
                                  "\"" + usr.User.UserName + "@eq.edu.au" + "\"," +
                                  "\"" + "Student" + "\"," +
                                  "\"" + usr.User.Preferred_First_Name + "\"," +
                                  "\"" + usr.User.Preferred_Last_Name + "\"," +
                                  "\"" + "Year " + usr.User.Year_Level + "\""
                              }).ToList<object>();

            }

            return returndata.Distinct().ToList<object>();
        }



        private List<object> GenerateClasses(bool AllClassesFlag)
        {
            List<object> returnvalue = new List<object>();
            if (AllClassesFlag)
            {
                returnvalue = (from clas in this._Context.Classes
                               where clas.Name != "Roll Class"
                               select new
                               {
                                   Title = "Class" + "," +
                                    "\"" + clas.Class_Code.Remove(clas.Class_Code.Length - 2, 1) + "\"," +
                                    "\"" + clas.Name + "\""
                               }).ToList<object>();
            }
            else
            {
                returnvalue = (from data in GetData()
                               select new
                               {
                                   Title = "Class" + "," +
                                    "\"" + data.Class.Class_Code.Remove(data.Class.Class_Code.Length - 2, 1) + "\"," +
                                    "\"" + data.Class.Name + "\""
                               }).ToList<object>();
            }
            return returnvalue.Distinct().ToList<object>();
        }

        private List<object> GenerateMemberShips(bool AllClassesFlag)
        {
            List<object> returnvalue = new List<object>();
            if (AllClassesFlag)
            {
                returnvalue = (from usr in this._Context.Users
                               from CUB in this._Context.Classes_Users_Bridge
                               from cls in this._Context.Classes.Where(c => !c.Name.Contains("Roll"))
                               where usr.ID == CUB.Users_ID
                                    && CUB.Classes_Class_Code == cls.Class_Code
                                    && usr.UserName != ""
                                    && usr.UserName != null
                                    && usr.UserName != " "
                               select new
                               {
                                   Title = "Membership" + "," +
                                   "\"" + usr.UserName + "@eq.edu.au" + "\"," +
                                   "\"" + cls.Class_Code.Remove(cls.Class_Code.Length - 2, 1) + "\""
                               }).ToList<object>();
            }

            else
            {
                returnvalue = (from data in GetData()

                               select new
                               {
                                   Title = "Membership" + "," +
                                   "\"" + data.User.UserName + "@eq.edu.au" + "\"," +
                                   "\"" + data.Class.Class_Code.Remove(data.Class.Class_Code.Length - 2, 1) + "\""
                               }).ToList<object>();


            }
            System.Diagnostics.Debug.WriteLine("All Classes: " + AllClassesFlag);

            System.Diagnostics.Debug.WriteLine("Membership Count: " + returnvalue.Count());

            return returnvalue;

        }
        private IEnumerable<dynamic> GetData()
        {
            List<Classes_Users_Bridge> ListofClasses = new List<Classes_Users_Bridge>();
            foreach (string search in this._ListofClasses)
            {
                ListofClasses.AddRange(this._Context.Classes_Users_Bridge.Where(i => i.Classes_Class_Code.Contains(search)));
            }

            return from CUB in ListofClasses
                   join clas in this._Context.Classes.Where(i => !i.Name.Contains("Roll")) on CUB.Classes_Class_Code equals clas.Class_Code
                   join user in this._Context.Users on CUB.Users_ID equals user.ID
                   orderby CUB.Classes_Class_Code ascending
                   select new { Class = clas, ClassUserBridge = CUB, User = user };

        }

        private List<object> Convert(List<object> Data)
        {
            List<object> returnData = new List<object>();
            foreach (dynamic item in Data)
            {
                returnData.Add(new ReadcloudFile(item.Title));
            }
            return returnData;
        }
    }
}