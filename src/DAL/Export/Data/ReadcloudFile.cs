using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Export.Data
{
    public class ReadcloudFile
    {
        private string _Title;
        public ReadcloudFile()
        {

        }

        public ReadcloudFile(string title)
        {
            this.Title = title;
        }


        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

    }
}
