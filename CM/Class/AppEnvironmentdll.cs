namespace CM
{
    using System;
    using System.IO;            // Used by Path.
    using System.Reflection;    // Used by Assembly.
    using System.Management;    // Add reference System.Management
    using System.Windows.Forms;
    using Microsoft.Win32;
    using System.Globalization;
    using System.Collections.Generic;

    /// <summary>
    /// Application environment.
    /// </summary>
    public class AppEnvironment : IDisposable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the application path.
        /// </summary>
        public string ApplicationPath { get; set; }

        /// <summary>
        /// geets or sets the user name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the machine name.
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// Gets or sets the windows version.
        /// </summary>
        public string WindowsVersion { get; set; }

        /// <summary>
        /// Gets or sets the nummer of processors.
        /// </summary>
        public string ProcessorCount { get; set; }

        /// <summary>
        /// Gets or sets the processor id.
        /// </summary>
        public string ProcessorId { get; set; }

        /// <summary>
        /// Gets or sets the BIOS id.
        /// </summary>
        public string BiosId { get; set; }

        /// <summary>
        /// Gets or sets the total maount of RAM.
        /// </summary>
        public string TotalRam { get; set; }

        /// <summary>
        /// Gets or sets the monitor width.
        /// </summary>
        public int MonitorWidth { get; set; }

        /// <summary>
        /// Gets or sets the number of monitors.
        /// </summary>
        public int MonitorCount { get; set; }

        /// <summary>
        /// Gets or sets the .net framework version.
        /// </summary>
        public List<string> DotNetFrameWorkVersion { get; set; }
        #endregion Properties

        #region Constructor
        public AppEnvironment()
        {
            this.SetProperties();
        }
        #endregion Constructor

        #region Methods

        /// <summary>
        /// Create a new foldeer.
        /// </summary>
        /// <param name="folderName">The name of the new folder.</param>
        /// <param name="applicationDataFolder">the application data folder.</param>
        /// <returns>True if succeeded.</returns>
        public bool CreateFolder(string folderName, bool applicationDataFolder)
        {
            if (string.IsNullOrEmpty(folderName))
            {
                return false;
            }

            if (applicationDataFolder)
            {
                string pathString = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), folderName);

                if (!Directory.Exists(pathString))
                {
                    try
                    {
                        Directory.CreateDirectory(pathString);
                        return true;
                    }
                    catch (AccessViolationException)
                    {
                        MessageBox.Show("U heeft geen schrijfrechten op de locatie: " + pathString, "Waarschuwing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            else
            {
                string appDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folderName);
                if (!Directory.Exists(appDir))
                {
                    try
                    {
                        Directory.CreateDirectory(appDir);
                        return true;
                    }
                    catch (AccessViolationException)
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
        }

        private void SetProperties()
        {
            this.ApplicationPath = Get_Applicatiepad();
            this.UserName = GetUserName();
            this.MachineName = GetMachineName();
            this.WindowsVersion = GetWindowsVersion(2);
            this.ProcessorCount = GetProcessorCount();
            this.ProcessorId = GetProcessorId();
            this.BiosId = GetBiosId();
            this.TotalRam = GetTotalRam();
            this.MonitorWidth = GetMonitorWidth();
            this.MonitorCount = GetMonitorCount();
            this.DotNetFrameWorkVersion = GetAllDotNetVersions();
        }

        private static string Get_Applicatiepad()  //get the application path
        {
            try
            {
                string appPath;
                appPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
                appPath += "\\";                                        // add \to the path
                return appPath.Replace("file:\\", "");  //remove the text "file:\\" from the path
            }
            catch (ArgumentException aex)
            {
                throw new InvalidOperationException(aex.Message);
            }
            catch (Exception)
            {
                throw new InvalidOperationException("");
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
                throw new InvalidOperationException("");
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
                throw new InvalidOperationException("");
            }
        }

        private static string GetWindowsVersion(short type)
        {
            try
            {
                string osVersion = "";

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
            catch (ArgumentException aex)
            {
                throw new InvalidOperationException(aex.Message);
            }
            catch (Exception)
            {
                throw new InvalidOperationException("");
            }
        }

        private static string GetProcessorCount()
        {
            try
            {
                return Convert.ToString(Environment.ProcessorCount, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                throw new InvalidOperationException("");
            }
        }

        private static string GetProcessorId()
        {
            try
            {
                string result = "";
                try
                {
                    ManagementObjectSearcher mbs = new ("Select ProcessorID From Win32_processor");  // Add reference assemblies: system.management
                    ManagementObjectCollection mbsList = mbs.Get();

                    foreach (ManagementObject mo in mbsList)
                    {
                        result = mo["ProcessorID"].ToString();
                    }
                    mbs.Dispose();

                    return result;
                    //More (all) options: https://msdn.microsoft.com/en-us/library/aa394373(v=vs.85).aspx
                }
                catch (Exception)
                {
                    throw new InvalidOperationException("");
                }
            }
            catch (Exception) { throw; }
        }

        private static string GetBiosId()
        {
            try
            {
                string bios = "";
                using (ManagementObjectSearcher searcher = new ("SELECT SerialNumber FROM Win32_BIOS"))
                {
                    foreach (ManagementObject mObject in searcher.Get())
                    {
                        bios = mObject["SerialNumber"].ToString();  // Manufacturer
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(bios))
                {
                    return bios;
                }
                else
                {
                    return "Geen BiosId gevonden.";
                }
            }
            catch (Exception)
            {
                throw new InvalidOperationException("");
            }
        }

        private static string GetTotalRam()
        {
            try
            {
                using ManagementClass mc = new ("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();

                foreach (ManagementObject item in moc)
                {
                    return Convert.ToString(Math.Round(Convert.ToDouble(item.Properties["TotalPhysicalMemory"].Value, CultureInfo.InvariantCulture) / 1073741824, 2), CultureInfo.InvariantCulture) + " GB";
                }

                return "Geen Totaal telling Ram gevonden.";
            }
            catch (Exception)
            {
                throw new InvalidOperationException("");
            }
        }

        private static int GetMonitorWidth()
        {
            try
            {
                var screen = Screen.PrimaryScreen.Bounds;
                return screen.Width;
            }
            catch (Exception)
            {
                throw new InvalidOperationException("");
            }
        }

        private static int GetMonitorCount()
        {
            try
            {
                return SystemInformation.MonitorCount;
            }
            catch (Exception)
            {
                throw new InvalidOperationException("");
            }
        }

        public static List<string> GetAllDotNetVersions()
        {
            GetDotNetVersion netVersion = new ();
            return netVersion.DotNetVersions();
        }

        #endregion Methods


        #region IDisposable
        //Bron : https://msdn.microsoft.com/en-us/library/b1yfkh5e(v=vs.100).aspx

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
                    this.ProcessorId = string.Empty;
                    this.BiosId = string.Empty;
                    this.TotalRam = string.Empty;
                    this.MonitorWidth = -1;
                    this.MonitorCount = -1;
                    this.DotNetFrameWorkVersion.Clear();
                }

                // Free your own state (unmanaged objects).
                // Set large fields to null.
                this.disposed = true;
            }
        }
        #endregion IDisposable
    }

    public class GetDotNetVersion
    {
        // bron: https://docs.microsoft.com/en-us/dotnet/framework/migration-guide/how-to-determine-which-versions-are-installed
        private readonly List<string> netVersions = new ();

        public GetDotNetVersion()
        {
            this.Get45PlusFromRegistry();
            this.GetVersionFromRegistry();
        }

        public List<string> DotNetVersions()
        {
            return this.netVersions;
        }

        private void Get45PlusFromRegistry()
        {
            const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";

            using (var ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey))
            {
                if (ndpKey != null && ndpKey.GetValue("Release") != null)
                {
                    this.netVersions.Add($".NET Framework Version: {CheckFor45PlusVersion((int)ndpKey.GetValue("Release"))}");
                }
                else
                {
                    this.netVersions.Add(".NET Framework Version 4.5 of nieuwer is niet gevonden.");
                }
            }

            // Checking the version using >= enables forward compatibility.
            static string CheckFor45PlusVersion(int releaseKey)
            {
                if (releaseKey >= 528040)
                {
                    return "4.8 of nieuwer";
                }

                if (releaseKey >= 461808)
                {
                    return "4.7.2";
                }

                if (releaseKey >= 461308)
                {
                    return "4.7.1";
                }

                if (releaseKey >= 460798)
                {
                    return "4.7";
                }

                if (releaseKey >= 394802)
                {
                    return "4.6.2";
                }

                if (releaseKey >= 394254)
                {
                    return "4.6.1";
                }

                if (releaseKey >= 393295)
                {
                    return "4.6";
                }

                if (releaseKey >= 379893)
                {
                    return "4.5.2";
                }

                if (releaseKey >= 378675)
                {
                    return "4.5.1";
                }

                if (releaseKey >= 378389)
                {
                    return "4.5";
                }

                // This code should never execute. A non-null release key should mean
                // that 4.5 or later is installed.
                return "Geen versie 4.5 of nieuwere versie aangetroffen.";
            }
        }

        private void GetVersionFromRegistry()
        {
            // Opens the registry key for the .NET Framework entry.
            using RegistryKey ndpKey =
                    RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).
                    OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\");

            foreach (var versionKeyName in ndpKey.GetSubKeyNames())
            {
                // Skip .NET Framework 4.5 version information.
                if (versionKeyName == "v4")
                {
                    continue;
                }

                if (versionKeyName.StartsWith("v"))
                {

                    RegistryKey versionKey = ndpKey.OpenSubKey(versionKeyName);
                    // Get the .NET Framework version value.
                    var name = (string)versionKey.GetValue("Version", "");
                    // Get the service pack (SP) number.
                    var sp = versionKey.GetValue("SP", "").ToString();

                    // Get the installation flag, or an empty string if there is none.
                    var install = versionKey.GetValue("Install", "").ToString();
                    if (string.IsNullOrEmpty(install)) // No install info; it must be in a child subkey.
                    {
                        this.netVersions.Add(".NET Framework Version: " + versionKeyName + " " + name);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(sp) && install == "1")
                        {
                            this.netVersions.Add(".NET Framework Version: " + versionKeyName + " " + name + " " + "SP" + sp);
                        }
                    }

                    if (!string.IsNullOrEmpty(name))
                    {
                        continue;
                    }

                    foreach (var subKeyName in versionKey.GetSubKeyNames())
                    {
                        RegistryKey subKey = versionKey.OpenSubKey(subKeyName);
                        name = (string)subKey.GetValue("Version", string.Empty);
                        if (!string.IsNullOrEmpty(name))
                            sp = subKey.GetValue(".NET Framework Version: " + "SP", "").ToString();

                        install = subKey.GetValue("Install", string.Empty).ToString();
                        if (string.IsNullOrEmpty(install)) //No install info; it must be later.
                        {
                            this.netVersions.Add(".NET Framework Version: " + versionKeyName + " " + name);
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(sp) && install == "1")
                            {
                                this.netVersions.Add(".NET Framework Version: " + versionKeyName + " " + name + " " + "SP" + sp);
                            }
                            else if (install == "1")
                            {
                                this.netVersions.Add(subKeyName + " " + name);
                            }
                        }
                    }
                }
            }
        }
    }

    #region Custum Exception
    [Serializable()]
    public class InvalidOperationException : System.Exception
    {
        public InvalidOperationException() : base() { }
        public InvalidOperationException(string message) : base(message) { }
        public InvalidOperationException(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client. 
        protected InvalidOperationException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    #endregion Custum Exception
}
