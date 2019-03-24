using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Constants
{
    public class FileLocations
    {
        public const string ROOT_PATH = @".\";
        public const string CONFIG_FOLDER_PATH = ROOT_PATH + @"Config\";
        public const string DBKEYS_FOLDER_PATH = CONFIG_FOLDER_PATH + @"DBKeys\";
        public const string MAPS_FOLDER_PATH = CONFIG_FOLDER_PATH + @"Maps\";

        public const string IMPORT_MAPS_FOLDER = MAPS_FOLDER_PATH + @"Import\";
        public const string EXPORT_MAPS_FOLDER = MAPS_FOLDER_PATH + @"Export\";

        public const string UPDATE_MAPS_FOLDER = MAPS_FOLDER_PATH + @"Update\";
        public const string IMPORT_FILES_FOLDER = ROOT_PATH + @"Import\";
        public const string EXPORT_FILES_FOLDER = ROOT_PATH + @"Export\";

    }
}
