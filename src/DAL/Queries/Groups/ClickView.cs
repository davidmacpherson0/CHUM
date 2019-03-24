using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Constants;
using DAL.Database.Model;
using DAL.Helpers;

namespace DAL.Queries.Groups
{
    public class ClickView : IQueries
    {
        private List<string> _AllowedParamaterKeys;
        private QueryBank _ID;
        private Dictionary<string, string> _Paramaters;
        private List<object> _Data;


        public ClickView(Dictionary<string, string> Paramaters, QueryBank id)
        {
            this._ID = id;
            this._AllowedParamaterKeys = new List<string>()
            {
                "User_Type"
            };
            _Paramaters = Paramaters;
        }

        public List<object> GetQueryData()
        {
            return this._Data;
        }

        public void LoadData()
        {
            using (CHUMDB context = new CHUMDB())
            {
                DBGeneric<User> Users = new DBGeneric<User>(context);

                List<object> returnvalue = null;
                string paramavalue = this._Paramaters["User_Type"];

                User_Type UT = (from ut in context.User_Type
                                where ut.Label == paramavalue
                                select ut).FirstOrDefault();


                if (paramavalue == "Teacher")
                {
                    returnvalue = (from usr in Users.Read()
                                   where usr.User_Type_ID == UT.ID 
                                        && usr.Exit_Date == null
                                        && usr.UserName != null
                                   select new
                                   {
                                       First_Name = usr.Preferred_First_Name,
                                       Last_Name = usr.Preferred_Last_Name,
                                       Email = usr.UserName + "@eq.edu.au"
                                   }).ToList<object>();
                }

                if (paramavalue == "Student")
                {
                    returnvalue = (from usr in Users.Read()
                                   where usr.User_Type_ID == UT.ID 
                                        && usr.Exit_Date == null
                                        && usr.UserName != null
                                   select new
                                   {
                                       First_Name = usr.Preferred_First_Name,
                                       Last_Name = usr.Preferred_Last_Name,
                                       Email = usr.UserName + "@eq.edu.au",
                                       YearGroup = usr.Year_Level
                                   }).ToList<object>();
                }

                this._Data = returnvalue ?? new List<object>(); ;
            }
        }

        public void CheckParamaters()
        {
            if (this._Paramaters != null)
            {
                ParamaterHelper.QueryParamterCheck(this._Paramaters, this._AllowedParamaterKeys);
            }
        }

        public QueryBank GetQueryID()
        {
            return this._ID;
        }


    }
}
