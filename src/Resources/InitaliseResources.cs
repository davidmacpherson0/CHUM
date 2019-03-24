using Common.Constants;
using Common.Data;
using Common.Data.FileMapping;
using Common.Data.FileMapping.Export;
using Common.Data.Options;
using Common.Interfaces;
using Microsoft.Practices.ServiceLocation;

using Resources.Data;
using Resources.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resources
{
    public class InitaliseResources
    {
        private Queue<IResource> _Resources;

        public void InitialResources(bool MakeFiles)
        {

            this._Resources = new Queue<IResource>();

            if (MakeFiles)
            {
                FolderFlush();
                //MakeJSONfiles();
            }
            //this._Resources.Enqueue(new FileMapping());

            this._Resources.Enqueue(new JSONToResource<FileMap>(ResourceName.Mapping_Import, FileLocations.IMPORT_MAPS_FOLDER));
            this._Resources.Enqueue(new JSONToResource<DBKeyData>(ResourceName.Mapping_DBSchema, FileLocations.DBKEYS_FOLDER_PATH));
            this._Resources.Enqueue(new JSONToResource<FileMap>(ResourceName.Mapping_Updates, FileLocations.UPDATE_MAPS_FOLDER));
            this._Resources.Enqueue(new JSONToResource<ExportMap>(ResourceName.Mapping_Export, FileLocations.EXPORT_MAPS_FOLDER));
            this._Resources.Enqueue(new JSONToResource<Settings>(ResourceName.Mapping_Settings, FileLocations.CONFIG_FOLDER_PATH));




            foreach (IResource resource in this._Resources)
            {
                System.Diagnostics.Debug.WriteLine("Resource Loaded: " + resource.getName());
                resource.InitialiseResource();
            }


        }

        private void FolderFlush()
        {
            CheckandDelete(FileLocations.CONFIG_FOLDER_PATH);
            CheckandDelete(FileLocations.MAPS_FOLDER_PATH);
            CheckandDelete(FileLocations.EXPORT_FILES_FOLDER);

            Directory.CreateDirectory(FileLocations.CONFIG_FOLDER_PATH);
            Directory.CreateDirectory(FileLocations.MAPS_FOLDER_PATH);
            Directory.CreateDirectory(FileLocations.IMPORT_MAPS_FOLDER);
            Directory.CreateDirectory(FileLocations.UPDATE_MAPS_FOLDER);
            Directory.CreateDirectory(FileLocations.DBKEYS_FOLDER_PATH);
            Directory.CreateDirectory(FileLocations.EXPORT_MAPS_FOLDER);
            Directory.CreateDirectory(FileLocations.EXPORT_FILES_FOLDER);
        }

        private void CheckandDelete(string FolderPath)
        {
            if (Directory.Exists(FolderPath))
            {
                Directory.Delete(FolderPath, true);
            }
        }
    }
}
