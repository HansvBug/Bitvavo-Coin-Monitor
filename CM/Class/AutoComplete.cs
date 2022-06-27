using System;
using System.Windows.Forms;

namespace CM
{
    public class AutoComplete : IDisposable
    {
        public AutoCompleteStringCollection CreAutoCompleteListFromTrv(TreeView Trv)
        {
            if (Trv != null)
            {
                AutoCompleteStringCollection DataCollection = new();
                this.AddItems(DataCollection, Trv);

                return DataCollection;
            }
            else
            {
                return null;
            }
        }

        private void AddItems(AutoCompleteStringCollection col, TreeView Trv)
        {
            TreeNodeCollection nodes = Trv.Nodes;
            foreach (TreeNode n in nodes)
            {
                this.GetTrvNodeName(n, col);
            }
        }

        private void GetTrvNodeName(TreeNode treeNode, AutoCompleteStringCollection col)
        {
            col.Add(treeNode.Name);
            foreach (TreeNode tn in treeNode.Nodes)
            {
                this.GetTrvNodeName(tn, col);
            }
        }


        #region Dispose
        private bool disposed = false;

        // Implement IDisposable.
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

                }
                // Free your own state (unmanaged objects).
                // Set large fields to null.

                this.disposed = true;
            }
        }
        #endregion Dispose
    }
}
