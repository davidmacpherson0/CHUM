using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data.FileMapping.Export
{
    public class ExportParamater
    {
        private string _Name;

        private object _Data;

        public object Data
        {
            get { return _Data; }
            set { _Data = value; }
        }

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }


    }
}
