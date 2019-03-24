using System;

namespace Common.Data.FileMapping
{
    public class Map
    {
        private string _TableFieldName;
        private string _FileFieldname;
        private object _DefaultData;
        private bool _IsIndex;

        public object DefaultData
        {
            get { return _DefaultData; }
            set { _DefaultData = value; }
        }

        public string TableFieldName
        {
            get { return _TableFieldName; }
            set { _TableFieldName = value; }
        }

        public string FileFieldname
        {
            get { return _FileFieldname; }
            set { _FileFieldname = value; }
        }

        public bool IsIndex
        {
            get { return _IsIndex; }
            set { _IsIndex = value; }
        }



    }
}