namespace CM
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Forms;
    using System.Text.Json;
    using Microsoft.Win32.SafeHandles;
    using System.Runtime.InteropServices;

    class SettingsManager : IDisposable
    {

        public static SettingsManager TDSettings { get; set; }

        public AppSettingsMeta JsonObjSettings { get; set; }

        private string SettingsFile { get; set; }

        public static bool DebugMode { get; set; }

        public SettingsManager()
        {
            this.SettingsFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppSettingsDefault.ApplicationName, AppSettingsDefault.SettingsFolder, AppSettingsDefault.ConfigFile);    //...\appdata\roaming\<application>\settings\...  
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
            /// <summary>
            /// Gets or sets the X (left position) of the form.
            /// </summary>
            public int FrmX { get; set; } = 20;

            /// <summary>
            /// Gets or sets the Y (top position) of the form.
            /// </summary>
            public int FrmY { get; set; } = 20;

            /// <summary>
            /// Gets or sets the height of the form.
            /// </summary>
            public int FrmHeight { get; set; }

            /// <summary>
            /// Gets or sets the width of the form.
            /// </summary>
            public int FrmWidth { get; set; }

            /// <summary>
            /// Gets or sets the windowstate of the form.
            /// </summary>
            public FormWindowState FrmWindowState { get; set; }
        }

        /// <summary>
        /// Load the application settings.
        /// </summary>
        public void LoadSettings()
        {
            if (File.Exists(this.SettingsFile))
            {
                if (DebugMode) { Logging.WriteToLogInformation("Ophalen settings."); }
                string json = File.ReadAllText(this.SettingsFile);
                this.JsonObjSettings = JsonSerializer.Deserialize<AppSettingsMeta>(json);
            }
            else
            {
                // Default values, the first time when there is no settings file jsonObjSettings = null
                if (this.JsonObjSettings != null)
                {
                    this.JsonObjSettings.AppParam[0].ActivateLogging = false;
                    this.JsonObjSettings.AppParam[0].AppendLogFile = false;
                    this.JsonObjSettings.AppParam[0].SettingsFileLocation = this.SettingsFile;
                    this.JsonObjSettings.AppParam[0].LogFileLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppSettingsDefault.ApplicationName, AppSettingsDefault.SettingsFolder);
                }
            }
        }

        /// <summary>
        /// Save settings file.
        /// </summary>
        /// <param name="jsonObjSettings">Object with the current settings.</param>
        public static void SaveSettings(dynamic jsonObjSettings)
        {
            if (jsonObjSettings != null)
            {
                // Get settings location
                string fileLocation = jsonObjSettings.AppParam[0].SettingsFileLocation;

                if (string.IsNullOrEmpty(fileLocation))
                {
                    // Defaul location
                    fileLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppSettingsDefault.ApplicationName, AppSettingsDefault.SettingsFolder, AppSettingsDefault.ConfigFile);
                }

                try
                {
                    if (DebugMode)
                    {
                        Logging.WriteToLogInformation("Opslaan settings.");
                    }

                    var options = new JsonSerializerOptions
                    {
                        WriteIndented = true,
                    };

                    string jsonString = JsonSerializer.Serialize(jsonObjSettings, options);

                    if (!string.IsNullOrEmpty(fileLocation) && !string.IsNullOrEmpty(jsonString))
                    {
                        File.WriteAllText(fileLocation, jsonString);
                    }
                }
                catch (AccessViolationException ex)
                {
                    Logging.WriteToLogError("Fout bij het opslaan van de settings.");
                    Logging.WriteToLogError(ex.Message);

                    // TODO debug mode and messagebox
                }
            }
        }

        /// <summary>
        /// Create the application settting file.
        /// </summary>
        public static void CreateSettingsFile()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            string settingsFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppSettingsDefault.ApplicationName, AppSettingsDefault.SettingsFolder, AppSettingsDefault.ConfigFile);

            if (!File.Exists(settingsFile))
            {
                Logging.WriteToLogInformation("Aanmaken settings bestand.");
                Logging.WriteToLogInformation("Locatie settings bestand : " + settingsFile);

                var appSettings = new AppSettingsMeta()
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
                            RateLimit = 1,
                            TrvFoundSearchColor = -10768897,
                            HideFromTaskbar = false,
                        },
                    },
                    FormMain = new List<FormMainParams>()
                    {
                        new FormMainParams()
                        {
                            FrmX = 200,  // Default = 200
                            FrmY = 100,
                            FrmHeight = 1150,
                            FrmWidth = 1750,
                            FrmWindowState = FormWindowState.Normal,
                        },
                    },
                    FormConfig = new List<FormConfigureParams>()
                    {
                        new FormConfigureParams()
                        {
                            FrmX = 20,
                            FrmY = 20,
                            FrmHeight = 880,
                            FrmWidth = 1240,
                            FrmWindowState = FormWindowState.Normal,
                        },
                    },
                };

                string jsonString;
                jsonString = JsonSerializer.Serialize(appSettings, options);

                File.WriteAllText(settingsFile, jsonString); // , Encoding.Unicode
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
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
                return;

            if (disposing)
            {
                this.handle.Dispose();
                // Free any other managed objects here.
                //
                //this.frm = null;
            }

            this.disposed = true;
        }
        #endregion Dispose

    }
}
