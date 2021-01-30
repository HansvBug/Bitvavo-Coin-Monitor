
namespace CM
{
    partial class FormConfigure
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConfigure));
            this.GroupBoxLogFile = new System.Windows.Forms.GroupBox();
            this.CheckBoxActivateLogging = new System.Windows.Forms.CheckBox();
            this.TextBoxLocationLogFile = new System.Windows.Forms.TextBox();
            this.LabelLocationLogFile = new System.Windows.Forms.Label();
            this.LabelLocationSettingsFile = new System.Windows.Forms.Label();
            this.TextBoxLocationSettingsFile = new System.Windows.Forms.TextBox();
            this.CheckBoxAppenLogFile = new System.Windows.Forms.CheckBox();
            this.GroupBoxDbmaintenance = new System.Windows.Forms.GroupBox();
            this.LabelLocationAppDatabaseFile = new System.Windows.Forms.Label();
            this.CopyDatabaseIntervalTextBox = new System.Windows.Forms.TextBox();
            this.TextBoxLocationDatabaseFile = new System.Windows.Forms.TextBox();
            this.LabelCopyAppDb = new System.Windows.Forms.Label();
            this.ButtonCompressAppDatabase = new System.Windows.Forms.Button();
            this.LabelBuildDate = new System.Windows.Forms.Label();
            this.LabelVersion = new System.Windows.Forms.Label();
            this.ButtonGetCoinNames = new System.Windows.Forms.Button();
            this.TreeViewCoinNames = new System.Windows.Forms.TreeView();
            this.ButtonSelectAll = new System.Windows.Forms.Button();
            this.ButtonDeselectAll = new System.Windows.Forms.Button();
            this.ButtonInvertSelection = new System.Windows.Forms.Button();
            this.TabControl1 = new System.Windows.Forms.TabControl();
            this.TabPageCoins = new System.Windows.Forms.TabPage();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.ButtonClose = new System.Windows.Forms.Button();
            this.ButtonSelectedOnly = new System.Windows.Forms.Button();
            this.LabelCountCheckedTrvNodes = new System.Windows.Forms.Label();
            this.ButtonSearchCoinName = new System.Windows.Forms.Button();
            this.LabelCoinFound = new System.Windows.Forms.Label();
            this.TextBoxSearchCoinName = new System.Windows.Forms.TextBox();
            this.TabPageAppSettings = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.TabPageAPI = new System.Windows.Forms.TabPage();
            this.button2 = new System.Windows.Forms.Button();
            this.TextBoxWarnPercentage = new System.Windows.Forms.TextBox();
            this.LabelWarnPercentage = new System.Windows.Forms.Label();
            this.TextBoxRateLimit = new System.Windows.Forms.TextBox();
            this.LabelRateLimit = new System.Windows.Forms.Label();
            this.GroupBoxUrl = new System.Windows.Forms.GroupBox();
            this.CheckBoxEnableUrl = new System.Windows.Forms.CheckBox();
            this.TextBoxUrl1 = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.TextBoxUrl2 = new System.Windows.Forms.TextBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.GroupBoxLogFile.SuspendLayout();
            this.GroupBoxDbmaintenance.SuspendLayout();
            this.TabControl1.SuspendLayout();
            this.TabPageCoins.SuspendLayout();
            this.GroupBox1.SuspendLayout();
            this.TabPageAppSettings.SuspendLayout();
            this.TabPageAPI.SuspendLayout();
            this.GroupBoxUrl.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroupBoxLogFile
            // 
            resources.ApplyResources(this.GroupBoxLogFile, "GroupBoxLogFile");
            this.GroupBoxLogFile.Controls.Add(this.CheckBoxActivateLogging);
            this.GroupBoxLogFile.Controls.Add(this.TextBoxLocationLogFile);
            this.GroupBoxLogFile.Controls.Add(this.LabelLocationLogFile);
            this.GroupBoxLogFile.Controls.Add(this.LabelLocationSettingsFile);
            this.GroupBoxLogFile.Controls.Add(this.TextBoxLocationSettingsFile);
            this.GroupBoxLogFile.Controls.Add(this.CheckBoxAppenLogFile);
            this.GroupBoxLogFile.Name = "GroupBoxLogFile";
            this.GroupBoxLogFile.TabStop = false;
            // 
            // CheckBoxActivateLogging
            // 
            resources.ApplyResources(this.CheckBoxActivateLogging, "CheckBoxActivateLogging");
            this.CheckBoxActivateLogging.Name = "CheckBoxActivateLogging";
            this.CheckBoxActivateLogging.UseVisualStyleBackColor = true;
            this.CheckBoxActivateLogging.Click += new System.EventHandler(this.CheckBoxActivateLogging_Click);
            // 
            // TextBoxLocationLogFile
            // 
            resources.ApplyResources(this.TextBoxLocationLogFile, "TextBoxLocationLogFile");
            this.TextBoxLocationLogFile.Name = "TextBoxLocationLogFile";
            // 
            // LabelLocationLogFile
            // 
            resources.ApplyResources(this.LabelLocationLogFile, "LabelLocationLogFile");
            this.LabelLocationLogFile.Name = "LabelLocationLogFile";
            // 
            // LabelLocationSettingsFile
            // 
            resources.ApplyResources(this.LabelLocationSettingsFile, "LabelLocationSettingsFile");
            this.LabelLocationSettingsFile.Name = "LabelLocationSettingsFile";
            // 
            // TextBoxLocationSettingsFile
            // 
            resources.ApplyResources(this.TextBoxLocationSettingsFile, "TextBoxLocationSettingsFile");
            this.TextBoxLocationSettingsFile.Name = "TextBoxLocationSettingsFile";
            // 
            // CheckBoxAppenLogFile
            // 
            resources.ApplyResources(this.CheckBoxAppenLogFile, "CheckBoxAppenLogFile");
            this.CheckBoxAppenLogFile.Name = "CheckBoxAppenLogFile";
            this.CheckBoxAppenLogFile.UseVisualStyleBackColor = true;
            this.CheckBoxAppenLogFile.Click += new System.EventHandler(this.CheckBoxAppenLogFile_Click);
            // 
            // GroupBoxDbmaintenance
            // 
            resources.ApplyResources(this.GroupBoxDbmaintenance, "GroupBoxDbmaintenance");
            this.GroupBoxDbmaintenance.Controls.Add(this.LabelLocationAppDatabaseFile);
            this.GroupBoxDbmaintenance.Controls.Add(this.CopyDatabaseIntervalTextBox);
            this.GroupBoxDbmaintenance.Controls.Add(this.TextBoxLocationDatabaseFile);
            this.GroupBoxDbmaintenance.Controls.Add(this.LabelCopyAppDb);
            this.GroupBoxDbmaintenance.Controls.Add(this.ButtonCompressAppDatabase);
            this.GroupBoxDbmaintenance.Name = "GroupBoxDbmaintenance";
            this.GroupBoxDbmaintenance.TabStop = false;
            // 
            // LabelLocationAppDatabaseFile
            // 
            resources.ApplyResources(this.LabelLocationAppDatabaseFile, "LabelLocationAppDatabaseFile");
            this.LabelLocationAppDatabaseFile.Name = "LabelLocationAppDatabaseFile";
            // 
            // CopyDatabaseIntervalTextBox
            // 
            resources.ApplyResources(this.CopyDatabaseIntervalTextBox, "CopyDatabaseIntervalTextBox");
            this.CopyDatabaseIntervalTextBox.Name = "CopyDatabaseIntervalTextBox";
            this.CopyDatabaseIntervalTextBox.TextChanged += new System.EventHandler(this.CopyDatabaseIntervalTextBox_TextChanged);
            // 
            // TextBoxLocationDatabaseFile
            // 
            resources.ApplyResources(this.TextBoxLocationDatabaseFile, "TextBoxLocationDatabaseFile");
            this.TextBoxLocationDatabaseFile.Name = "TextBoxLocationDatabaseFile";
            // 
            // LabelCopyAppDb
            // 
            resources.ApplyResources(this.LabelCopyAppDb, "LabelCopyAppDb");
            this.LabelCopyAppDb.Name = "LabelCopyAppDb";
            // 
            // ButtonCompressAppDatabase
            // 
            resources.ApplyResources(this.ButtonCompressAppDatabase, "ButtonCompressAppDatabase");
            this.ButtonCompressAppDatabase.Name = "ButtonCompressAppDatabase";
            this.ButtonCompressAppDatabase.UseVisualStyleBackColor = true;
            this.ButtonCompressAppDatabase.Click += new System.EventHandler(this.ButtonCompressAppDatabase_Click);
            // 
            // LabelBuildDate
            // 
            resources.ApplyResources(this.LabelBuildDate, "LabelBuildDate");
            this.LabelBuildDate.Name = "LabelBuildDate";
            // 
            // LabelVersion
            // 
            resources.ApplyResources(this.LabelVersion, "LabelVersion");
            this.LabelVersion.Name = "LabelVersion";
            // 
            // ButtonGetCoinNames
            // 
            resources.ApplyResources(this.ButtonGetCoinNames, "ButtonGetCoinNames");
            this.ButtonGetCoinNames.Name = "ButtonGetCoinNames";
            this.ButtonGetCoinNames.UseVisualStyleBackColor = true;
            this.ButtonGetCoinNames.Click += new System.EventHandler(this.ButtonGetCoinNames_Click);
            // 
            // TreeViewCoinNames
            // 
            resources.ApplyResources(this.TreeViewCoinNames, "TreeViewCoinNames");
            this.TreeViewCoinNames.CheckBoxes = true;
            this.TreeViewCoinNames.Name = "TreeViewCoinNames";
            this.TreeViewCoinNames.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewCoinNames_AfterSelect);
            this.TreeViewCoinNames.Click += new System.EventHandler(this.TreeViewCoinNames_Click);
            // 
            // ButtonSelectAll
            // 
            resources.ApplyResources(this.ButtonSelectAll, "ButtonSelectAll");
            this.ButtonSelectAll.Name = "ButtonSelectAll";
            this.ButtonSelectAll.UseVisualStyleBackColor = true;
            this.ButtonSelectAll.Click += new System.EventHandler(this.ButtonSelectAll_Click);
            // 
            // ButtonDeselectAll
            // 
            resources.ApplyResources(this.ButtonDeselectAll, "ButtonDeselectAll");
            this.ButtonDeselectAll.Name = "ButtonDeselectAll";
            this.ButtonDeselectAll.UseVisualStyleBackColor = true;
            this.ButtonDeselectAll.Click += new System.EventHandler(this.ButtonDeselectAll_Click);
            // 
            // ButtonInvertSelection
            // 
            resources.ApplyResources(this.ButtonInvertSelection, "ButtonInvertSelection");
            this.ButtonInvertSelection.Name = "ButtonInvertSelection";
            this.ButtonInvertSelection.UseVisualStyleBackColor = true;
            this.ButtonInvertSelection.Click += new System.EventHandler(this.ButtonInvertSelection_Click);
            // 
            // TabControl1
            // 
            resources.ApplyResources(this.TabControl1, "TabControl1");
            this.TabControl1.Controls.Add(this.TabPageCoins);
            this.TabControl1.Controls.Add(this.TabPageAppSettings);
            this.TabControl1.Controls.Add(this.TabPageAPI);
            this.TabControl1.Name = "TabControl1";
            this.TabControl1.SelectedIndex = 0;
            // 
            // TabPageCoins
            // 
            resources.ApplyResources(this.TabPageCoins, "TabPageCoins");
            this.TabPageCoins.Controls.Add(this.GroupBox1);
            this.TabPageCoins.Name = "TabPageCoins";
            this.TabPageCoins.UseVisualStyleBackColor = true;
            // 
            // GroupBox1
            // 
            resources.ApplyResources(this.GroupBox1, "GroupBox1");
            this.GroupBox1.Controls.Add(this.ButtonClose);
            this.GroupBox1.Controls.Add(this.ButtonSelectedOnly);
            this.GroupBox1.Controls.Add(this.LabelCountCheckedTrvNodes);
            this.GroupBox1.Controls.Add(this.ButtonSearchCoinName);
            this.GroupBox1.Controls.Add(this.LabelCoinFound);
            this.GroupBox1.Controls.Add(this.TextBoxSearchCoinName);
            this.GroupBox1.Controls.Add(this.ButtonGetCoinNames);
            this.GroupBox1.Controls.Add(this.ButtonInvertSelection);
            this.GroupBox1.Controls.Add(this.TreeViewCoinNames);
            this.GroupBox1.Controls.Add(this.ButtonDeselectAll);
            this.GroupBox1.Controls.Add(this.ButtonSelectAll);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.TabStop = false;
            // 
            // ButtonClose
            // 
            resources.ApplyResources(this.ButtonClose, "ButtonClose");
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.UseVisualStyleBackColor = true;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // ButtonSelectedOnly
            // 
            resources.ApplyResources(this.ButtonSelectedOnly, "ButtonSelectedOnly");
            this.ButtonSelectedOnly.Name = "ButtonSelectedOnly";
            this.ButtonSelectedOnly.UseVisualStyleBackColor = true;
            this.ButtonSelectedOnly.Click += new System.EventHandler(this.ButtonSelectedOnly_Click);
            // 
            // LabelCountCheckedTrvNodes
            // 
            resources.ApplyResources(this.LabelCountCheckedTrvNodes, "LabelCountCheckedTrvNodes");
            this.LabelCountCheckedTrvNodes.Name = "LabelCountCheckedTrvNodes";
            // 
            // ButtonSearchCoinName
            // 
            resources.ApplyResources(this.ButtonSearchCoinName, "ButtonSearchCoinName");
            this.ButtonSearchCoinName.Name = "ButtonSearchCoinName";
            this.ButtonSearchCoinName.UseVisualStyleBackColor = true;
            this.ButtonSearchCoinName.Click += new System.EventHandler(this.ButtonSearchCoinName_Click);
            // 
            // LabelCoinFound
            // 
            resources.ApplyResources(this.LabelCoinFound, "LabelCoinFound");
            this.LabelCoinFound.Name = "LabelCoinFound";
            // 
            // TextBoxSearchCoinName
            // 
            resources.ApplyResources(this.TextBoxSearchCoinName, "TextBoxSearchCoinName");
            this.TextBoxSearchCoinName.Name = "TextBoxSearchCoinName";
            this.TextBoxSearchCoinName.TextChanged += new System.EventHandler(this.TextBoxSearchCoinName_TextChanged);
            // 
            // TabPageAppSettings
            // 
            resources.ApplyResources(this.TabPageAppSettings, "TabPageAppSettings");
            this.TabPageAppSettings.Controls.Add(this.button1);
            this.TabPageAppSettings.Controls.Add(this.GroupBoxLogFile);
            this.TabPageAppSettings.Controls.Add(this.GroupBoxDbmaintenance);
            this.TabPageAppSettings.Controls.Add(this.LabelBuildDate);
            this.TabPageAppSettings.Controls.Add(this.LabelVersion);
            this.TabPageAppSettings.Name = "TabPageAppSettings";
            this.TabPageAppSettings.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // TabPageAPI
            // 
            resources.ApplyResources(this.TabPageAPI, "TabPageAPI");
            this.TabPageAPI.Controls.Add(this.button2);
            this.TabPageAPI.Controls.Add(this.TextBoxWarnPercentage);
            this.TabPageAPI.Controls.Add(this.LabelWarnPercentage);
            this.TabPageAPI.Controls.Add(this.TextBoxRateLimit);
            this.TabPageAPI.Controls.Add(this.LabelRateLimit);
            this.TabPageAPI.Controls.Add(this.GroupBoxUrl);
            this.TabPageAPI.Name = "TabPageAPI";
            this.TabPageAPI.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // TextBoxWarnPercentage
            // 
            resources.ApplyResources(this.TextBoxWarnPercentage, "TextBoxWarnPercentage");
            this.TextBoxWarnPercentage.Name = "TextBoxWarnPercentage";
            this.TextBoxWarnPercentage.TextChanged += new System.EventHandler(this.TextBoxWarnPercentage_TextChanged);
            this.TextBoxWarnPercentage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBoxWarnPercentage_KeyPress);
            // 
            // LabelWarnPercentage
            // 
            resources.ApplyResources(this.LabelWarnPercentage, "LabelWarnPercentage");
            this.LabelWarnPercentage.Name = "LabelWarnPercentage";
            // 
            // TextBoxRateLimit
            // 
            resources.ApplyResources(this.TextBoxRateLimit, "TextBoxRateLimit");
            this.TextBoxRateLimit.Name = "TextBoxRateLimit";
            this.TextBoxRateLimit.TextChanged += new System.EventHandler(this.TextBoxRateLimit_TextChanged);
            this.TextBoxRateLimit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBoxRateLimit_KeyPress);
            this.TextBoxRateLimit.Leave += new System.EventHandler(this.TextBoxRateLimit_Leave);
            // 
            // LabelRateLimit
            // 
            resources.ApplyResources(this.LabelRateLimit, "LabelRateLimit");
            this.LabelRateLimit.Name = "LabelRateLimit";
            // 
            // GroupBoxUrl
            // 
            resources.ApplyResources(this.GroupBoxUrl, "GroupBoxUrl");
            this.GroupBoxUrl.Controls.Add(this.CheckBoxEnableUrl);
            this.GroupBoxUrl.Controls.Add(this.TextBoxUrl1);
            this.GroupBoxUrl.Controls.Add(this.Label1);
            this.GroupBoxUrl.Controls.Add(this.Label2);
            this.GroupBoxUrl.Controls.Add(this.TextBoxUrl2);
            this.GroupBoxUrl.Name = "GroupBoxUrl";
            this.GroupBoxUrl.TabStop = false;
            // 
            // CheckBoxEnableUrl
            // 
            resources.ApplyResources(this.CheckBoxEnableUrl, "CheckBoxEnableUrl");
            this.CheckBoxEnableUrl.Name = "CheckBoxEnableUrl";
            this.CheckBoxEnableUrl.UseVisualStyleBackColor = true;
            this.CheckBoxEnableUrl.Click += new System.EventHandler(this.CheckBoxEnableUrl_Click);
            // 
            // TextBoxUrl1
            // 
            resources.ApplyResources(this.TextBoxUrl1, "TextBoxUrl1");
            this.TextBoxUrl1.Name = "TextBoxUrl1";
            this.TextBoxUrl1.TextChanged += new System.EventHandler(this.TextBoxUrl1_Leave);
            this.TextBoxUrl1.Leave += new System.EventHandler(this.TextBoxUrl1_Leave);
            // 
            // Label1
            // 
            resources.ApplyResources(this.Label1, "Label1");
            this.Label1.Name = "Label1";
            // 
            // Label2
            // 
            resources.ApplyResources(this.Label2, "Label2");
            this.Label2.Name = "Label2";
            // 
            // TextBoxUrl2
            // 
            resources.ApplyResources(this.TextBoxUrl2, "TextBoxUrl2");
            this.TextBoxUrl2.Name = "TextBoxUrl2";
            this.TextBoxUrl2.TextChanged += new System.EventHandler(this.TextBoxUrl2_Leave);
            this.TextBoxUrl2.Leave += new System.EventHandler(this.TextBoxUrl2_Leave);
            // 
            // FormConfigure
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormConfigure";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormConfigure_FormClosing);
            this.Load += new System.EventHandler(this.FormConfigure_Load);
            this.GroupBoxLogFile.ResumeLayout(false);
            this.GroupBoxLogFile.PerformLayout();
            this.GroupBoxDbmaintenance.ResumeLayout(false);
            this.GroupBoxDbmaintenance.PerformLayout();
            this.TabControl1.ResumeLayout(false);
            this.TabPageCoins.ResumeLayout(false);
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            this.TabPageAppSettings.ResumeLayout(false);
            this.TabPageAppSettings.PerformLayout();
            this.TabPageAPI.ResumeLayout(false);
            this.TabPageAPI.PerformLayout();
            this.GroupBoxUrl.ResumeLayout(false);
            this.GroupBoxUrl.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox GroupBoxLogFile;
        private System.Windows.Forms.CheckBox CheckBoxActivateLogging;
        private System.Windows.Forms.TextBox TextBoxLocationLogFile;
        private System.Windows.Forms.Label LabelLocationLogFile;
        private System.Windows.Forms.Label LabelLocationSettingsFile;
        private System.Windows.Forms.TextBox TextBoxLocationSettingsFile;
        private System.Windows.Forms.CheckBox CheckBoxAppenLogFile;
        private System.Windows.Forms.GroupBox GroupBoxDbmaintenance;
        private System.Windows.Forms.TextBox CopyDatabaseIntervalTextBox;
        private System.Windows.Forms.Label LabelCopyAppDb;
        private System.Windows.Forms.Button ButtonCompressAppDatabase;
        private System.Windows.Forms.Label LabelBuildDate;
        private System.Windows.Forms.Label LabelVersion;
        private System.Windows.Forms.TextBox TextBoxLocationDatabaseFile;
        private System.Windows.Forms.Label LabelLocationAppDatabaseFile;
        private System.Windows.Forms.Button ButtonGetCoinNames;
        private System.Windows.Forms.TreeView TreeViewCoinNames;
        private System.Windows.Forms.Button ButtonSelectAll;
        private System.Windows.Forms.Button ButtonDeselectAll;
        private System.Windows.Forms.Button ButtonInvertSelection;
        private System.Windows.Forms.TabControl TabControl1;
        private System.Windows.Forms.TabPage TabPageCoins;
        private System.Windows.Forms.GroupBox GroupBox1;
        private System.Windows.Forms.TabPage TabPageAppSettings;
        private System.Windows.Forms.TabPage TabPageAPI;
        private System.Windows.Forms.GroupBox GroupBoxUrl;
        private System.Windows.Forms.CheckBox CheckBoxEnableUrl;
        private System.Windows.Forms.TextBox TextBoxUrl1;
        private System.Windows.Forms.Label Label1;
        private System.Windows.Forms.Label Label2;
        private System.Windows.Forms.TextBox TextBoxUrl2;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TextBox TextBoxRateLimit;
        private System.Windows.Forms.Label LabelRateLimit;
        private System.Windows.Forms.TextBox TextBoxWarnPercentage;
        private System.Windows.Forms.Label LabelWarnPercentage;
        private System.Windows.Forms.Label LabelCoinFound;
        private System.Windows.Forms.TextBox TextBoxSearchCoinName;
        private System.Windows.Forms.Button ButtonSearchCoinName;
        private System.Windows.Forms.Label LabelCountCheckedTrvNodes;
        private System.Windows.Forms.Button ButtonSelectedOnly;
        private System.Windows.Forms.Button ButtonClose;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}