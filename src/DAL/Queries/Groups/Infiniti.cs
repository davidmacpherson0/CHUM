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
    public class Infiniti : IQueries
    {
        private List<string> _AllowedParamaterKeys;
        private QueryBank _ID;
        private Dictionary<string, string> _Paramaters;
        private List<object> _Data;


        public Infiniti(Dictionary<string, string> Paramaters, QueryBank id)
        {
            this._ID = id;
            this._AllowedParamaterKeys = new List<string>()
            {
                "Password",
                "Enable"
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
                string DefaultPassword = this._Paramaters["Password"];
                string Enabled = this._Paramaters["Enabled"];

                returnvalue = (from usr in Users.Read()
                               where usr.Exit_Date == null
                                  && usr.UserName != null
                               select new
                               {
                                   username = usr.UserName,
                                   password = (usr.User_Type_ID == 2 ? "Staff" : "Student"),
                                   primary_email_password = usr.UserName + "@eq.edu.au",
                                   given_name = usr.Preferred_First_Name,
                                   surname = usr.Preferred_Last_Name,
                                   gender = usr.Sex,
                                   library_barcode = usr.Barcode,
                                   admin_system_id = usr.Barcode,
                                   graduation_yeat = "",
                                   form_class = "",
                                   enabled = "Y",
                                   staff_member = (usr.User_Type_ID == 2 ? "true" : "false"),

                               }).ToList<object>();




                //ew
                //{

                //}).ToList<object>();
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
