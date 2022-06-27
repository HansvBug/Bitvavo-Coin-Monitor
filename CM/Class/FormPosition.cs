using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using CM.Class;

namespace CM
{
    public class FormPosition : IDisposable
    {
        private readonly FormMain MainForm;

        private readonly FormConfigure ConfigureForm;

        private dynamic JsonObjSettings { get; set; }

        #region Constructor
        public FormPosition(FormMain MainForm)
        {
            this.MainForm = MainForm;
            this.JsonObjSettings = MainForm.JsonObjSettings;
        }

        public FormPosition(FormConfigure ConfigureForm)
        {
            this.ConfigureForm = ConfigureForm;
            this.JsonObjSettings = ConfigureForm.JsonObjSettings;
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
            if (CmDebugMode.DebugMode)
            {
                Logging.WriteToLogInformation("Ophalen scherm positie hoofdscherm.");
            }

            // default
            this.MainForm.WindowState = FormWindowState.Normal;
            this.MainForm.StartPosition = FormStartPosition.WindowsDefaultBounds;

            if (this.JsonObjSettings != null && this.JsonObjSettings.FormMain != null)
            {
                Rectangle FrmRect = new ()
                {
                    X = this.JsonObjSettings.FormMain[0].FrmX,
                    Y = this.JsonObjSettings.FormMain[0].FrmY,
                    Width = this.JsonObjSettings.FormMain[0].FrmWidth,
                    Height = this.JsonObjSettings.FormMain[0].FrmHeight,
                };

                // check if the saved bounds are nonzero and visible on any screen
                if (FrmRect != Rectangle.Empty && IsVisibleOnAnyScreen(FrmRect))
                {   // first set the bounds
                    this.MainForm.StartPosition = FormStartPosition.Manual;
                    this.MainForm.DesktopBounds = FrmRect;

                    // afterwards set the window state to the saved value (which could be Maximized)
                    this.MainForm.WindowState = this.JsonObjSettings.FormMain[0].FrmWindowState;
                }
                else
                {
                    // this resets the upper left corner of the window to windows standards
                    this.MainForm.StartPosition = FormStartPosition.WindowsDefaultLocation;

                    // we can still apply the saved size
                    if (FrmRect != Rectangle.Empty)
                    {
                        this.MainForm.Size = FrmRect.Size;
                    }
                }
            }
        }

        public void SaveMainFormPosition()
        {
            if (CmDebugMode.DebugMode)
            {
                Logging.WriteToLogInformation("Opslaan scherm positie hoofdscherm.");
            }

            string SettingsFile = this.JsonObjSettings.AppParam[0].SettingsFileLocation;

            if (File.Exists(SettingsFile))
            {
                if (this.MainForm.WindowState == FormWindowState.Normal)
                {
                    this.JsonObjSettings.FormMain[0].FrmWindowState = FormWindowState.Normal;

                    if (this.MainForm.Location.X >= 0)
                    {
                        this.JsonObjSettings.FormMain[0].FrmX = this.MainForm.Location.X;
                    }
                    else
                    {
                        this.JsonObjSettings.FormMain[0].FrmX = 0;
                    }

                    if (this.MainForm.Location.Y >= 0)
                    {
                        this.JsonObjSettings.FormMain[0].FrmY = this.MainForm.Location.Y;
                    }
                    else
                    {
                        this.JsonObjSettings.FormMain[0].FrmY = 0;
                    }

                    this.JsonObjSettings.FormMain[0].FrmHeight = this.MainForm.Height;
                    this.JsonObjSettings.FormMain[0].FrmWidth = this.MainForm.Width;
                }
                else
                {
                    this.JsonObjSettings.FormMain[0].FrmWindowState = this.MainForm.WindowState;
                }
            }
        }

        #endregion FormMain

        #region Form Configure
        public void LoadConfigureFormPosition()
        {
            if (CmDebugMode.DebugMode)
            {
                Logging.WriteToLogInformation("Ophalen scherm positie configuratie scherm.");
            }

            // this is the default
            this.ConfigureForm.WindowState = FormWindowState.Normal;
            this.ConfigureForm.StartPosition = FormStartPosition.WindowsDefaultBounds;


            if (this.JsonObjSettings != null && this.JsonObjSettings.FormMain != null)
            {
                Rectangle FrmRect = new ()
                {
                    X = this.JsonObjSettings.FormConfig[0].FrmX,
                    Y = this.JsonObjSettings.FormConfig[0].FrmY,
                    Width = this.JsonObjSettings.FormConfig[0].FrmWidth,
                    Height = this.JsonObjSettings.FormConfig[0].FrmHeight,
                };

                if (FrmRect != Rectangle.Empty && IsVisibleOnAnyScreen(FrmRect))
                {
                    this.ConfigureForm.StartPosition = FormStartPosition.Manual;
                    this.ConfigureForm.DesktopBounds = FrmRect;

                    this.ConfigureForm.WindowState = this.JsonObjSettings.FormConfig[0].FrmWindowState;
                }
                else
                {
                    this.ConfigureForm.StartPosition = FormStartPosition.WindowsDefaultLocation;

                    if (FrmRect != Rectangle.Empty)
                    {
                        this.ConfigureForm.Size = FrmRect.Size;
                    }
                }
            }
        }

        public void SaveConfigureFormPosition()
        {
            if (CmDebugMode.DebugMode)
            {
                Logging.WriteToLogInformation("Opslaan scherm positie configuratie scherm.");
            }

            string SettingsFile = this.JsonObjSettings.AppParam[0].SettingsFileLocation;

            if (File.Exists(SettingsFile))
            {

                if (this.ConfigureForm.WindowState == FormWindowState.Normal)
                {

                    this.JsonObjSettings.FormConfig[0].FrmWindowState = FormWindowState.Normal;

                    if (this.ConfigureForm.Location.X >= 0)
                    {
                        this.JsonObjSettings.FormConfig[0].FrmX = this.ConfigureForm.Location.X;
                    }
                    else
                    {
                        this.JsonObjSettings.FormConfig[0].FrmX = 0;
                    }

                    if (this.ConfigureForm.Location.Y >= 0)
                    {
                        this.JsonObjSettings.FormConfig[0].FrmY = this.ConfigureForm.Location.Y;
                    }
                    else
                    {
                        this.JsonObjSettings.FormConfig[0].FrmY = 0;
                    }

                    this.JsonObjSettings.FormConfig[0].FrmHeight = this.ConfigureForm.Height;
                    this.JsonObjSettings.FormConfig[0].FrmWidth = this.ConfigureForm.Width;
                }
                else
                {
                    this.JsonObjSettings.FormConfig[0].FrmWindowState = this.ConfigureForm.WindowState;
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
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

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
