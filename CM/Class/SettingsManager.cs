using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Text.Json;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace CM
{
    class SettingsManager : IDisposable
    {

        public static SettingsManager TDSettings { get; set; }
        public AppSettingsMeta JsonObjSettings { get; set; }

        private string SettingsFile { get; set; }
        public bool DebugMode { get; set; }

        public SettingsManager()
        {
            SettingsFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppSettingsDefault.ApplicationName, AppSettingsDefault.SettingsFolder, AppSettingsDefault.ConfigFile);    //...\appdata\roaming\<application>\settings\...  
        }

        public class AppSettingsMeta
        {
            public List<AppParams> AppParam { get; set; }
            public List<FormMainParams> FormMain { get; set; }
            public List<FormConfigureParams> FormConfig { get; set; }            
        }

        public class AppParams
        {
            public bool ActivateLogging { get; set; }
            public bool AppendLogFile { get; set; }

            public string ApplicationName { get; set; }
            public string ApplicationVersion { get; set; }
            public string ApplicationBuildDate { get; set; }

            public string SettingsFileLocation { get; set; }
            public string LogFileLocation { get; set; }
            public string DatabaseLocation { get; set; }

            public int CopyAppDataBaseAfterEveryXStartups { get; set; }
            public int CopyAppDataBaseAfterEveryXStartupsCounter { get; set; }

            public string Url1 { get; set; }
            public string Url2 { get; set; }
            public double WarnPercentage { get; set; }
            public double RateLimit { get; set; }
            public int TrvFoundSearchColor { get; set; }
            public bool HideFromTaskbar { get; set; }
        }

        public class FormMainParams
        {
            //system.drawing.rectangle = 10; 10; 700; 500 ==> x, y, width, height
            public int FrmX { get; set; }
            public int FrmY { get; set; }
            public int FrmHeight { get; set; }
            public int FrmWidth { get; set; }
            public FormWindowState FrmWindowState { get; set; }
        }
        public class FormConfigureParams
        {
            public int FrmX { get; set; } = 20;
            public int FrmY { get; set; } = 20;
            public int FrmHeight { get; set; }
            public int FrmWidth { get; set; }
            public FormWindowState FrmWindowState { get; set; }
        }
        public void LoadSettings()
        {
            if (File.Exists(SettingsFile))
            {
                Logging.WriteToLogInformation("Ophalen settings.");
                string Json = File.ReadAllText(SettingsFile);
                JsonObjSettings = JsonSerializer.Deserialize<AppSettingsMeta>(Json);
            }
            else
            {   //Default values
                if (JsonObjSettings != null)  //the first time when there is no settings file jsonObjSettings = null
                {
                    JsonObjSettings.AppParam[0].ActivateLogging = false;
                    JsonObjSettings.AppParam[0].AppendLogFile = false;
                    JsonObjSettings.AppParam[0].SettingsFileLocation = SettingsFile;
                    JsonObjSettings.AppParam[0].LogFileLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppSettingsDefault.ApplicationName, AppSettingsDefault.SettingsFolder);
                }
            }
        }
        public static void SaveSettings(dynamic JsonObjSettings)
        {
            if (JsonObjSettings != null)
            {
                //get settings location
                string fileLocation = JsonObjSettings.AppParam[0].SettingsFileLocation;

                if (string.IsNullOrEmpty(fileLocation))
                {
                    //defaul location
                    fileLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppSettingsDefault.ApplicationName, AppSettingsDefault.SettingsFolder, AppSettingsDefault.ConfigFile);
                }

                try
                {
                    Logging.WriteToLogInformation("Opslaan settings.");
                    var options = new JsonSerializerOptions
                    {
                        WriteIndented = true
                    };

                    string jsonString = JsonSerializer.Serialize(JsonObjSettings, options);

                    if (!string.IsNullOrEmpty(fileLocation) && !string.IsNullOrEmpty(jsonString))
                    {
                        File.WriteAllText(fileLocation, jsonString);
                    }
                }
                catch (AccessViolationException ex)
                {
                    Logging.WriteToLogError("Fout bij het opslaan van de settings.");
                    Logging.WriteToLogError(ex.Message);
                    //TODO debug mode and messagebox
                }
            }
        }

        public static void CreateSettingsFile()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string SettingsFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppSettingsDefault.ApplicationName, AppSettingsDefault.SettingsFolder, AppSettingsDefault.ConfigFile);

            if (!File.Exists(SettingsFile))
            {
                Logging.WriteToLogInformation("Aanmaken settings bestand.");
                Logging.WriteToLogInformation("Locatie settings bestand : " + SettingsFile);

                var AppSettings = new AppSettingsMeta()
                {
                    AppParam = new List<AppParams>()
                    {
                        new AppParams()
                        {
                            ApplicationName = AppSettingsDefault.ApplicationName,
                            ApplicationVersion = AppSettingsDefault.ApplicationVersion,
                            ApplicationBuildDate = AppSettingsDefault.ApplicationBuildDate,
                            ActivateLogging = true,
                            AppendLogFile = true,
                            SettingsFileLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppSettingsDefault.ApplicationName, AppSettingsDefault.SettingsFolder, AppSettingsDefault.ConfigFile),
                            LogFileLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppSettingsDefault.ApplicationName, AppSettingsDefault.SettingsFolder),
                            DatabaseLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppSettingsDefault.ApplicationName, AppSettingsDefault.DatabaseFolder),
                            
                            CopyAppDataBaseAfterEveryXStartups = 25,
                            CopyAppDataBaseAfterEveryXStartupsCounter = 0,
                            Url1 = "https://api.bitvavo.com/v2/ticker/price",
                            Url2 = "https://api.bitvavo.com/v2/ticker/24h",
                            WarnPercentage = 1,
                            RateLimit = 1 ,
                            TrvFoundSearchColor = -10768897,
                            HideFromTaskbar = false
                        }
                    },
                    FormMain = new List<FormMainParams>()
                    {
                        new FormMainParams()
                        {
                            FrmX = 200,  //default = 200
                            FrmY = 100,
                            FrmHeight = 580,
                            FrmWidth = 815,
                            FrmWindowState = FormWindowState.Normal
                        }
                    },
                    FormConfig = new List<FormConfigureParams>()
                    {
                        new FormConfigureParams()
                        {
                            FrmX = 20,
                            FrmY = 20,
                            FrmHeight = 485,
                            FrmWidth = 800,
                            FrmWindowState = FormWindowState.Normal
                        }
                    }                    
                };

                string jsonString;
                jsonString = JsonSerializer.Serialize(AppSettings, options);

                File.WriteAllText(SettingsFile, jsonString); //, Encoding.Unicode
            }
        }

        #region Dispose
        // Flag: Has Dispose already been called?
        bool disposed;

        // Instantiate a SafeHandle instance.
        readonly SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                // Free any other managed objects here.
                //
                //this.frm = null;
            }

            disposed = true;
        }
        #endregion Dispose


    }
}
