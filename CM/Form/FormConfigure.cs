using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Forms;
using CM.Class;

namespace CM
{
    public partial class FormConfigure : Form
    {
        #region Properties

        public dynamic JsonObjSettings { get; set; }

        private int CopyAppDataBaseAfterEveryXStartups { get; set; }

        private string DecimalSeperator { get; set; }

        private int CheckedTrvNodes { get; set; } // The number of checked treeview nodes

        private readonly List<string> SelectedCoinNames = new();
        #endregion Properties

        public FormConfigure()
        {
            this.InitializeComponent();
            this.DecimalSeperator = CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator;
            this.TextBoxSearchCoinName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        }

        #region load form
        private void FormConfigure_Load(object sender, EventArgs e)
        {
            this.LoadSettings();
            this.ApplySettings();
            this.LoadFormPosition();
            this.GetCoinNames();
        }

        private void SetAutoComplete()
        {
            using AutoComplete aCompleteSource = new();
            AutoCompleteStringCollection DataCollection;
            DataCollection = aCompleteSource.CreAutoCompleteListFromTrv(this.TreeViewCoinNames);  // Create the autocomplete list for the search box

            this.TextBoxSearchCoinName.AutoCompleteSource = AutoCompleteSource.CustomSource;
            this.TextBoxSearchCoinName.AutoCompleteCustomSource = DataCollection;
        }

        private void LoadSettings()
        {
            using SettingsManager Set = new();
            Set.LoadSettings();
            this.JsonObjSettings = Set.JsonObjSettings;
        }

        private void ApplySettings()
        {
            if (this.JsonObjSettings.AppParam[0].AppendLogFile)
            {
                this.CheckBoxAppenLogFile.Checked = true;
            }
            else
            {
                this.CheckBoxAppenLogFile.Checked = false;
            }

            if (this.JsonObjSettings.AppParam[0].ActivateLogging)
            {
                this.CheckBoxActivateLogging.Checked = true;
            }
            else
            {
                this.CheckBoxActivateLogging.Checked = false;
            }

            this.TextBoxLocationSettingsFile.Text = this.JsonObjSettings.AppParam[0].SettingsFileLocation;
            this.TextBoxLocationLogFile.Text = this.JsonObjSettings.AppParam[0].LogFileLocation + AppSettingsDefault.LogFileName;
            this.TextBoxLocationDatabaseFile.Text = this.JsonObjSettings.AppParam[0].DatabaseLocation + AppSettingsDefault.SqlLiteDatabaseName;

            this.LabelVersion.Text = "Versie : " + this.JsonObjSettings.AppParam[0].ApplicationVersion;
            this.LabelBuildDate.Text = "Build datum : " + this.JsonObjSettings.AppParam[0].ApplicationBuildDate;

            int CopyAppDataBaseAfterEveryXStartups = this.JsonObjSettings.AppParam[0].CopyAppDataBaseAfterEveryXStartups;
            this.CopyDatabaseIntervalTextBox.Text = CopyAppDataBaseAfterEveryXStartups.ToString();

            this.TextBoxUrl1.Text = this.JsonObjSettings.AppParam[0].Url1;
            this.TextBoxUrl2.Text = this.JsonObjSettings.AppParam[0].Url2;

            this.TextBoxRateLimit.Text = this.JsonObjSettings.AppParam[0].RateLimit.ToString();
            this.TextBoxWarnPercentage.Text = this.JsonObjSettings.AppParam[0].WarnPercentage.ToString();
        }

        private void LoadFormPosition()
        {
            using FormPosition FormPosition = new (this);
            FormPosition.LoadConfigureFormPosition();
        }
        #endregion load form

        private void CheckBoxEnableUrl_Click(object sender, EventArgs e)
        {
            if (this.CheckBoxEnableUrl.Checked)
            {
                this.TextBoxUrl1.Enabled = true;
                this.TextBoxUrl2.Enabled = true;
            }
            else
            {
                this.TextBoxUrl1.Enabled = false;
                this.TextBoxUrl2.Enabled = false;
            }
        }

