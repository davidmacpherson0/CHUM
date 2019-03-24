using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Data;
using Common.Constants;
using Common.Data.Options;
using Newtonsoft.Json;
using System.IO;

namespace Resources
{
    public class ResourceManager : IResourceManager
    {
        private List<ResourcePayload> _Resources;

        public ResourceManager()
        {
            this._Resources = new List<ResourcePayload>();
        }

        public List<string> GetListofResources()
        {
            return (from data in this._Resources
                    select data.Name.ToString()).ToList<string>();
        }

        public void UpdateSettings(string Name, string newValue)
        {
            ResourcePayload getPayload = (from res in this._Resources
                                          where res.Name == ResourceName.Mapping_Settings
                                          select res).FirstOrDefault();
            if (getPayload == null)
            {
                throw new Exception("Couldn't remove payload Named Mapping_Settings");
            }
            this._Resources.Remove(getPayload);


            Settings settings = (Settings)getPayload.Data;

            Setting setting = (from set in settings.ListOfSettings
                               where set.Name == Name
                               select set).FirstOrDefault();

            settings.ListOfSettings.Remove(setting);
            setting.Data = newValue;
            settings.ListOfSettings.Add(setting);

            File.WriteAllText(FileLocations.CONFIG_FOLDER_PATH + "\\Settings.JSON", JsonConvert.SerializeObject(settings, Formatting.Indented));

            this._Resources.Add(new ResourcePayload(ResourceName.Mapping_Settings)
            {
                Data = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(FileLocations.CONFIG_FOLDER_PATH + "\\Settings.JSON"))
            });
        }


        public object GetResourcePayload(ResourceName Name)
        {
            object payloadData = (from data in this._Resources
                                  where data.Name == Name
                                  select data.Data).FirstOrDefault();

            if (payloadData == null)
            {
                throw new Exception("Tried to retrive Resource: " + Name + " No Payload Found");
            }

            return payloadData;
        }

        public void InitialiseResources(bool MakeFiles)
        {
            InitaliseResources initresource = new InitaliseResources();
            initresource.InitialResources(MakeFiles);
        }

        public void SetResource(ResourcePayload Resource)
        {
            this._Resources.Add(Resource);
        }


    }
}
