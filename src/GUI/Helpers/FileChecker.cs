using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Helpers
{
    public class FileChecker
    {
        public List<FileInfo> GetLastestFile(string Folder, string[] SearchName)
        {
            List<FileInfo> returnvalue = new List<FileInfo>();
            DirectoryInfo DI = new DirectoryInfo(Folder);


            foreach (string searchitem in SearchName)
            {
                returnvalue.Add((from fil in DI.EnumerateFiles()
                                 where fil.Name.Contains(searchitem)
                                 orderby fil.LastWriteTime descending
                                 select fil).FirstOrDefault());
            }


            //returnvalue = (from fil in DI.EnumerateFiles()
            //               from name in SearchName
            //               where fil.Name.Contains(name)
            //               orderby fil.LastWriteTime descending
            //               select fil).Take(3).ToList();
            return returnvalue;
        }

    }
}
