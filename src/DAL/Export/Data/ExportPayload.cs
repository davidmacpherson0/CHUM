using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Export.Data
{
    public class ExportPayload
    {
        private string _FileName;
        private List<object> _Data;
        private bool _ShowHeader;
        private bool _QuoteField;

        public bool QuoteField
        {
            get { return _QuoteField; }
            set { _QuoteField = value; }
        }

        public bool ShowHeader
        {
            get { return _ShowHeader; }
            set { _ShowHeader = value; }
        }


        public List<object> Data
        {
            get { return _Data; }
            set { _Data = value; }
        }


        public string FilePath
        {
            get { return _FileName; }
            set { _FileName = value; }
        }



    }
}
