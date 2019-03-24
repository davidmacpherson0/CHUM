using DAL.Export.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    internal interface IExportFile
    {
        ExportPayload CreatePayload();
        void SetPayload(ExportPayload Payload);

        void PayloadToFile();
    }
}
