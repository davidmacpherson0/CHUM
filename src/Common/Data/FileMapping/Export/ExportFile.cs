using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data.FileMapping.Export
{
    public class ExportFile
    {
        private string _FileName;
        private bool _ShowHeader;
        private bool _QuoteNoFields;
        private List<ExportParamater> _Paramaters;

        public List<ExportParamater> Paramaters
        {
            get { return _Paramaters; }
            set { _Paramaters = value; }
        }

        public bool ShowHeader
        {
            get { return _ShowHeader; }
            set { _ShowHeader = value; }
        }

        public bool QuoteNoFields
        {
            get { return _QuoteNoFields; }
            set { _QuoteNoFields = value; }
        }

        public string FileName
        {
            get { return _FileName; }
            set { _FileName = value; }
        }



    }
}