        #region Form closing
        private void FormConfigure_FormClosing(object sender, FormClosingEventArgs e)
        {
            // TODO coins opslaanin SQLlite
            Logging.WriteToLogInformation("Sluiten configuratie scherm.");
            this.SaveFormPosition();
            this.SaveSettings();
            this.SaveCheckedCoins();
            this.CreateTables();
        }

        private void SaveFormPosition()
        {
            using FormPosition FormPosition = new(this);
            FormPosition.SaveConfigureFormPosition();
        }

        private void SaveSettings()
        {
            SettingsManager.SaveSettings(this.JsonObjSettings);
        }

        private void SaveCheckedCoins()
        {
            List<string> CoinNames = new();
            ApplicationDatabase addCoin = new();
            foreach (TreeNode aNode in this.TreeViewCoinNames.Nodes) // First create a list withe the coin names
            {
                if (aNode.Checked == true) // At least one node must checked to start the filter
                {
                    CoinNames.Add(aNode.Name);
                }
            }

            if (CoinNames.Count > 0)  // If any coin name is checked then save it
            {
                addCoin.SaveCoinNames(CoinNames);
            }
        }

        private void CreateTables()
        {
            List<string> CoinNames = new();
            ApplicationDatabase addCoin = new();

            foreach (TreeNode aNode in this.TreeViewCoinNames.Nodes) // first create a list withe the coin names
            {
                if (aNode.Checked == true) // At least one node must checked to start the filter
                {
                    CoinNames.Add(aNode.Name);
                }
            }

            if (CoinNames.Count > 0) // If any coin name is checked then save it
            {
                foreach (string tbl in CoinNames)
                {
                    addCoin.CreateCoinTable(tbl);
                }
            }
        }
        #endregion Form closing

        private void CheckBoxActivateLogging_Click(object sender, EventArgs e)
        {
            if (this.CheckBoxActivateLogging.Checked)
            {
                this.CheckBoxAppenLogFile.Enabled = true;

                this.JsonObjSettings.AppParam[0].ActivateLogging = true;

                Logging.ActivateLogging = true;
                Logging.StartLogging();
                Logging.WriteToLogInformation("Logging aangezet.");
            }
            else
            {
                this.CheckBoxAppenLogFile.Checked = false;
                this.CheckBoxAppenLogFile.Enabled = false;

                this.JsonObjSettings.AppParam[0].ActivateLogging = false;
                this.JsonObjSettings.AppParam[0].AppendLogFile = false;

                Logging.WriteToLogInformation("Logging uitgezet.");
                Logging.StopLogging();
                Logging.ActivateLogging = false;
            }
        }

        private void CheckBoxAppenLogFile_Click(object sender, EventArgs e)
        {
            if (this.CheckBoxAppenLogFile.Checked)
            {
                this.JsonObjSettings.AppParam[0].AppendLogFile = true;
                Logging.WriteToLogInformation("Logging aanvullen is aangezet.");
            }
            else
            {
                this.JsonObjSettings.AppParam[0].AppendLogFile = false;
                Logging.WriteToLogInformation("Logging aanvullen is uitgezet.");
            }
        }

        private void CopyDatabaseIntervalTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.CopyAppDataBaseAfterEveryXStartups = Int32.Parse(this.CopyDatabaseIntervalTextBox.Text, CultureInfo.InvariantCulture);
                if (this.CopyAppDataBaseAfterEveryXStartups > 200)
                {
                    this.CopyAppDataBaseAfterEveryXStartups = 200;
                    this.CopyDatabaseIntervalTextBox.Text = "200";
                    MessageBox.Show("De maximale waarde is 200", "Informatie", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.JsonObjSettings.AppParam[0].CopyAppDataBaseAfterEveryXStartups = this.CopyAppDataBaseAfterEveryXStartups;
            }
            catch (FormatException ex)
            {
                Logging.WriteToLogError("Fout opgetreden bij het omzetten van een string naar een integer.");
                Logging.WriteToLogError("Melding:");
                Logging.WriteToLogError(ex.Message);
                this.CopyDatabaseIntervalTextBox.Text = "0";
            }
        }

