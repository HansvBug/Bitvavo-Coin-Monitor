using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Forms;
using HvBLogging;

namespace CM
{
    public partial class FormConfigure : Form
    {
        #region Properties

        public bool DebugMode { get; set; }
        public dynamic JsonObjSettings { get; set; }
        private int CopyAppDataBaseAfterEveryXStartups { get; set; }
        private string DecimalSeperator { get; set; }
        #endregion Properties

        public FormConfigure()
        {
            InitializeComponent();
            DecimalSeperator = CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator;
            TextBoxSearchCoinName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        }

        #region load form
        private void FormConfigure_Load(object sender, EventArgs e)
        {
            LoadSettings();
            ApplySettings();
            LoadFormPosition();
            GetCoinNames();            
        }
        private void SetAutoComplete()
        {
            using (AutoComplete aCompleteSource = new AutoComplete())
            {
                AutoCompleteStringCollection DataCollection;
                DataCollection = aCompleteSource.CreAutoCompleteListFromTrv(TreeViewCoinNames);  //Create the autocomplete list for the search box

                TextBoxSearchCoinName.AutoCompleteSource = AutoCompleteSource.CustomSource;
                TextBoxSearchCoinName.AutoCompleteCustomSource = DataCollection;
            }
        }
        private void LoadSettings()
        {
            using SettingsManager Set = new();
            Set.LoadSettings();
            JsonObjSettings = Set.JsonObjSettings;
        }
        private void ApplySettings()
        {
            if (JsonObjSettings.AppParam[0].AppendLogFile)
            {
                CheckBoxAppenLogFile.Checked = true;
            }
            else
            {
                CheckBoxAppenLogFile.Checked = false;
            }

            if (JsonObjSettings.AppParam[0].ActivateLogging)
            {
                CheckBoxActivateLogging.Checked = true;
            }
            else
            {
                CheckBoxActivateLogging.Checked = false;
            }

            TextBoxLocationSettingsFile.Text = JsonObjSettings.AppParam[0].SettingsFileLocation;
            TextBoxLocationLogFile.Text = JsonObjSettings.AppParam[0].LogFileLocation + AppSettingsDefault.LogFileName;
            TextBoxLocationDatabaseFile.Text = JsonObjSettings.AppParam[0].DatabaseLocation + AppSettingsDefault.SqlLiteDatabaseName;

            LabelVersion.Text = "Versie : " + JsonObjSettings.AppParam[0].ApplicationVersion;
            LabelBuildDate.Text = "Build datum : " + JsonObjSettings.AppParam[0].ApplicationBuildDate;

            
            int CopyAppDataBaseAfterEveryXStartups = JsonObjSettings.AppParam[0].CopyAppDataBaseAfterEveryXStartups;
            CopyDatabaseIntervalTextBox.Text = CopyAppDataBaseAfterEveryXStartups.ToString();

            TextBoxUrl1.Text = JsonObjSettings.AppParam[0].Url1;
            TextBoxUrl2.Text = JsonObjSettings.AppParam[0].Url2;

            TextBoxRateLimit.Text = JsonObjSettings.AppParam[0].RateLimit.ToString();
            TextBoxWarnPercentage.Text = JsonObjSettings.AppParam[0].WarnPercentage.ToString();

        }
        private void LoadFormPosition()
        {
            using FormPosition FormPosition = new(this);
            FormPosition.LoadConfigureFormPosition();
        }
        #endregion load form

        private void CheckBoxEnableUrl_Click(object sender, EventArgs e)
        {
            if (CheckBoxEnableUrl.Checked)
            {
                TextBoxUrl1.Enabled = true;
                TextBoxUrl2.Enabled = true;
            }
            else
            {
                TextBoxUrl1.Enabled = false;
                TextBoxUrl2.Enabled = false;
            }
        }

        #region Form closing
        private void FormConfigure_FormClosing(object sender, FormClosingEventArgs e)
        {
            //TODO coins opslaanin SQLlite
            Logging.WriteToLogInformation("Sluiten configuratie scherm.");
            SaveFormPosition();
            SaveSettings();
            SaveCheckedCoins();
            CreateTables();
        }
        private void SaveFormPosition()
        {
            using FormPosition FormPosition = new(this)
            {
                DebugMode = this.DebugMode
            };
            FormPosition.SaveConfigureFormPosition();
        }
        private void SaveSettings()
        {
            SettingsManager.SaveSettings(JsonObjSettings);
        }
        private void SaveCheckedCoins()
        {
            List<string> CoinNames = new List<string>();
            ApplicationDatabase addCoin = new();
            foreach (TreeNode aNode in TreeViewCoinNames.Nodes) //first create a list withe the coin names
            {
                if (aNode.Checked == true)  //at least one node must checked to start the filter
                {
                    CoinNames.Add(aNode.Name);
                }
            }
            if (CoinNames.Count > 0)  //if any coin name is checked then save it
            {
                addCoin.SaveCoinNames(CoinNames);
            }
        }
        private void CreateTables()
        {
            List<string> CoinNames = new List<string>();
            ApplicationDatabase addCoin = new();
            
            foreach (TreeNode aNode in TreeViewCoinNames.Nodes) //first create a list withe the coin names
            {
                if (aNode.Checked == true)  //at least one node must checked to start the filter
                {
                    CoinNames.Add(aNode.Name);
                }
            }
            if (CoinNames.Count > 0)  //if any coin name is checked then save it
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
            if (CheckBoxActivateLogging.Checked)
            {
                CheckBoxAppenLogFile.Enabled = true;

                JsonObjSettings.AppParam[0].ActivateLogging = true;

                Logging.ActivateLogging = true;
                Logging.StartLogging();
                Logging.WriteToLogInformation("Logging aangezet.");
            }
            else
            {
                CheckBoxAppenLogFile.Checked = false;
                CheckBoxAppenLogFile.Enabled = false;

                JsonObjSettings.AppParam[0].ActivateLogging = false;
                JsonObjSettings.AppParam[0].AppendLogFile = false;

                Logging.WriteToLogInformation("Logging uitgezet.");
                Logging.StopLogging();
                Logging.ActivateLogging = false;
            }
        }

