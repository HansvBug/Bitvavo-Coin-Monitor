using System;
using System.Collections.Generic;
using System.Globalization;

namespace CM
{
    public class ProcessArguments : IDisposable
    {
        #region Properties
        public List<String> cmdLineArg = new List<string>();

        public string ArgIntall { get; set; }
        public string ArgInstallUser { get; set; }
        public string ArgInstallOwner { get; set; }
        public string ArgDebug { get; set; }
        #endregion Properties

        #region constructor
        public ProcessArguments()
        {
            GetArguments();
        }
        #endregion constructor

        private void GetArguments()
        {
            string[] args = Environment.GetCommandLineArgs();   //store command line arguments

            foreach (string arg in args)
            {
                string argument = Convert.ToString(arg, CultureInfo.InvariantCulture);
                cmdLineArg.Add(argument);  //0 = program name
                switch (argument)
                {
                    case "Install":
                        ArgIntall = "Install";
                        break;                   
                    case "DebugMode=On":
                        ArgDebug = "DebugMode=On";
                        break;
                }
            }
        }

        #region Dispose 
        private bool disposed;

        //Implement IDisposable.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Free other state (managed objects).
                    cmdLineArg = null;
                }
                // Free your own state (unmanaged objects).
                // Set large fields to null.

                disposed = true;
            }
        }
        #endregion Dispose
    }
}
