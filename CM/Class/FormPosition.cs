using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace CM
{
    public class FormPosition : IDisposable
    {
        private readonly FormMain MainForm;
        private readonly FormConfigure ConfigureForm;

        public bool DebugMode { get; set; }
        private dynamic JsonObjSettings { get; set; }

        #region Constructor
        public FormPosition(FormMain MainForm)
        {
            this.MainForm = MainForm;
            JsonObjSettings = MainForm.JsonObjSettings;
        }
        public FormPosition(FormConfigure ConfigureForm)
        {
            this.ConfigureForm = ConfigureForm;
            JsonObjSettings = ConfigureForm.JsonObjSettings;
        }
        #endregion Constructor

        #region Helper
        private static bool IsVisibleOnAnyScreen(Rectangle rect)
        {
            foreach (Screen screen in Screen.AllScreens)
            {
                if (screen.WorkingArea.IntersectsWith(rect))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion Helper

        #region FormMain        
        public void LoadMainFormPosition()
        {
            if (DebugMode) { Logging.WriteToLogInformation("Ophalen scherm positie hoofdscherm."); }
            // default
            MainForm.WindowState = FormWindowState.Normal;
            MainForm.StartPosition = FormStartPosition.WindowsDefaultBounds;

            if (JsonObjSettings != null && JsonObjSettings.FormMain != null)
            {
                Rectangle FrmRect = new()
                {
                    X = JsonObjSettings.FormMain[0].FrmX,
                    Y = JsonObjSettings.FormMain[0].FrmY,
                    Width = JsonObjSettings.FormMain[0].FrmWidth,
                    Height = JsonObjSettings.FormMain[0].FrmHeight
                };

                // check if the saved bounds are nonzero and visible on any screen
                if (FrmRect != Rectangle.Empty && IsVisibleOnAnyScreen(FrmRect))
                {   // first set the bounds
                    MainForm.StartPosition = FormStartPosition.Manual;
                    MainForm.DesktopBounds = FrmRect;

                    // afterwards set the window state to the saved value (which could be Maximized)
                    MainForm.WindowState = JsonObjSettings.FormMain[0].FrmWindowState;
                }
                else
                {
                    // this resets the upper left corner of the window to windows standards
                    MainForm.StartPosition = FormStartPosition.WindowsDefaultLocation;

                    // we can still apply the saved size
                    if (FrmRect != Rectangle.Empty)
                    {
                        MainForm.Size = FrmRect.Size;
                    }
                }
            }
        }
        public void SaveMainFormPosition()
        {
            if (DebugMode) { Logging.WriteToLogInformation("Opslaan scherm positie hoofdscherm."); }
            string SettingsFile = JsonObjSettings.AppParam[0].SettingsFileLocation;

            if (File.Exists(SettingsFile))
            {
                if (MainForm.WindowState == FormWindowState.Normal)
                {
                    JsonObjSettings.FormMain[0].FrmWindowState = FormWindowState.Normal;

                    if (MainForm.Location.X >= 0)
                    {
                        JsonObjSettings.FormMain[0].FrmX = MainForm.Location.X;
                    }
                    else
                    {
                        JsonObjSettings.FormMain[0].FrmX = 0;
                    }

                    if (MainForm.Location.Y >= 0)
                    {
                        JsonObjSettings.FormMain[0].FrmY = MainForm.Location.Y;
                    }
                    else
                    {
                        JsonObjSettings.FormMain[0].FrmY = 0;
                    }
                    JsonObjSettings.FormMain[0].FrmHeight = MainForm.Height;
                    JsonObjSettings.FormMain[0].FrmWidth = MainForm.Width;
                }
                else
                {
                    JsonObjSettings.FormMain[0].FrmWindowState = MainForm.WindowState;
                }
            }
        }

        #endregion FormMain

        #region Form Configure
        public void LoadConfigureFormPosition()
        {
            if (DebugMode) { Logging.WriteToLogInformation("Ophalen scherm positie configuratie scherm."); }
            // this is the default
            ConfigureForm.WindowState = FormWindowState.Normal;
            ConfigureForm.StartPosition = FormStartPosition.WindowsDefaultBounds;


            if (JsonObjSettings != null && JsonObjSettings.FormMain != null)
            {
                Rectangle FrmRect = new()
                {
                    X = JsonObjSettings.FormConfig[0].FrmX,
                    Y = JsonObjSettings.FormConfig[0].FrmY,
                    Width = JsonObjSettings.FormConfig[0].FrmWidth,
                    Height = JsonObjSettings.FormConfig[0].FrmHeight
                };

                if (FrmRect != Rectangle.Empty && IsVisibleOnAnyScreen(FrmRect))
                {
                    ConfigureForm.StartPosition = FormStartPosition.Manual;
                    ConfigureForm.DesktopBounds = FrmRect;

                    ConfigureForm.WindowState = JsonObjSettings.FormConfig[0].FrmWindowState; ;
                }
                else
                {
                    ConfigureForm.StartPosition = FormStartPosition.WindowsDefaultLocation;

                    if (FrmRect != Rectangle.Empty)
                    {
                        ConfigureForm.Size = FrmRect.Size;
                    }
                }
            }
        }
        public void SaveConfigureFormPosition()
        {
            if (DebugMode) { Logging.WriteToLogInformation("Opslaan scherm positie configuratie scherm."); }
            string SettingsFile = JsonObjSettings.AppParam[0].SettingsFileLocation;

            if (File.Exists(SettingsFile))
            {

                if (ConfigureForm.WindowState == FormWindowState.Normal)
                {

                    JsonObjSettings.FormConfig[0].FrmWindowState = FormWindowState.Normal;

                    if (ConfigureForm.Location.X >= 0)
                    {
                        JsonObjSettings.FormConfig[0].FrmX = ConfigureForm.Location.X;
                    }
                    else
                    {
                        JsonObjSettings.FormConfig[0].FrmX = 0;
                    }

                    if (ConfigureForm.Location.Y >= 0)
                    {
                        JsonObjSettings.FormConfig[0].FrmY = ConfigureForm.Location.Y;
                    }
                    else
                    {
                        JsonObjSettings.FormConfig[0].FrmY = 0;
                    }
                    JsonObjSettings.FormConfig[0].FrmHeight = ConfigureForm.Height;
                    JsonObjSettings.FormConfig[0].FrmWidth = ConfigureForm.Width;

                }
                else
                {
                    JsonObjSettings.FormConfig[0].FrmWindowState = MainForm.WindowState;
                }
            }
        }
        #endregion Form Configure

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