        private void CheckBoxAppenLogFile_Click(object sender, EventArgs e)
        {
            if (CheckBoxAppenLogFile.Checked)
            {
                JsonObjSettings.AppParam[0].AppendLogFile = true;
                Logging.WriteToLogInformation("Logging aanvullen is aangezet.");
            }
            else
            {
                JsonObjSettings.AppParam[0].AppendLogFile = false;
                Logging.WriteToLogInformation("Logging aanvullen is uitgezet.");
            }
        }

        private void CopyDatabaseIntervalTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CopyAppDataBaseAfterEveryXStartups = Int32.Parse(CopyDatabaseIntervalTextBox.Text, CultureInfo.InvariantCulture);
                if (CopyAppDataBaseAfterEveryXStartups > 200)
                {
                    CopyAppDataBaseAfterEveryXStartups = 200;
                    CopyDatabaseIntervalTextBox.Text = "200";
                    MessageBox.Show("De maximale waarde is 200", "Informatie", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                JsonObjSettings.AppParam[0].CopyAppDataBaseAfterEveryXStartups = CopyAppDataBaseAfterEveryXStartups;
            }
            catch (FormatException ex)
            {
                Logging.WriteToLogError("Fout opgetreden bij het omzetten van een string naar een integer.");
                Logging.WriteToLogError("Melding:");
                Logging.WriteToLogError(ex.Message);
                CopyDatabaseIntervalTextBox.Text = "0";
            }
        }

        private void ButtonSelectAll_Click(object sender, EventArgs e)
        {
            foreach (TreeNode node in TreeViewCoinNames.Nodes)  //29-12-2017; hvb checklistbox replaced by  treeview
            {
                node.Checked = true;
            }
        }

        private void ButtonDeselectAll_Click(object sender, EventArgs e)
        {
            foreach (TreeNode node in TreeViewCoinNames.Nodes)
            {
                node.Checked = false;
            }
        }

        private void ButtonInvertSelection_Click(object sender, EventArgs e)
        {
            if (TreeViewCoinNames.Nodes.Count > 0)
            {
                foreach (TreeNode aNode in TreeViewCoinNames.Nodes)
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
                TreeViewCoinNames.SelectedNode = TreeViewCoinNames.Nodes[0];   //  Force the ListBox to scroll back to the top of the list.
            }
        }

        private void ButtonGetCoinNames_Click(object sender, EventArgs e)
        {
            GetCoinNames();
        }
        private void GetCoinNames()
        {
            if (StartSession.CheckForInternetConnection())  // First check if there is an active internet connection
            {

                MarketPrice AllCoinNames = new MarketPrice();    //create the coindata objectlist
                AllCoinNames.GetAllCoinNames();
                LoadCoinNames(AllCoinNames);
                CheckCoinNames();
                SetAutoComplete();
            }
        }
        private void LoadCoinNames(MarketPrice mp)
        {
            TreeViewCoinNames.Nodes.Clear();           //Remove all nodes            
            TreeViewCoinNames.BeginUpdate();           //Suppress repainting the TreeView until all the objects have been created.
            foreach (object item in mp.CoinNames)     //Fill it again with only the filtered items
            {
                TreeNode aNode = new TreeNode(item.ToString()) { Name = item.ToString() };
                TreeViewCoinNames.Nodes.Add(aNode);
            }
            TreeViewCoinNames.EndUpdate();
        }
        private void CheckCoinNames()
        {
            TreeViewCoinNames.BeginUpdate();

            //Get the coin names from the app database
            ApplicationDatabase CoinNames = new();
            List<string> AllCoinNames = CoinNames.GetCoinNames();

            if (AllCoinNames != null)
            {
                //Check the found coin names n the treeview
                TreeNodeCollection nodes = TreeViewCoinNames.Nodes;

                foreach (string Coin in AllCoinNames)
                {
                    string tmp = Coin;

                    foreach (TreeNode n in nodes)
                    {
                        //CheckTrvNode(n, Coin);
                        if (n.Text == Coin)
                        {
                            n.Checked = true;
                        }
                    }
                }
            }
            TreeViewCoinNames.EndUpdate();
        }


