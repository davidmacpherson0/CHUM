using Common.Base;
using Common.Constants;
using Common.Data.FileMapping;
using Common.Data.FileMapping.Export;
using Common.Data.Logging;
using Common.Data.Options;
using Common.Events;
using Common.Interfaces;
using GUI.Helpers;
using Microsoft.Practices.ServiceLocation;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GUI.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private string _LocalImportPath;
        private string _RemoteImportPath;
        private string _ExportPath;
        private bool _isPrecheckEnabled;
        private bool _isAllChecked;
        private bool _isClickView;
        private bool _isInfiniti;
        private bool _isReadCloud;
        private bool _isRunEnabled;
        private bool _isFlushDB;
        private bool _isImport;
        private ObservableCollection<object> _ListofImportFiles;
        private ObservableCollection<object> _ListofMaps;
        private ObservableCollection<object> _ListofExportTemplates;
        private ILog _Logger;
        private Settings _Settings;
        private IImport _Importer;
        private IExport _Exporter;
        private IDBActions _DBActions;

        private IResourceManager _resourceManager;

        public MainViewModel()
        {
            this._Logger = ServiceLocator.Current.GetInstance<ILog>();
            this._resourceManager = ServiceLocator.Current.GetInstance<IResourceManager>();
            this._resourceManager.InitialiseResources(false);
            this._Settings = (Settings)this._resourceManager.GetResourcePayload(ResourceName.Mapping_Settings);
            if (this._Settings == null)
            {
                this._Settings = new Settings();
            }
            else
            {
                this._LocalImportPath = this._Settings.ListOfSettings.Where(i => i.Name == "Import_Folder").Select(i => i.Data).FirstOrDefault();
                this.RemoteImportPath = this._Settings.ListOfSettings.Where(i => i.Name == "Remote_Import_Folder").Select(i => i.Data).FirstOrDefault();
                this.ExportPath = this._Settings.ListOfSettings.Where(i => i.Name == "Export_Folder").Select(i => i.Data).FirstOrDefault();
            }

            this.isPrecheckEnabled = true;
            this.isAllChecked = false;
            this.isReadCloud = true;
            this._DBActions = ServiceLocator.Current.GetInstance<IDBActions>();

            this.isFlushDB = true;
            this.isImport = true;

            this._Importer = ServiceLocator.Current.GetInstance<IImport>();
            this._Exporter = ServiceLocator.Current.GetInstance<IExport>();

            if (this._RemoteImportPath != null) { this.PreChecks(); }
        }

        private void PreChecks()
        {
            try
            {
                this._Logger.LogMessage("Comencing Checks");
                CheckPaths();
                CheckJSONImportMaps();
                CheckJSONExportMaps();
                this._Logger.LogMessage("Checks all Complete");
                this.isRunEnabled = true;
                this.isPrecheckEnabled = true;

                this._resourceManager.UpdateSettings("Remote_Import_Folder", this.RemoteImportPath);
                this._resourceManager.UpdateSettings("Export_Folder", this.ExportPath);

                foreach (object Map in this._ListofMaps)
                {
                    this._Logger.LogMessage(Map.ToString());
                }
                if (Directory.Exists(this._LocalImportPath) == false)
                {
                    Directory.CreateDirectory(this._LocalImportPath);
                }
            }
            catch (Exception e)
            {
                this._Logger.LogMessage(e.Message);
            }
        }


        private void Run()
        {
            Task task = new Task(go);
            task.Start();

        }

        private void go()
        {
            //try
            //{
            CopyDownFiles();

            this._Logger.LogMessage("Flushing Export Folder");

            Directory.Delete(this.ExportPath, true);

            this._Logger.LogMessage("Flushing Export Folder");
            if (this.isFlushDB)
            {
                this._Logger.LogMessage("Flushing Database");
                this._DBActions.FlushDatabase();
            }
            if (this.isImport)
            {
                this._Logger.LogMessage("Starting Import");
                this._Importer.GoImport(this._LocalImportPath + "\\");
                this._Logger.LogMessage("Import Completed");

            }

            this._Logger.LogMessage("Starting Export");
            this._Exporter.RunExport();
            this._Logger.LogMessage("ExportComplete");
        }


        private void CopyDownFiles()
        {
            this._Logger.LogMessage("Copying Down Files");

            if (this._RemoteImportPath != this._LocalImportPath)
            {
                foreach (FileInfo file in this.ListofImportFiles)
                {
                    this._Logger.LogMessage("Copying down " + file.Name);
                    string filename = null;

                    filename = this._Settings.ListOfSettings.Where(i => file.Name.Contains(i.Data)).Select(i => i.Name.Replace("File_Search_", "")).FirstOrDefault() + ".csv";

                    if (filename == null) { throw new Exception("filename for local transfer cannot be null"); }

                    File.Copy(file.FullName, this._LocalImportPath + "\\" + filename, true);
                }
            }
        }

        private void CheckPaths()
        {
            this.ListofMaps = new ObservableCollection<object>((List<FileMap>)this._resourceManager.GetResourcePayload(ResourceName.Mapping_Import));

            this._Logger.LogMessage("Checking Paths Maps");
            if (string.IsNullOrWhiteSpace(this.RemoteImportPath) || string.IsNullOrWhiteSpace(this.ExportPath))
            {
                throw new Exception("Unable to Process paths are null or white space");
            }
            if (Directory.Exists(this.RemoteImportPath) == false)
            {
                throw new Exception("Import Path Doesn't Exist");
            }
            if (Directory.Exists(this.ExportPath) == false)
            {
                this._Logger.LogMessage("Export Path Doesn't Exist Creating");
                Directory.CreateDirectory(this.ExportPath);
            }
            if (Directory.EnumerateFiles(this.RemoteImportPath).Count() == 0)
            {
                throw new Exception("Import Path has no Files");
            }
            FileChecker FC = new FileChecker();
            this.ListofImportFiles = new ObservableCollection<object>(FC.GetLastestFile(this.RemoteImportPath, GetSearchStrings()));// new string[] { "student", "timetable", "class" }));

           
            this.ListofExportTemplates = new ObservableCollection<object>((List<ExportMap>)this._resourceManager.GetResourcePayload(ResourceName.Mapping_Export));

            if (this.ListofImportFiles.Count() < 3) { throw new Exception("Not engough Import files found"); }
        }

        private string[] GetSearchStrings()
        {

            return this._Settings.ListOfSettings.Where(i => i.Name.Contains("File_Search_")).Select(i => i.Data).ToArray<string>();
        }

        private bool CheckJSONExportMaps()
        {
            this._Logger.LogMessage("Checking Export Maps");
            return true;

        }

        private bool CheckJSONImportMaps()
        {
            this._Logger.LogMessage("Checking Import Maps");
            return true;
        }


        public ICommand btnPrechecks { get { return new DelegateCommand(PreChecks); } }
        public ICommand btnRun { get { return new DelegateCommand(Run); } }

        public string ExportPath
        {
            get { return _ExportPath; }
            set
            {
                _ExportPath = value;
                base.NotifyPropertyChanged("ExportPath");
            }
        }

        public string RemoteImportPath
        {
            get { return _RemoteImportPath; }
            set
            {
                _RemoteImportPath = value;
                base.NotifyPropertyChanged("RemoteImportPath");
            }
        }

        public bool isPrecheckEnabled
        {
            get { return _isPrecheckEnabled; }
            set
            {
                _isPrecheckEnabled = value;
                base.NotifyPropertyChanged("isPrecheckEnabled");
            }
        }

        public bool isRunEnabled
        {
            get { return _isRunEnabled; }
            set
            {
                _isRunEnabled = value;
                base.NotifyPropertyChanged("isRunEnabled");
            }
        }

        public bool isReadCloud
        {
            get { return _isReadCloud; }
            set
            {
                _isReadCloud = value;
                if (value == false) { this.isAllChecked = false; }
                base.NotifyPropertyChanged("isReadCloud");
            }
        }

        public bool isInfiniti
        {
            get { return _isInfiniti; }
            set
            {
                _isInfiniti = value;
                if (value == false) { this.isAllChecked = false; }
                base.NotifyPropertyChanged("isInfiniti");
            }
        }

        public bool isClickView
        {
            get { return _isClickView; }
            set
            {
                _isClickView = value;
                if (value == false) { this.isAllChecked = false; }
                base.NotifyPropertyChanged("isClickView");
            }
        }

        public bool isAllChecked
        {
            get { return _isAllChecked; }
            set
            {
                _isAllChecked = value;
                if (value)
                {
                    this.isClickView = true;
                    this.isInfiniti = true;
                    this.isReadCloud = true;
                }
                base.NotifyPropertyChanged("isAllChecked");
            }
        }

        public bool isImport
        {
            get { return _isImport; }
            set
            {
                _isImport = value;
                base.NotifyPropertyChanged("isImport");
            }
        }



        public bool isFlushDB
        {
            get { return _isFlushDB; }
            set
            {
                _isFlushDB = value;
                base.NotifyPropertyChanged("isFlushDB");
            }
        }





        public ObservableCollection<object> ListofExportTemplates
        {
            get { return _ListofExportTemplates; }
            set
            {
                _ListofExportTemplates = value;
                base.NotifyPropertyChanged("ListofExportTemplates");
            }
        }

        public ObservableCollection<object> ListofMaps
        {
            get { return _ListofMaps; }
            set
            {
                _ListofMaps = value;
                base.NotifyPropertyChanged("ListofMaps");

            }
        }

        public ObservableCollection<object> ListofImportFiles
        {
            get { return _ListofImportFiles; }
            set
            {
                _ListofImportFiles = value;
                base.NotifyPropertyChanged("ListofImportFiles");
            }
        }


    }
}