        private void ButtonSelectAll_Click(object sender, EventArgs e)
        {
            foreach (TreeNode node in this.TreeViewCoinNames.Nodes)  //29-12-2017; hvb checklistbox replaced by  treeview
            {
                node.Checked = true;
            }
        }

        private void ButtonDeselectAll_Click(object sender, EventArgs e)
        {
            foreach (TreeNode node in this.TreeViewCoinNames.Nodes)
            {
                node.Checked = false;
            }
        }

        private void ButtonInvertSelection_Click(object sender, EventArgs e)
        {
            if (this.TreeViewCoinNames.Nodes.Count > 0)
            {
                foreach (TreeNode aNode in this.TreeViewCoinNames.Nodes)
                {
                    if (aNode.Checked == true)
                    {
                        aNode.Checked = false;
                    }
                    else
                    {
                        aNode.Checked = true;
                    }
                }

                this.TreeViewCoinNames.SelectedNode = this.TreeViewCoinNames.Nodes[0];   // Force the ListBox to scroll back to the top of the list.
            }
        }

        private void ButtonGetCoinNames_Click(object sender, EventArgs e)
        {
            this.StoreSelectedItems(this.TreeViewCoinNames);   // First make a list of the checked coin names.
            this.GetCoinNames();                          // Load all coins.
        }

        private void StoreSelectedItems(TreeView Trv)
        {
            this.SelectedCoinNames.Clear();

            foreach (TreeNode aNode in Trv.Nodes)
            {
                if (aNode.Checked == true)  // At least one node is checked to start the filter
                {
                    this.SelectedCoinNames.Add(aNode.Text);
                }
            }
        }

        private void GetCoinNames()
        {
            if (StartSession.CheckForInternetConnection())  // First check if there is an active internet connection
            {

                MarketPrice AllCoinNames = new();    // create the coindata objectlist
                AllCoinNames.GetAllCoinNames();                 // Get the all the available coin names (read the API result)
                this.LoadCoinNames(AllCoinNames);                    // Load all the available coin names intot the treeview
                this.CheckCoinNames();
                this.SetAutoComplete();

                TreeNodeCollection nodes = this.TreeViewCoinNames.Nodes;
                this.CountCheckedNodes(nodes);   // Count the checked nodes
            }
        }

        private void LoadCoinNames(MarketPrice mp)
        {
            this.TreeViewCoinNames.Nodes.Clear();           // Remove all nodes
            this.TreeViewCoinNames.BeginUpdate();           // Suppress repainting the TreeView until all the objects have been created.
            foreach (object item in mp.CoinNames)     // Fill it again with only the filtered items
            {
                TreeNode aNode = new(item.ToString()) { Name = item.ToString() };
                this.TreeViewCoinNames.Nodes.Add(aNode);
            }

            this.TreeViewCoinNames.EndUpdate();
        }

        private void CheckCoinNames()
        {
            this.TreeViewCoinNames.BeginUpdate();

            // Get the coin names from the app database
            ApplicationDatabase CoinNames = new();
            List<string> AllCoinNames = CoinNames.GetSelectedCoinNames();

            if (AllCoinNames != null)
            {
                // Check the found coin names n the treeview
                TreeNodeCollection nodes = this.TreeViewCoinNames.Nodes;

                foreach (string Coin in AllCoinNames)
                {
                    string tmp = Coin;

                    foreach (TreeNode n in nodes)
                    {
                        // CheckTrvNode(n, Coin);
                        if (n.Text == Coin)
                        {
                            n.Checked = true;
                        }
                    }
                }
            }

            this.TreeViewCoinNames.EndUpdate();
        }

