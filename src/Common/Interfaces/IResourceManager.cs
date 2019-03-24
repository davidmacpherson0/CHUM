using Common.Constants;
using Common.Data;
using Common.Data.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IResourceManager
    {
        void InitialiseResources(bool MakeFiles);

        void SetResource(ResourcePayload Resource);
        object GetResourcePayload(ResourceName Name);
        List<string> GetListofResources();
        void UpdateSettings(string Name, string newValue);



    }
}
