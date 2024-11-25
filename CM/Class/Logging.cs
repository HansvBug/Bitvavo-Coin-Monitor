namespace CM
{
    using System;
    using System.IO;
    using System.Windows.Forms;
    using System.Globalization;
    using System.Reflection;
    using System.Management;


    public static class Logging
    {
        #region Properties

        public static Form Parent { get; set; }
        public static string NameLogFile { get; set; }
        public static bool AppendLogFile { get; set; } = true;      // Append the logfile true is the default.
        public static string LogFolder { get; set; }
        public static bool ActivateLogging { get; set; } = true;    // Activate logging.
        public static bool WriteToFile { get; set; }                // If logging fails then this can be set to false and de application will work without logging.
        public static string Customer { get; set; }
        public static string ApplicationName { get; set; }
        public static string ApplicationVersion { get; set; }
        public static string ApplicationBuildDate { get; set; }
        public static bool DebugMode { get; set; }

        private static string UserName { get; set; }
        private static string MachineName { get; set; }
        private static string TotalRam { get; set; }

        private static string WindowsVersion { get; set; }
        private static string ProcessorCount { get; set; }

        private static short LoggingEfforts { get; set; }
        private static string CurrentCultureInfo { get; set; }
        private static bool AbortLogging { get; set; }
        #endregion Properties

        #region constructor

        static Logging()
        {
            // Default
        }
        #endregion constructor

        #region Settings
        private static void GetAppEnvironmentSettings()
        {
            using AppEnvironment AppEnv = new AppEnvironment();
            UserName = AppEnv.UserName;
            MachineName = AppEnv.MachineName;
            WindowsVersion = AppEnv.WindowsVersion;
            ProcessorCount = AppEnv.ProcessorCount;
            TotalRam = AppEnv.TotalRam;
        }

        #region DefaultSettings
        private static void SetDefaultSettings()
        {
            CurrentCultureInfo = GetCultureInfo();

            LoggingEfforts = 0;
            WriteToFile = true;

            SetDefaultLogFileName();
            SetDefaultLogFileFolder();
        }

        private static void SetDefaultLogFileName()
        {
            if (string.IsNullOrEmpty(NameLogFile) || string.IsNullOrWhiteSpace(NameLogFile))
            {
                NameLogFile = "LogFile.log";
            }
        }

        private static void SetDefaultLogFileFolder()
        {
            //When there is no path for the log file, the file will be placed in the application folder
            if (string.IsNullOrEmpty(LogFolder) || string.IsNullOrWhiteSpace(LogFolder))
            {
                using AppEnvironment Folder = new AppEnvironment();
                LogFolder = Folder.ApplicationPath;
            }
        }

        private static string GetCultureInfo()
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture;
            return culture.ToString();
        }
        #endregion #region DefaultSettings
        #endregion Settings

        #region Check if there is a settings folder
        private static void CheckSettingsFolder()
        {
            if (CheckExistsSettingsFolder())
            {
                MessageBox.Show("De map 'Settings' is aangemaakt.", "Informatie.",
                                 MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private static bool CheckExistsSettingsFolder() // Check if the Settings folder does exists, if no then create the folder.
        {
            try
            {
                if (!Directory.Exists(LogFolder))
                {
                    Directory.CreateDirectory(LogFolder);  // Create the settings folder.
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (UnauthorizedAccessException ue)
            {
                AbortLogging = true;
                MessageBox.Show("Fout opgetreden bij het bepalen van de 'settings map'. " + Environment.NewLine +
                                "Fout: " + ue.Message + Environment.NewLine + Environment.NewLine +
                                "Locatie: " + LogFolder
                                , "Controleer of u rechten heeft om een map aan te maken op de locatie",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            catch (PathTooLongException pe)
            {
                AbortLogging = true;
                MessageBox.Show("Fout opgetreden bij het bepalen van de 'settings map'. " + Environment.NewLine +
                                "Fout: " + pe.Message + Environment.NewLine + Environment.NewLine +
                                "Pad: " + LogFolder
                                , "Het opgegeven pad bevat meer tekens dan is toegestaan",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout opgetreden bij het bepalen van de 'settings map'. " + Environment.NewLine +
                                "Fout: " + ex.Message + Environment.NewLine + Environment.NewLine +
                                "Pad: " + LogFolder
                                , "Het opgegeven pad bevat meer tekens dan is toegestaan",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                AbortLogging = true;
                throw;
            }
        }

        #endregion Check if there is a settings folder

        #region methods
        private static void DoesLogFileExists()
        {
            //First check if the logfile excists
            try
            {
                if (string.IsNullOrEmpty(UserName) || string.IsNullOrWhiteSpace(UserName))
                {
                    if (!File.Exists(LogFolder + NameLogFile))
                    {
                        File.Create(LogFolder + NameLogFile).Close();
                    }
                }
                else
                {
                    if (!File.Exists(LogFolder + UserName + "_" + NameLogFile))
                    {
                        File.Create(LogFolder + UserName + "_" + NameLogFile).Close();
                    }
                }
            }
            catch (IOException e)
            {
                AbortLogging = true;
                MessageBox.Show("Fout bij het controleren of het log bestand al bestaat." + Environment.NewLine
                                + Environment.NewLine +
                                "Fout: " + e.Message
                                , "Fout.",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private static void ClearLogfile()
        {
            if (!AppendLogFile)
            {
                try
                {   //Check if the logfile exists and then remove it.
                    if (File.Exists(LogFolder + UserName + "_" + NameLogFile))
                    {
                        File.Delete(LogFolder + UserName + "_" + NameLogFile);  //remove the logfile
                    }
                }
                catch (Exception e)
                {
                    AbortLogging = true;
                    MessageBox.Show("Fout bij het verwijderen van het log bestand." + Environment.NewLine
                                    + Environment.NewLine +
                                    "Fout: " + e.Message
                                    , "Fout.",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                    throw;
                }
            }
        }
        #endregion methods

        #region Handle the log file
        public static void WriteToLogInformation(string logMessage)
        {
            if (ActivateLogging)
            {
                WriteToLog("INFORMATIE", logMessage);
            }
        }

        public static void WriteToLogError(string logMessage)
        {
            if (ActivateLogging)
            {
                WriteToLog("FOUT", logMessage);
            }
        }

        public static void WriteToLogWarning(string logMessage)
        {
            if (ActivateLogging)
            {
                WriteToLog("WAARSCHUWING", logMessage);
            }
        }

        public static void WriteToLogDebug(string logMessage)  //Nog niet in gebruik
        {
            if (ActivateLogging)
            {
                WriteToLog("DEBUG", logMessage);
            }
        }
        #endregion Handle the log file


        public static bool StartLogging()
        {
            if (!AbortLogging)
            {
                if (ActivateLogging)
                {
                    SetDefaultSettings();
                    GetAppEnvironmentSettings();
                    CheckSettingsFolder();  // Is there a folder for the log file.

                    DoesLogFileExists();    // If there is no log file then it will be created.
                    ClearLogfile();         // If AppendLogFile = no then clear the (current) log file.

                    if (string.IsNullOrEmpty(UserName) || string.IsNullOrWhiteSpace(UserName))
                    {
                        using StreamWriter w = File.AppendText(LogFolder + NameLogFile);
                        LogStart(w);
                    }
                    else
                    {
                        using StreamWriter w = File.AppendText(LogFolder + UserName + "_" + NameLogFile);
                        LogStart(w);
                    }

                    if (!AbortLogging)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static void StopLogging()
        {
            if (ActivateLogging)
            {
                if (string.IsNullOrEmpty(UserName) || string.IsNullOrWhiteSpace(UserName))
                {
                    using StreamWriter w = File.AppendText(LogFolder + NameLogFile);
                    LogStop(w);
                }
                else
                {
                    using StreamWriter w = File.AppendText(LogFolder + UserName + "_" + NameLogFile);
                    LogStop(w);
                }
            }
        }

        private static void WriteToLog(string errorType, string logMessage)
        {
            if (WriteToFile)
            {
                string LogFile;
                if (string.IsNullOrEmpty(UserName) || string.IsNullOrWhiteSpace(UserName))
                {
                    LogFile = LogFolder + NameLogFile;
                }
                else
                {
                    LogFile = LogFolder + UserName + "_" + NameLogFile;
                }

                using StreamWriter w = File.AppendText(LogFile);

                // If the logfile gets to big then a new log fil will be made
                if (!CheckBestandsgrootte())
                {
                    LogRegulier(errorType, logMessage, w);
                }
                else
                {
                    LogRegulier("INFORMATIE", string.Empty, w);
                    LogRegulier("INFORMATIE", "Logbestand wordt groter dan 10 Mb.", w);
                    LogRegulier("INFORMATIE", "Een nieuw log bestand wordt aangemaakt.", w);
                    LogRegulier("INFORMATIE", string.Empty, w);
                    LogStop(w);

                    CopyLogFile();    // Make a copy off the logfile
                    EmptyLogFile();   // Clear the current logfile
                }
            }
            else
            {
                // hier kan een eventlog worden gezet.
            }
        }

        private static bool CheckBestandsgrootte()
        {
            try
            {
                long lengthFile;
                if (string.IsNullOrEmpty(UserName) || string.IsNullOrWhiteSpace(UserName))
                {
                    lengthFile = new FileInfo(LogFolder + NameLogFile).Length;
                }
                else
                {
                    lengthFile = new FileInfo(LogFolder + UserName + "_" + NameLogFile).Length;
                }

                // (10mB)  //TODO add tot the configfform
                if (lengthFile > (10 * 1024 * 1024))
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                AbortLogging = true;

                MessageBox.Show(
                    "Bepalen grootte log bestand is mislukt." + Environment.NewLine +
                    Environment.NewLine +
                    "Fout" + ex.Message + Environment.NewLine +
                    "Logging wordt uitgeschakelt.",
                    "Informatie.", 
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);

                throw;
            }
        }

        private static void CopyLogFile()
        {
            string currentDateTime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            currentDateTime = currentDateTime.Replace(":", "_");

            string newFile;
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrWhiteSpace(UserName))
            {
                newFile = LogFolder + currentDateTime + NameLogFile;
                File.Copy(LogFolder + NameLogFile, newFile);
            }
            else
            {
                newFile = LogFolder + currentDateTime + "_" + UserName + "_" + NameLogFile;
                File.Copy(LogFolder + UserName + "_" + NameLogFile, newFile);
            }
        }

        private static void EmptyLogFile()
        {
            // Check if the log file does exists.
            if (File.Exists(LogFolder + UserName + "_" + NameLogFile))
            {
                File.Delete(LogFolder + UserName + "_" + NameLogFile); // Remove the file (and create an empty new file)

                StartLogging();
            }
        }

        private static void LogStart(TextWriter w)
        {
            try
            {
                string date = DateTime.Today.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                w.WriteLine("===================================================================================================");
                w.WriteLine("Applicatie        : " + ApplicationName);
                w.WriteLine("Versie            : " + ApplicationVersion);
                w.WriteLine("Datum Applicatie  : " + ApplicationBuildDate);
                w.WriteLine("Organisatie       : " + Customer);
                w.WriteLine(string.Empty);
                w.WriteLine("Datum             : " + date);
                w.WriteLine("Naam gebruiker    : " + UserName);
                w.WriteLine("Naam machine      : " + MachineName);
                if (DebugMode)
                {
                    w.WriteLine("Windows versie    : " + WindowsVersion);
                    w.WriteLine("Aantal processors : " + ProcessorCount);
                    w.WriteLine("Fysiek geheugen   : " + Convert.ToString(TotalRam, CultureInfo.InvariantCulture));
                    w.WriteLine("CultuurInfo       : " + CurrentCultureInfo);
                }

                w.WriteLine("===================================================================================================");
                w.WriteLine(string.Empty);


                // regel in Gebeurtenissen logboek wegschrijven
                // AddMessageToEventLog("De Applicatie is succesvol gestart", "INFORMATIE", 1000);
            }
            catch (IOException ioex)
            {
                AbortLogging = true;
                MessageBox.Show(
                    "Het starten van de logging is niet mogelijk." + Environment.NewLine + Environment.NewLine +
                    "Fout : " + ioex.Message,
                    "Fout.",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                WriteToFile = false; // stop the logging when start logging failed

                // string foutmelding = e.ToString();
                // TODO melden in event logbook
            }
            catch (Exception ex)
            {
                AbortLogging = true;
                MessageBox.Show(
                    "Het starten van de logging is niet mogelijk." + Environment.NewLine + Environment.NewLine +
                    "Fout : " + ex.Message,
                    "Fout.",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                WriteToFile = false; // stop the logging when start logging failed
                throw;
            }
        }

        private static void LogRegulier(string errorType, string logMessage, TextWriter w)
        {
            try
            {
                switch (errorType)
                {
                    case "INFORMATIE":
                        {
                            w.WriteLine(DateTime.Now + " | INFORMATIE   | " + logMessage);
                            break;
                        }

                    case "WAARSCHUWING":
                        {
                            w.WriteLine(DateTime.Now + " | WAARSCHUWING | " + logMessage);
                            break;
                        }

                    case "FOUT":
                        {
                            w.WriteLine(DateTime.Now + " | FOUT         | " + logMessage);

                            // TODO: hier moet de eventlogging komen
                            // AddMessageToEventLog("De Applicatie is succesvol gestart", "INFORMATIE", 1000);
                            break;
                        }

                    case "DEBUG":
                        {
                            w.WriteLine(DateTime.Now + " | DEBUG        | " + logMessage);
                            break;
                        }

                    default:
                        {
                            w.WriteLine(DateTime.Now + " | ONBEKEND     | " + logMessage);
                            break;
                        }
                }
            }
            catch (ArgumentException aex)
            {
                LoggingEfforts += 1;
                if (DebugMode) { WriteToLogDebug(aex.ToString()); }
                StoppenLogging();
            }
            catch (Exception)
            {
                AbortLogging = true;
                LoggingEfforts += 1;
                StoppenLogging();
                throw new InvalidOperationException("Onverwachte fout opgetreden bij het aanmaken van de log regel.");
            }
        }

        private static void StoppenLogging()
        {
            if (LoggingEfforts == 5)
            {
                WriteToFile = false; // stop the writting to the logfile

                // AddMessageToEventLog("De logging van '" + Form_Main.Applicatienaam + "' is gestopt omdat er reeds 5 pogingen zijn mislukt.", "INFORMATIE", 1000);
                WriteToLogError("De logging van '" + ApplicationName + "' is gestopt omdat er reeds 5 pogingen zijn mislukt.");

                MessageBox.Show("Schrijven naar het logbestand is mislukt." + Environment.NewLine +
                                "" + Environment.NewLine +
                                "Logging wordt uitgeschakelt.",
                                "Informatie.",
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private static void LogStop(TextWriter w)
        {
            string CloseLogString;
            if (string.IsNullOrEmpty(ApplicationName) || string.IsNullOrWhiteSpace(ApplicationName))
            {
                CloseLogString = "De applicatie is afgesloten.";
            }
            else
            {
                CloseLogString = ApplicationName + " is afgesloten.";
            }

            try
            {
                w.WriteLine("===================================================================================================");
                w.WriteLine(CloseLogString);
                w.WriteLine("===================================================================================================");
                w.WriteLine(string.Empty);
                w.Flush();
                w.Close();
            }
            catch (IOException ioex)
            {
                AbortLogging = true;
                MessageBox.Show("Het starten van de logging is niet mogelijk." + Environment.NewLine + Environment.NewLine +
                                "Fout : " + ioex.Message,
                                "Fout.",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (DebugMode) { WriteToLogDebug(ioex.ToString()); }
                //TODO melden in gebeurtenissenlogboek
            }
            catch (Exception)
            {
                AbortLogging = true;
                throw new InvalidOperationException("Onverwachte fout opgetreden bij het stoppen van de logging.");
            }
        }

        #region Writing to Windows event log in Windows NOT USED
        //Writing to Windows event log in Windows NOT USED
        /*
        private static void AddMessageToEventLog(string mess, string ErrorType, int Id)
        {
            EventLog elog = new EventLog("");

            if (!EventLog.SourceExists(Settings.ApplicationName))
            {
                EventLog.CreateEventSource(Settings.ApplicationName, "Application");
            }

            elog.Source = Settings.ApplicationName;
            elog.EnableRaisingEvents = true;

            EventLogEntryType entryType = EventLogEntryType.Error;

            switch (ErrorType)
            {
                case "FOUT":
                    {
                        entryType = EventLogEntryType.Error;
                        break;
                    }
                case "INFORMATIE":
                    {
                        entryType = EventLogEntryType.Information;
                        break;
                    }
                case "WAARSCHUWING":
                    {
                        entryType = EventLogEntryType.Warning;
                        break;
                    }
            }

            elog.WriteEntry(mess, entryType, Id);

            /*    catch (System.Security.SecurityException secError)
                {

                }
                catch (Exception eventError)
                {

                }*/
        /* }*/
        #endregion Writing to Windows event log in Windows NOT USED



        private class AppEnvironment : IDisposable
        {
            #region Properties
            public string ApplicationPath { get; set; }
            public string UserName { get; set; }
            public string MachineName { get; set; }
            public string WindowsVersion { get; set; }
            public string ProcessorCount { get; set; }
            public string TotalRam { get; set; }
            #endregion Properties

            #region Constructor
            public AppEnvironment()
            {
                this.SetProperties();
            }
            #endregion Constructor

            private void SetProperties()
            {
                this.ApplicationPath = Get_Applicatiepad();
                this.UserName = GetUserName();
                this.MachineName = GetMachineName();
                this.WindowsVersion = GetWindowsVersion(2);
                this.ProcessorCount = GetProcessorCount();
                this.TotalRam = GetTotalRam();
            }

            private static string Get_Applicatiepad() // Get the application path
            {
                try
                {
                    string appPath;
                    appPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
                    appPath += "\\";                                  // Add \to the path
                    return appPath.Replace("file:\\", string.Empty);  // Remove the text "file:\\" from the path
                }
                catch (ArgumentException aex)
                {
                    throw new InvalidOperationException(aex.Message);
                }
                catch (Exception)
                {
                    throw new InvalidOperationException("Ophalen locatie applicatie is mislukt.");
                }
            }

            private static string GetUserName()
            {
                try
                {
                    return Environment.UserName;
                }
                catch (Exception)
                {
                    throw new InvalidOperationException("Ophalen naam gebruiker is mislukt.");
                }
            }
            private static string GetMachineName()
            {
                try
                {
                    return Environment.MachineName;
                }
                catch (Exception)
                {
                    throw new InvalidOperationException("Ophalen naam machine is mislukt.");
                }
            }

            private static string GetWindowsVersion(short type)
            {
                try
                {
                    string osVersion = string.Empty;

                    switch (type)
                    {
                        case 1:
                            {
                                osVersion = Environment.OSVersion.ToString();
                                break;
                            }

                        case 2:
                            {
                                osVersion = Convert.ToString(Environment.OSVersion.Version, CultureInfo.InvariantCulture);
                                break;
                            }

                        default:
                            {
                                osVersion = Convert.ToString(Environment.OSVersion.Version, CultureInfo.InvariantCulture);
                                break;
                            }
                    }

                    return osVersion;
                }
                catch (ArgumentException)
                {
                    throw new InvalidOperationException("Onverwachte fout opgetreden bij het bepalen van de Windowsversie(Argument Exception).");
                }
                catch (Exception)
                {
                    throw new InvalidOperationException("Onverwachte fout opgetreden bij het bepalen van de Windowsversie.");
                }
            }  //Test de verschillen !!!!

            private static string GetProcessorCount()
            {
                try
                {
                    return Convert.ToString(Environment.ProcessorCount, CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    throw new InvalidOperationException("Ophalen aantal processors is mislukt.");
                }
            }

            private static string GetTotalRam()
            {
                try
                {
                    using ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                    ManagementObjectCollection moc = mc.GetInstances();

                    ObjectQuery wql = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher(wql);
                    ManagementObjectCollection results = searcher.Get();

                    string TotalVisibleMemorySize = "Geen Totaal telling Ram gevonden.";

                    // string FreePhysicalMemory;
                    // string TotalVirtualMemorySize;
                    // string FreeVirtualMemory;
                    foreach (ManagementObject result in results)
                    {
                        TotalVisibleMemorySize = Convert.ToString(Math.Round(Convert.ToDouble(result["TotalVisibleMemorySize"], CultureInfo.InvariantCulture) / 1000000, 2), CultureInfo.InvariantCulture) + " GB";

                        // FreePhysicalMemory = result["FreePhysicalMemory"].ToString();
                        // TotalVirtualMemorySize = result["TotalVirtualMemorySize"].ToString();
                        // FreeVirtualMemory = result["FreeVirtualMemory"].ToString();
                    }

                    return TotalVisibleMemorySize;
                }
                catch (Exception)
                {
                    // throw new InvalidOperationException(ResourceEx.Get_TotalRam);
                    return "Geen Totaal telling Ram gevonden.";
                }

                /*try
                {
                    using (ManagementClass mc = new ManagementClass("Win32_ComputerSystem"))
                    {
                        ManagementObjectCollection moc = mc.GetInstances();

                        foreach (ManagementObject item in moc)
                        {
                            return Convert.ToString(Math.Round(Convert.ToDouble(item.Properties["TotalPhysicalMemory"].Value, CultureInfo.InvariantCulture) / 1073741824, 2), CultureInfo.InvariantCulture) + " GB";
                        }

                        return "Geen Totaal telling Ram gevonden.";
                    }
                }
                catch (Exception)
                {
                    //throw new InvalidOperationException(ResourceEx.Get_TotalRam);
                    return "Geen Totaal telling Ram gevonden.";
                }*/
            }

            #region IDisposable

            private bool disposed = false;

            //Implement IDisposable.
            public void Dispose()
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (!this.disposed)
                {
                    if (disposing)
                    {
                        // Free other state (managed objects).
                        this.ApplicationPath = string.Empty;
                        this.UserName = string.Empty;
                        this.MachineName = string.Empty;
                        this.WindowsVersion = string.Empty;
                        this.ProcessorCount = string.Empty;
                        this.TotalRam = string.Empty;
                    }

                    // Free your own state (unmanaged objects).
                    // Set large fields to null.
                    this.disposed = true;
                }
            }
            #endregion IDisposable
        }

    }
}
