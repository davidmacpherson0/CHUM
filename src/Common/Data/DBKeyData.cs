using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data
{
    public class DBKeyData
    {
        private List<string> _KeyNames;
        private Type _Table;


        public Type Table
        {
            get { return _Table; }
            set { _Table = value; }
        }

        public List<string> KeyNames
        {
            get { return _KeyNames; }
            set { _KeyNames = value; }
        }



    }
}
