using Common.Constants;
using Common.Data.FileMapping.Export;
using Common.Interfaces;
using DAL.CSV;
using DAL.Export.Data;
using DAL.Interfaces;
using DAL.Queries;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Export.Actions
{
    public class Export : IExport
    {

        

        public void RunExport()
        {

            IResourceManager manager = ServiceLocator.Current.GetInstance<IResourceManager>();
            List<ExportMap> ListOfExports = (List<ExportMap>)manager.GetResourcePayload(ResourceName.Mapping_Export);


            foreach (ExportMap map in ListOfExports.Where(i => i.ExportTemplate != null))
            {
                IExportTemplate Data = (IExportTemplate)Activator.CreateInstance(map.ExportTemplate);

                string Folderpath = FileLocations.EXPORT_FILES_FOLDER + map.Folder;

                if (Directory.Exists(Folderpath))
                {
                    Directory.Delete(Folderpath,true);
                }
                Directory.CreateDirectory(Folderpath);

                foreach (ExportFile exportFile in map.Files)
                {
                    ExportPayload payload = Data.ProcessFile(Folderpath, exportFile);
                    Writer writer = new Writer(payload);
                    writer.WriteFile();
                }
            }
        }





        //Query querybank = new Query();

        //querybank.PreLoadData();

        //    List<object> staff = querybank.GetQueryData(QueryBank.ClickView_Staff);
        //List<object> student = querybank.GetQueryData(QueryBank.ClickView_Students);
        //List<object> infiniti = querybank.GetQueryData(QueryBank.Infiniti);

        //System.Diagnostics.Debug.WriteLine("Staff Count: " + staff.Count());
        //    System.Diagnostics.Debug.WriteLine("Student Count: " + student.Count());
        //    System.Diagnostics.Debug.WriteLine("Infiniti Count: " + infiniti.Count());

        //}
    }
}
