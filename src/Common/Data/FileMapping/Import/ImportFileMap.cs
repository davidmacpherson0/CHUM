using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data.FileMapping
{
    public class FileMap
    {
        private string _FileName;
        private Type _TableType;
        private bool _IsUpdate;
        private Map[] _FieldMap;

        public bool IsUpdate
        {
            get { return _IsUpdate; }
            set { _IsUpdate = value; }
        }

        public Map[] FieldMap
        {
            get { return _FieldMap; }
            set { _FieldMap = value; }
        }

        public Type TableType
        {
            get { return _TableType; }
            set { _TableType = value; }
        }

        public string FileName
        {
            get { return _FileName; }
            set { _FileName = value; }
        }
    }
}
