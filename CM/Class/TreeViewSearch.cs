using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CM
{
    public class TreeViewSearch
    {
        //TODO; Can not be idisposible otherwise find next does not work  --> Check if this still is true

        #region Properties
        private readonly List<TreeNode> CurrentNodeMatches = new();

        private int LastNodeIndex = 0;

        private string LastSearchText;
        #endregion Properties

        public void SearchInTreeViewNodes(TreeView trv, string searchText)
        {
            if (string.IsNullOrEmpty(searchText) || trv == null)
            {
                return;
            };

            if (this.LastSearchText != searchText)
            {
                if (trv.Nodes.Count > 0)
                {
                    // It's a new Search
                    try
                    {
                        this.CurrentNodeMatches.Clear();
                        this.LastSearchText = searchText;
                        this.LastNodeIndex = 0;
                        this.SearchNodes(searchText, trv.Nodes[0]);


                        TreeNode selectedNode = this.CurrentNodeMatches[this.LastNodeIndex];

                        trv.SelectedNode = selectedNode;
                        trv.SelectedNode.Expand();
                        trv.Select();
                        this.LastNodeIndex++;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        // If the searchtext does not exist it gives an exception
                    }
                }
            }
            else
            {
                if (this.LastNodeIndex >= 0 && this.CurrentNodeMatches.Count > 0 && this.LastNodeIndex < this.CurrentNodeMatches.Count)
                {
                    try
                    {
                        TreeNode selectedNode = this.CurrentNodeMatches[this.LastNodeIndex];
                        this.LastNodeIndex++;
                        trv.SelectedNode = selectedNode;
                        trv.SelectedNode.Expand();
                        trv.Select();
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        // If the searchtext does not exist it gives anexception
                    }
                }
                else if (this.LastNodeIndex == this.CurrentNodeMatches.Count)
                {
                    try
                    {
                        TreeNode selectedNode = this.CurrentNodeMatches[0];
                        this.LastNodeIndex = 0;
                        trv.SelectedNode = selectedNode;
                        trv.SelectedNode.Expand();
                        trv.Select();
                        this.LastNodeIndex++;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        // If the searchtext does not exist it gives anexception
                    }
                }
                else
                {
                    // It's a new Search
                    this.CurrentNodeMatches.Clear();
                    this.LastSearchText = searchText;
                    this.LastNodeIndex = 0;
                    this.SearchNodes(searchText, trv.Nodes[0]);
                }
            }
        }

        private void SearchNodes(string searchText, TreeNode startNode)
        {
            while (startNode != null)
            {
                if (startNode.Text.ToUpperInvariant().Contains(searchText.ToUpperInvariant()))
                {
                    this.CurrentNodeMatches.Add(startNode);
                }

                if (startNode.Nodes.Count != 0)
                {
                    this.SearchNodes(searchText, startNode.Nodes[0]); // Recursive Search.
                }

                startNode = startNode.NextNode;
            }

        }
    }
}
