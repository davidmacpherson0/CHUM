using Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data.FileMapping.Export
{
    public class ExportMap
    {
        private Type _ExportTemplate;
        private string _Folder;
        private List<ExportFile> _Files;

        public List<ExportFile> Files
        {
            get { return _Files; }
            set { _Files = value; }
        }

        public string Folder
        {
            get { return _Folder; }
            set { _Folder = value; }
        }

        public Type ExportTemplate
        {
            get { return _ExportTemplate; }
            set { _ExportTemplate = value; }
        }
    }
}
