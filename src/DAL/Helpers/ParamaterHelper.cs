using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Helpers
{
    public class ParamaterHelper
    {
        internal static void QueryParamterCheck(Dictionary<string, string> Paramters, List<string> AllowedKeys)
        {
            foreach (KeyValuePair<string, string> kp in Paramters)
            {
                if (AllowedKeys.Contains(kp.Key) == false)
                {
                    throw new Exception("Paramater Name can only be: " + Paramters.Keys.Select(i => i + "\r\n"));
                }

                if (kp.Key == null || string.IsNullOrWhiteSpace(kp.Key))
                {
                    throw new Exception("Paramater Name Cannot be null or empty");
                }

                if (kp.Value == null || string.IsNullOrWhiteSpace(kp.Value))
                {
                    throw new Exception(kp.Key + " Paramater Value Can not Be empty");
                }

            }
        }



    }
}
