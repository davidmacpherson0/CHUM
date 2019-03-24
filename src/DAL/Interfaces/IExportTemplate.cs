using Common.Data.FileMapping.Export;
using DAL.Export.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    interface IExportTemplate
    {
        ExportPayload ProcessFile(string Folder, ExportFile exportFile);
    }
}