        private void ButtonCompressAppDatabase_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Logging.WriteToLogInformation("Het bestand " + AppSettingsDefault.SqlLiteDatabaseName + " wordt gecomprimeerd...");
            try
            {
                string argument = string.Empty;
                string DbLocation = JsonObjSettings.AppParam[0].DatabaseLocation;
                ApplicationDatabase compress = new()
                {
                    DebugMode = this.DesignMode
                };

                if (compress.CopyDatabaseFile(string.Empty))    //Copy the database file before compress takes place
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
                if (this.DebugMode) { Logging.WriteToLogDebug(ex.ToString()); }
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Fout opgetreden bij het comprimeren van " + AppSettingsDefault.SqlLiteDatabaseName + "." + Environment.NewLine +
                                Environment.NewLine + "Raadpleeg het log bestand."
                                , "Fout bij comprimeren bestand.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TextBoxRateLimit_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeyPresstextBox(sender, e);
        }
        private void KeyPresstextBox(object sender, KeyPressEventArgs e)
        {   //TODO: this is double code with main form
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
            JsonObjSettings.AppParam[0].RateLimit = Convert.ToDouble(TextBoxRateLimit.Text);
        }

        private void TextBoxRateLimit_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TextBoxRateLimit.Text))
                    {
                if (Convert.ToDouble(TextBoxRateLimit.Text) > (double)0)
                {
                    if (Convert.ToDouble(TextBoxRateLimit.Text) < ((double)1 / (double)1000))  //Bitvavo excepts 1000 requests per minute
                    {
                        MessageBox.Show("0,001 is de laagste toegestane waarde." + Environment.NewLine +
                            "Dit zijn 1000 request per minuut. Dat is het maximum wat is toegestaan."
                            , "Let op", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        TextBoxRateLimit.Text = "0" + DecimalSeperator + "001";
                    }
                }                
            }
            JsonObjSettings.AppParam[0].RateLimit = Convert.ToDouble(TextBoxRateLimit.Text);
        }

        private void TextBoxUrl1_Leave(object sender, EventArgs e)
        {
            JsonObjSettings.AppParam[0].Url1 = TextBoxUrl1.Text;            
        }

        private void TextBoxUrl2_Leave(object sender, EventArgs e)
        {
            JsonObjSettings.AppParam[0].Url2 = TextBoxUrl2.Text;
        }

        private void TextBoxWarnPercentage_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeyPresstextBox(sender, e);
        }

        private void TextBoxWarnPercentage_TextChanged(object sender, EventArgs e)
        {
            JsonObjSettings.AppParam[0].WarnPercentage = Convert.ToDouble(TextBoxWarnPercentage.Text);
        }

        private int FoundTrvCoinsTextSearch;
        private void TextBoxSearchCoinName_TextChanged(object sender, EventArgs e)
        {
            FoundTrvCoinsTextSearch = 0;
            TreeNodeCollection nodes = TreeViewCoinNames.Nodes;
            foreach (TreeNode n in nodes)
            {
                ColorTrvSearchNode(n, TextBoxSearchCoinName);
            }
            LabelCoinFound.Text = FoundTrvCoinsTextSearch.ToString(CultureInfo.InvariantCulture) + " st";
        }
        private void ColorTrvSearchNode(TreeNode treeNode, TextBox Tb)
        {
            if (treeNode == null)
            {
                return;
            }

            if (!String.IsNullOrEmpty(Tb.Text))
            {
                if (treeNode.Text.ToUpperInvariant().Contains(Tb.Text.ToUpperInvariant()))
                {
                    //string TrvFoundSearchColorString = JsonObjSettings.AppParam[0].TrvFoundSearchColor;
                    //int TrvFoundQuerySearchClr = int.Parse(TrvFoundSearchColorString, CultureInfo.InvariantCulture);

                    int TrvFoundSearchClr = JsonObjSettings.AppParam[0].TrvFoundSearchColor;
                    Color SelectColor = Color.FromArgb(TrvFoundSearchClr);

                    treeNode.BackColor = SelectColor;
                    FoundTrvCoinsTextSearch++;                    
                }
                else
                {
                    treeNode.BackColor = Color.White;
                }

                foreach (TreeNode tn in treeNode.Nodes)
                {
                    ColorTrvSearchNode(tn, Tb);
                }
            }
            else
            {
                treeNode.BackColor = Color.White;
                FoundTrvCoinsTextSearch = 0;

                foreach (TreeNode tn in treeNode.Nodes)
                {
                    ColorTrvSearchNode(tn, Tb);
                }
            }
        }

        readonly TreeViewSearch tvSearch = new TreeViewSearch();  //refence to TreeViewSearch, outside a function otherwise find next does not work
        private void ButtonSearchCoinName_Click(object sender, EventArgs e)
        {
            tvSearch.SearchInTreeViewNodes(TreeViewCoinNames, TextBoxSearchCoinName.Text);
        }
    }
}
