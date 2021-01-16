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
        private readonly List<TreeNode> CurrentNodeMatches = new List<TreeNode>();

        private int LastNodeIndex = 0;

        private string LastSearchText;
        #endregion Properties

        public void SearchInTreeViewNodes(TreeView trv, string searchText)
        {
            if (String.IsNullOrEmpty(searchText) || trv == null)
            {
                return;
            };

            if (LastSearchText != searchText)
            {
                if (trv.Nodes.Count > 0)
                {
                    //It's a new Search
                    try
                    {
                        CurrentNodeMatches.Clear();
                        LastSearchText = searchText;
                        LastNodeIndex = 0;
                        SearchNodes(searchText, trv.Nodes[0]);


                        TreeNode selectedNode = CurrentNodeMatches[LastNodeIndex];

                        trv.SelectedNode = selectedNode;
                        trv.SelectedNode.Expand();
                        trv.Select();
                        LastNodeIndex++;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        //If the searchtext does not exist it gives an exception
                    }
                }
            }
            else
            {
                if (LastNodeIndex >= 0 && CurrentNodeMatches.Count > 0 && LastNodeIndex < CurrentNodeMatches.Count)
                {
                    try
                    {
                        TreeNode selectedNode = CurrentNodeMatches[LastNodeIndex];
                        LastNodeIndex++;
                        trv.SelectedNode = selectedNode;
                        trv.SelectedNode.Expand();
                        trv.Select();
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        //If the searchtext does not exist it gives anexception
                    }
                }
                else if (LastNodeIndex == CurrentNodeMatches.Count)
                {
                    try
                    {
                        TreeNode selectedNode = CurrentNodeMatches[0];
                        LastNodeIndex = 0;
                        trv.SelectedNode = selectedNode;
                        trv.SelectedNode.Expand();
                        trv.Select();
                        LastNodeIndex++;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        //If the searchtext does not exist it gives anexception
                    }
                }
                else
                {
                    //It's a new Search
                    CurrentNodeMatches.Clear();
                    LastSearchText = searchText;
                    LastNodeIndex = 0;
                    SearchNodes(searchText, trv.Nodes[0]);
                }
            }
        }

        private void SearchNodes(string SearchText, TreeNode StartNode)
        {
            while (StartNode != null)
            {
                if (StartNode.Text.ToUpperInvariant().Contains(SearchText.ToUpperInvariant()))
                {
                    CurrentNodeMatches.Add(StartNode);
                };
                if (StartNode.Nodes.Count != 0)
                {
                    SearchNodes(SearchText, StartNode.Nodes[0]);//Recursive Search 
                };
                StartNode = StartNode.NextNode;
            };

        }
    }
}