        private void ButtonCompressAppDatabase_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Logging.WriteToLogInformation("Het bestand " + AppSettingsDefault.SqlLiteDatabaseName + " wordt gecomprimeerd...");
            try
            {
                string argument = string.Empty;
                string DbLocation = this.JsonObjSettings.AppParam[0].DatabaseLocation;
                ApplicationDatabase compress = new();

                // Copy the database file before compress takes place
                if (compress.CopyDatabaseFile(string.Empty))
                {
                    compress.CompressDatabase();
                    Logging.WriteToLogInformation("Het bestand " + AppSettingsDefault.SqlLiteDatabaseName + " is succesvol gecomprimeerd.");

                    MessageBox.Show("De applicatie database is succesvol gecomprimeerd.", "Informatie.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Logging.WriteToLogInformation("Onverwachte fout opgetreden bij het comprimeren van '" + AppSettingsDefault.SqlLiteDatabaseName + "'.");
                Logging.WriteToLogInformation("Melding:");
                Logging.WriteToLogInformation(ex.Message);

                if (CmDebugMode.DebugMode)
                {
                    Logging.WriteToLogDebug(ex.ToString());
                }

                Cursor.Current = Cursors.Default;
                MessageBox.Show(
                    "Fout opgetreden bij het comprimeren van " + AppSettingsDefault.SqlLiteDatabaseName + "." + Environment.NewLine +
                    Environment.NewLine + "Raadpleeg het log bestand.",
                    "Fout bij comprimeren bestand.",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void TextBoxRateLimit_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.KeyPresstextBox(sender, e);
        }

        private void KeyPresstextBox(object sender, KeyPressEventArgs e)
        {
            // TODO: this is double code with main form
            char seperator = this.DecimalSeperator[0];

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                    (e.KeyChar != seperator))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == seperator) && ((sender as TextBox).Text.IndexOf(seperator) > -1))
            {
                e.Handled = true;
            }
        }

        private void TextBoxRateLimit_Leave(object sender, EventArgs e)
        {
            this.JsonObjSettings.AppParam[0].RateLimit = Convert.ToDouble(this.TextBoxRateLimit.Text);
        }

