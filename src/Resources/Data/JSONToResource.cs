using Common.Constants;
using Common.Data;
using Common.Interfaces;
using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json;
using Resources.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resources.Data
{
    public class JSONToResource<T> : IResource
    {
        private IResourceManager _manager;
        private ResourceName _ResourceName;
        private string _JSONFolderPath;

        public JSONToResource(ResourceName resourceName, string FolderPath)
        {
            this._manager = ServiceLocator.Current.GetInstance<IResourceManager>();
            this._ResourceName = resourceName;
            this._JSONFolderPath = FolderPath;
        }

        public ResourceName getName()
        {
            return this._ResourceName;
        }

        public void InitialiseResource()
        {
            ResourcePayload payload = new ResourcePayload(this._ResourceName);

            DirectoryInfo DI = new DirectoryInfo(this._JSONFolderPath);


            IEnumerable<FileInfo> JSON_Files = DI.GetFiles().Where(i => i.Extension == ".JSON" || i.Extension == ".json");

            List<T> data = new List<T>();

            if (JSON_Files.Count() == 1)
            {
                if (JSON_Files.ElementAt(0).Name == "Settings.JSON")
                {
                    payload.Data = JsonConvert.DeserializeObject<T>(File.ReadAllText(JSON_Files.ElementAt(0).FullName));
                }
                else
                {
                    data.Add(JsonConvert.DeserializeObject<T>(File.ReadAllText(JSON_Files.ElementAt(0).FullName)));
                    payload.Data = data;
                }
            }
            else
            {
                foreach (FileInfo item in JSON_Files)
                {
                    data.Add(JsonConvert.DeserializeObject<T>(File.ReadAllText(item.FullName)));
                }

                payload.Data = data;
            }

            this._manager.SetResource(payload);
        }
    }
}
