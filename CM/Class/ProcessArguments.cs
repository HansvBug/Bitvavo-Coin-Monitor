using System;
using System.Collections.Generic;
using System.Globalization;

namespace CM
{
    public class ProcessArguments : IDisposable
    {
        #region Properties
        public List<String> cmdLineArg = new();

        public string ArgIntall { get; set; }
        public string ArgInstallUser { get; set; }
        public string ArgInstallOwner { get; set; }
        public string ArgDebug { get; set; }
        #endregion Properties

        #region constructor
        public ProcessArguments()
        {
            this.GetArguments();
        }
        #endregion constructor

        private void GetArguments()
        {
            string[] args = Environment.GetCommandLineArgs();   // Store command line arguments

            foreach (string arg in args)
            {
                string argument = Convert.ToString(arg, CultureInfo.InvariantCulture);
                this.cmdLineArg.Add(argument);  // 0 = program name
                switch (argument)
                {
                    case "Install":
                        this.ArgIntall = "Install";
                        break;

                    case "DebugMode=On":
                        this.ArgDebug = "DebugMode=On";
                        break;
                }
            }
        }

        #region Dispose
        private bool disposed;

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
                    this.cmdLineArg = null;
                }

                // Free your own state (unmanaged objects).
                // Set large fields to null.

                this.disposed = true;
            }
        }
        #endregion Dispose
    }
}