        private void TextBoxRateLimit_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.TextBoxRateLimit.Text))
                    {
                if (Convert.ToDouble(this.TextBoxRateLimit.Text) > (double)0)
                {
                    if (Convert.ToDouble(this.TextBoxRateLimit.Text) < ((double)1 / (double)1000))  // Bitvavo excepts 1000 requests per minute
                    {
                        MessageBox.Show(
                            "0,001 is de laagste toegestane waarde." + Environment.NewLine +
                            "Dit zijn 1000 request per minuut. Dat is het maximum wat is toegestaan.",
                            "Let op",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        this.TextBoxRateLimit.Text = "0" + this.DecimalSeperator + "001";
                    }
                }
            }

            this.JsonObjSettings.AppParam[0].RateLimit = Convert.ToDouble(this.TextBoxRateLimit.Text);
        }

        private void TextBoxUrl1_Leave(object sender, EventArgs e)
        {
            this.JsonObjSettings.AppParam[0].Url1 = this.TextBoxUrl1.Text;
        }

        private void TextBoxUrl2_Leave(object sender, EventArgs e)
        {
            this.JsonObjSettings.AppParam[0].Url2 = this.TextBoxUrl2.Text;
        }

        private void TextBoxWarnPercentage_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.KeyPresstextBox(sender, e);
        }

        private void TextBoxWarnPercentage_TextChanged(object sender, EventArgs e)
        {
            this.JsonObjSettings.AppParam[0].WarnPercentage = Convert.ToDouble(this.TextBoxWarnPercentage.Text);
        }

        private int FoundTrvCoinsTextSearch;

        private void TextBoxSearchCoinName_TextChanged(object sender, EventArgs e)
        {
            if (this.TextBoxSearchCoinName.Text.Length > 0)
            {
                this.ButtonSearchCoinName.Enabled = true;
            }
            else
            {
                this.ButtonSearchCoinName.Enabled = false;
            }

            this.FoundTrvCoinsTextSearch = 0;
            TreeNodeCollection nodes = this.TreeViewCoinNames.Nodes;
            foreach (TreeNode n in nodes)
            {
                this.ColorTrvSearchNode(n, this.TextBoxSearchCoinName);
            }

            this.LabelCoinFound.Text = this.FoundTrvCoinsTextSearch.ToString(CultureInfo.InvariantCulture) + " st";
        }

        private void ColorTrvSearchNode(TreeNode treeNode, TextBox Tb)
        {
            if (treeNode == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(Tb.Text))
            {
                if (treeNode.Text.ToUpperInvariant().Contains(Tb.Text.ToUpperInvariant()))
                {
                    // string TrvFoundSearchColorString = JsonObjSettings.AppParam[0].TrvFoundSearchColor;
                    // int TrvFoundQuerySearchClr = int.Parse(TrvFoundSearchColorString, CultureInfo.InvariantCulture);
                    int TrvFoundSearchClr = this.JsonObjSettings.AppParam[0].TrvFoundSearchColor;
                    Color SelectColor = Color.FromArgb(TrvFoundSearchClr);

                    treeNode.BackColor = SelectColor;
                    this.FoundTrvCoinsTextSearch++;
                }
                else
                {
                    treeNode.BackColor = Color.White;
                }

                foreach (TreeNode tn in treeNode.Nodes)
                {
                    this.ColorTrvSearchNode(tn, Tb);
                }
            }
            else
            {
                treeNode.BackColor = Color.White;
                this.FoundTrvCoinsTextSearch = 0;

                foreach (TreeNode tn in treeNode.Nodes)
                {
                    this.ColorTrvSearchNode(tn, Tb);
                }
            }
        }

        readonly TreeViewSearch tvSearch = new();  // Refence to TreeViewSearch, outside a function otherwise find next does not work

        private void ButtonSearchCoinName_Click(object sender, EventArgs e)
        {
            this.tvSearch.SearchInTreeViewNodes(this.TreeViewCoinNames, this.TextBoxSearchCoinName.Text);
        }

        private void TreeViewCoinNames_Click(object sender, EventArgs e)
        {
            TreeNodeCollection nodes = this.TreeViewCoinNames.Nodes;
            this.CountCheckedNodes(nodes);
        }

        private void  CountCheckedNodes(TreeNodeCollection nodes)
        {
            this.CheckedTrvNodes = 0;

            foreach (TreeNode n in nodes)
            {
                if (n.Checked)
                {
                    this.CheckedTrvNodes++;
                }
            }

            if (this.CheckedTrvNodes > 0)
            {
                this.LabelCountCheckedTrvNodes.Text = "Aantal geselecteerd : " + this.CheckedTrvNodes.ToString();
            }
            else
            {
                this.LabelCountCheckedTrvNodes.Text = "Aantal geselecteerd : 0";
            }

            this.Refresh();
        }

        private void TreeViewCoinNames_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNodeCollection nodes = this.TreeViewCoinNames.Nodes;
            this.CountCheckedNodes(nodes);
        }

        private void ButtonSelectedOnly_Click(object sender, EventArgs e)
        {
            // Show only the selected items in the treeview
            for (int i = 0; i < this.TreeViewCoinNames.Nodes.Count; i++)
            {
                RemoveUncheckedNodes(this.TreeViewCoinNames);
            }

            Cursor.Current = Cursors.Default;
        }

        private static void RemoveUncheckedNodes(TreeView Trv)
        {
            var nodes = new Stack<TreeNode>(Trv.Nodes.Cast<TreeNode>());
            while (nodes.Count > 0)
            {
                var n = nodes.Pop();
                if (!n.Checked)
                {
                    if (n.Parent != null)
                    {
                        n.Parent.Nodes.Remove(n);
                    }
                    else
                    {
                        Trv.Nodes.Remove(n);
                    }
                }
                else
                {
                    foreach (TreeNode tn in n.Nodes)
                    {
                        nodes.Push(tn);
                    }
                }
            }
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
