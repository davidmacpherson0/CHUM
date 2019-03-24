using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data.Options
{
    public class Settings
    {
        private List<Setting> _ListOfSettings;

        public List<Setting> ListOfSettings
        {
            get { return _ListOfSettings; }
            set { _ListOfSettings = value; }
        }


    }
}
