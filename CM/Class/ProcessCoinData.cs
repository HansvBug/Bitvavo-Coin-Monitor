using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace CM
{
    public class ProcessCoinData : IDisposable
    {

        #region constructor
        public ProcessCoinData()
        {
            //
        }
        #endregion constructor


        #region Dispose
        // Flag: Has Dispose already been called?
        bool disposed = false;

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
            }

            this.disposed = true;
        }
        #endregion Dispose
    }
}
