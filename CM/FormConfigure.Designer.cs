
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
            this.ButtonSearchCoinName = new System.Windows.Forms.Button();
            this.LabelCoinFound = new System.Windows.Forms.Label();
            this.TextBoxSearchCoinName = new System.Windows.Forms.TextBox();
            this.TabPageAppSettings = new System.Windows.Forms.TabPage();
            this.TabPageAPI = new System.Windows.Forms.TabPage();
            this.TextBoxWarnPercentage = new System.Windows.Forms.TextBox();
            this.LabelWarnPercentage = new System.Windows.Forms.Label();
            this.TextBoxRateLimit = new System.Windows.Forms.TextBox();
            this.LabelRateLimit = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
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
            this.GroupBoxLogFile.Controls.Add(this.CheckBoxActivateLogging);
            this.GroupBoxLogFile.Controls.Add(this.TextBoxLocationLogFile);
            this.GroupBoxLogFile.Controls.Add(this.LabelLocationLogFile);
            this.GroupBoxLogFile.Controls.Add(this.LabelLocationSettingsFile);
            this.GroupBoxLogFile.Controls.Add(this.TextBoxLocationSettingsFile);
            this.GroupBoxLogFile.Controls.Add(this.CheckBoxAppenLogFile);
            this.GroupBoxLogFile.Location = new System.Drawing.Point(8, 8);
            this.GroupBoxLogFile.Name = "GroupBoxLogFile";
            this.GroupBoxLogFile.Size = new System.Drawing.Size(665, 154);
            this.GroupBoxLogFile.TabIndex = 13;
            this.GroupBoxLogFile.TabStop = false;
            this.GroupBoxLogFile.Text = "Log bestand";
            // 
            // CheckBoxActivateLogging
            // 
            this.CheckBoxActivateLogging.AutoSize = true;
            this.CheckBoxActivateLogging.Location = new System.Drawing.Point(6, 22);
            this.CheckBoxActivateLogging.Name = "CheckBoxActivateLogging";
            this.CheckBoxActivateLogging.Size = new System.Drawing.Size(113, 19);
            this.CheckBoxActivateLogging.TabIndex = 6;
            this.CheckBoxActivateLogging.Text = "Activeer logging";
            this.CheckBoxActivateLogging.UseVisualStyleBackColor = true;
            this.CheckBoxActivateLogging.Click += new System.EventHandler(this.CheckBoxActivateLogging_Click);
            // 
            // TextBoxLocationLogFile
            // 
            this.TextBoxLocationLogFile.Enabled = false;
            this.TextBoxLocationLogFile.Location = new System.Drawing.Point(169, 114);
            this.TextBoxLocationLogFile.Name = "TextBoxLocationLogFile";
            this.TextBoxLocationLogFile.Size = new System.Drawing.Size(482, 23);
            this.TextBoxLocationLogFile.TabIndex = 11;
            // 
            // LabelLocationLogFile
            // 
            this.LabelLocationLogFile.AutoSize = true;
            this.LabelLocationLogFile.Location = new System.Drawing.Point(6, 117);
            this.LabelLocationLogFile.Name = "LabelLocationLogFile";
            this.LabelLocationLogFile.Size = new System.Drawing.Size(119, 15);
            this.LabelLocationLogFile.TabIndex = 1;
            this.LabelLocationLogFile.Text = "Locatie Log bestand :";
            // 
            // LabelLocationSettingsFile
            // 
            this.LabelLocationSettingsFile.AutoSize = true;
            this.LabelLocationSettingsFile.Location = new System.Drawing.Point(6, 87);
            this.LabelLocationSettingsFile.Name = "LabelLocationSettingsFile";
            this.LabelLocationSettingsFile.Size = new System.Drawing.Size(140, 15);
            this.LabelLocationSettingsFile.TabIndex = 10;
            this.LabelLocationSettingsFile.Text = "Locatie settings bestand :";
            // 
            // TextBoxLocationSettingsFile
            // 
            this.TextBoxLocationSettingsFile.Enabled = false;
            this.TextBoxLocationSettingsFile.Location = new System.Drawing.Point(169, 84);
            this.TextBoxLocationSettingsFile.Name = "TextBoxLocationSettingsFile";
            this.TextBoxLocationSettingsFile.Size = new System.Drawing.Size(482, 23);
            this.TextBoxLocationSettingsFile.TabIndex = 5;
            // 
            // CheckBoxAppenLogFile
            // 
            this.CheckBoxAppenLogFile.AutoSize = true;
            this.CheckBoxAppenLogFile.Location = new System.Drawing.Point(6, 47);
            this.CheckBoxAppenLogFile.Name = "CheckBoxAppenLogFile";
            this.CheckBoxAppenLogFile.Size = new System.Drawing.Size(150, 19);
            this.CheckBoxAppenLogFile.TabIndex = 7;
            this.CheckBoxAppenLogFile.Text = "Vul het log bestand aan";
            this.CheckBoxAppenLogFile.UseVisualStyleBackColor = true;
            this.CheckBoxAppenLogFile.Click += new System.EventHandler(this.CheckBoxAppenLogFile_Click);
            // 
            // GroupBoxDbmaintenance
            // 
            this.GroupBoxDbmaintenance.Controls.Add(this.LabelLocationAppDatabaseFile);
            this.GroupBoxDbmaintenance.Controls.Add(this.CopyDatabaseIntervalTextBox);
            this.GroupBoxDbmaintenance.Controls.Add(this.TextBoxLocationDatabaseFile);
            this.GroupBoxDbmaintenance.Controls.Add(this.LabelCopyAppDb);
            this.GroupBoxDbmaintenance.Controls.Add(this.ButtonCompressAppDatabase);
            this.GroupBoxDbmaintenance.Location = new System.Drawing.Point(8, 168);
            this.GroupBoxDbmaintenance.Name = "GroupBoxDbmaintenance";
            this.GroupBoxDbmaintenance.Size = new System.Drawing.Size(665, 115);
            this.GroupBoxDbmaintenance.TabIndex = 18;
            this.GroupBoxDbmaintenance.TabStop = false;
            this.GroupBoxDbmaintenance.Text = "Database onderhoud";
            // 
            // LabelLocationAppDatabaseFile
            // 
            this.LabelLocationAppDatabaseFile.AutoSize = true;
            this.LabelLocationAppDatabaseFile.Location = new System.Drawing.Point(6, 84);
            this.LabelLocationAppDatabaseFile.Name = "LabelLocationAppDatabaseFile";
            this.LabelLocationAppDatabaseFile.Size = new System.Drawing.Size(155, 15);
            this.LabelLocationAppDatabaseFile.TabIndex = 21;
            this.LabelLocationAppDatabaseFile.Text = "Locatie applicatie database :";
            // 
            // CopyDatabaseIntervalTextBox
            // 
            this.CopyDatabaseIntervalTextBox.Location = new System.Drawing.Point(233, 52);
            this.CopyDatabaseIntervalTextBox.Name = "CopyDatabaseIntervalTextBox";
            this.CopyDatabaseIntervalTextBox.Size = new System.Drawing.Size(43, 23);
            this.CopyDatabaseIntervalTextBox.TabIndex = 3;
            this.CopyDatabaseIntervalTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.CopyDatabaseIntervalTextBox.TextChanged += new System.EventHandler(this.CopyDatabaseIntervalTextBox_TextChanged);
            // 
            // TextBoxLocationDatabaseFile
            // 
            this.TextBoxLocationDatabaseFile.Enabled = false;
            this.TextBoxLocationDatabaseFile.Location = new System.Drawing.Point(169, 81);
            this.TextBoxLocationDatabaseFile.Name = "TextBoxLocationDatabaseFile";
            this.TextBoxLocationDatabaseFile.Size = new System.Drawing.Size(482, 23);
            this.TextBoxLocationDatabaseFile.TabIndex = 22;
            // 
            // LabelCopyAppDb
            // 
            this.LabelCopyAppDb.AutoSize = true;
            this.LabelCopyAppDb.Location = new System.Drawing.Point(3, 55);
            this.LabelCopyAppDb.Name = "LabelCopyAppDb";
            this.LabelCopyAppDb.Size = new System.Drawing.Size(224, 15);
            this.LabelCopyAppDb.TabIndex = 1;
            this.LabelCopyAppDb.Text = "Kopieer de app database na x keer starten";
            // 
            // ButtonCompressAppDatabase
            // 
            this.ButtonCompressAppDatabase.Location = new System.Drawing.Point(6, 22);
            this.ButtonCompressAppDatabase.Name = "ButtonCompressAppDatabase";
            this.ButtonCompressAppDatabase.Size = new System.Drawing.Size(191, 23);
            this.ButtonCompressAppDatabase.TabIndex = 0;
            this.ButtonCompressAppDatabase.Text = "Comprimeer de app. database";
            this.ButtonCompressAppDatabase.UseVisualStyleBackColor = true;
            this.ButtonCompressAppDatabase.Click += new System.EventHandler(this.ButtonCompressAppDatabase_Click);
            // 
            // LabelBuildDate
            // 
            this.LabelBuildDate.AutoSize = true;
            this.LabelBuildDate.Location = new System.Drawing.Point(14, 355);
            this.LabelBuildDate.Name = "LabelBuildDate";
            this.LabelBuildDate.Size = new System.Drawing.Size(86, 15);
            this.LabelBuildDate.TabIndex = 20;
            this.LabelBuildDate.Text = "Build datum : -";
            // 
            // LabelVersion
            // 
            this.LabelVersion.AutoSize = true;
            this.LabelVersion.Location = new System.Drawing.Point(14, 327);
            this.LabelVersion.Name = "LabelVersion";
            this.LabelVersion.Size = new System.Drawing.Size(52, 15);
            this.LabelVersion.TabIndex = 19;
            this.LabelVersion.Text = "Versie : -";
            // 
            // ButtonGetCoinNames
            // 
            this.ButtonGetCoinNames.Location = new System.Drawing.Point(193, 126);
            this.ButtonGetCoinNames.Name = "ButtonGetCoinNames";
            this.ButtonGetCoinNames.Size = new System.Drawing.Size(75, 23);
            this.ButtonGetCoinNames.TabIndex = 23;
            this.ButtonGetCoinNames.Text = "Vernieuw";
            this.ButtonGetCoinNames.UseVisualStyleBackColor = true;
            this.ButtonGetCoinNames.Click += new System.EventHandler(this.ButtonGetCoinNames_Click);
            // 
            // TreeViewCoinNames
            // 
            this.TreeViewCoinNames.CheckBoxes = true;
            this.TreeViewCoinNames.Location = new System.Drawing.Point(6, 17);
            this.TreeViewCoinNames.Name = "TreeViewCoinNames";
            this.TreeViewCoinNames.Size = new System.Drawing.Size(181, 323);
            this.TreeViewCoinNames.TabIndex = 24;
            // 
            // ButtonSelectAll
            // 
            this.ButtonSelectAll.Location = new System.Drawing.Point(193, 17);
            this.ButtonSelectAll.Name = "ButtonSelectAll";
            this.ButtonSelectAll.Size = new System.Drawing.Size(75, 23);
            this.ButtonSelectAll.TabIndex = 25;
            this.ButtonSelectAll.Text = "Alle";
            this.ButtonSelectAll.UseVisualStyleBackColor = true;
            this.ButtonSelectAll.Click += new System.EventHandler(this.ButtonSelectAll_Click);
            // 
            // ButtonDeselectAll
            // 
            this.ButtonDeselectAll.Location = new System.Drawing.Point(193, 46);
            this.ButtonDeselectAll.Name = "ButtonDeselectAll";
            this.ButtonDeselectAll.Size = new System.Drawing.Size(75, 23);
            this.ButtonDeselectAll.TabIndex = 26;
            this.ButtonDeselectAll.Text = "Geen";
            this.ButtonDeselectAll.UseVisualStyleBackColor = true;
            this.ButtonDeselectAll.Click += new System.EventHandler(this.ButtonDeselectAll_Click);
            // 
            // ButtonInvertSelection
            // 
            this.ButtonInvertSelection.Location = new System.Drawing.Point(193, 75);
            this.ButtonInvertSelection.Name = "ButtonInvertSelection";
            this.ButtonInvertSelection.Size = new System.Drawing.Size(75, 23);
            this.ButtonInvertSelection.TabIndex = 27;
            this.ButtonInvertSelection.Text = "Invert";
            this.ButtonInvertSelection.UseVisualStyleBackColor = true;
            this.ButtonInvertSelection.Click += new System.EventHandler(this.ButtonInvertSelection_Click);
            // 
            // TabControl1
            // 
            this.TabControl1.Controls.Add(this.TabPageCoins);
            this.TabControl1.Controls.Add(this.TabPageAppSettings);
            this.TabControl1.Controls.Add(this.TabPageAPI);
            this.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabControl1.Location = new System.Drawing.Point(0, 0);
            this.TabControl1.Name = "TabControl1";
            this.TabControl1.SelectedIndex = 0;
            this.TabControl1.Size = new System.Drawing.Size(710, 408);
            this.TabControl1.TabIndex = 28;
            // 
            // TabPageCoins
            // 
            this.TabPageCoins.Controls.Add(this.GroupBox1);
            this.TabPageCoins.Location = new System.Drawing.Point(4, 24);
            this.TabPageCoins.Name = "TabPageCoins";
            this.TabPageCoins.Padding = new System.Windows.Forms.Padding(3);
            this.TabPageCoins.Size = new System.Drawing.Size(702, 380);
            this.TabPageCoins.TabIndex = 0;
            this.TabPageCoins.Text = "Munt namen";
            this.TabPageCoins.UseVisualStyleBackColor = true;
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.ButtonSearchCoinName);
            this.GroupBox1.Controls.Add(this.LabelCoinFound);
            this.GroupBox1.Controls.Add(this.TextBoxSearchCoinName);
            this.GroupBox1.Controls.Add(this.ButtonGetCoinNames);
            this.GroupBox1.Controls.Add(this.ButtonInvertSelection);
            this.GroupBox1.Controls.Add(this.TreeViewCoinNames);
            this.GroupBox1.Controls.Add(this.ButtonDeselectAll);
            this.GroupBox1.Controls.Add(this.ButtonSelectAll);
            this.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GroupBox1.Location = new System.Drawing.Point(3, 3);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(696, 374);
            this.GroupBox1.TabIndex = 25;
            this.GroupBox1.TabStop = false;
            // 
            // ButtonSearchCoinName
            // 
            this.ButtonSearchCoinName.Location = new System.Drawing.Point(112, 346);
            this.ButtonSearchCoinName.Name = "ButtonSearchCoinName";
            this.ButtonSearchCoinName.Size = new System.Drawing.Size(75, 23);
            this.ButtonSearchCoinName.TabIndex = 31;
            this.ButtonSearchCoinName.Text = "Zoek";
            this.ButtonSearchCoinName.UseVisualStyleBackColor = true;
            this.ButtonSearchCoinName.Click += new System.EventHandler(this.ButtonSearchCoinName_Click);
            // 
            // LabelCoinFound
            // 
            this.LabelCoinFound.AutoSize = true;
            this.LabelCoinFound.Location = new System.Drawing.Point(193, 350);
            this.LabelCoinFound.Name = "LabelCoinFound";
            this.LabelCoinFound.Size = new System.Drawing.Size(25, 15);
            this.LabelCoinFound.TabIndex = 30;
            this.LabelCoinFound.Text = "0 st";
            // 
            // TextBoxSearchCoinName
            // 
            this.TextBoxSearchCoinName.Location = new System.Drawing.Point(6, 345);
            this.TextBoxSearchCoinName.Name = "TextBoxSearchCoinName";
            this.TextBoxSearchCoinName.Size = new System.Drawing.Size(100, 23);
            this.TextBoxSearchCoinName.TabIndex = 29;
            this.TextBoxSearchCoinName.TextChanged += new System.EventHandler(this.TextBoxSearchCoinName_TextChanged);
            // 
            // TabPageAppSettings
            // 
            this.TabPageAppSettings.Controls.Add(this.GroupBoxLogFile);
            this.TabPageAppSettings.Controls.Add(this.GroupBoxDbmaintenance);
            this.TabPageAppSettings.Controls.Add(this.LabelBuildDate);
            this.TabPageAppSettings.Controls.Add(this.LabelVersion);
            this.TabPageAppSettings.Location = new System.Drawing.Point(4, 24);
            this.TabPageAppSettings.Name = "TabPageAppSettings";
            this.TabPageAppSettings.Padding = new System.Windows.Forms.Padding(3);
            this.TabPageAppSettings.Size = new System.Drawing.Size(702, 380);
            this.TabPageAppSettings.TabIndex = 1;
            this.TabPageAppSettings.Text = "Instellingen";
            this.TabPageAppSettings.UseVisualStyleBackColor = true;
            // 
            // TabPageAPI
            // 
            this.TabPageAPI.Controls.Add(this.TextBoxWarnPercentage);
            this.TabPageAPI.Controls.Add(this.LabelWarnPercentage);
            this.TabPageAPI.Controls.Add(this.TextBoxRateLimit);
            this.TabPageAPI.Controls.Add(this.LabelRateLimit);
            this.TabPageAPI.Controls.Add(this.label3);
            this.TabPageAPI.Controls.Add(this.GroupBoxUrl);
            this.TabPageAPI.Location = new System.Drawing.Point(4, 24);
            this.TabPageAPI.Name = "TabPageAPI";
            this.TabPageAPI.Padding = new System.Windows.Forms.Padding(3);
            this.TabPageAPI.Size = new System.Drawing.Size(702, 380);
            this.TabPageAPI.TabIndex = 2;
            this.TabPageAPI.Text = "API";
            this.TabPageAPI.UseVisualStyleBackColor = true;
            // 
            // TextBoxWarnPercentage
            // 
            this.TextBoxWarnPercentage.Location = new System.Drawing.Point(246, 167);
            this.TextBoxWarnPercentage.Name = "TextBoxWarnPercentage";
            this.TextBoxWarnPercentage.Size = new System.Drawing.Size(49, 23);
            this.TextBoxWarnPercentage.TabIndex = 34;
            this.TextBoxWarnPercentage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.TextBoxWarnPercentage.TextChanged += new System.EventHandler(this.TextBoxWarnPercentage_TextChanged);
            this.TextBoxWarnPercentage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBoxWarnPercentage_KeyPress);
            // 
            // LabelWarnPercentage
            // 
            this.LabelWarnPercentage.AutoSize = true;
            this.LabelWarnPercentage.Location = new System.Drawing.Point(18, 170);
            this.LabelWarnPercentage.Name = "LabelWarnPercentage";
            this.LabelWarnPercentage.Size = new System.Drawing.Size(211, 15);
            this.LabelWarnPercentage.TabIndex = 33;
            this.LabelWarnPercentage.Text = "Waarschuwen bij een verschil van x % :";
            // 
            // TextBoxRateLimit
            // 
            this.TextBoxRateLimit.Location = new System.Drawing.Point(246, 138);
            this.TextBoxRateLimit.Name = "TextBoxRateLimit";
            this.TextBoxRateLimit.Size = new System.Drawing.Size(49, 23);
            this.TextBoxRateLimit.TabIndex = 32;
            this.TextBoxRateLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.TextBoxRateLimit.TextChanged += new System.EventHandler(this.TextBoxRateLimit_TextChanged);
            this.TextBoxRateLimit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBoxRateLimit_KeyPress);
            this.TextBoxRateLimit.Leave += new System.EventHandler(this.TextBoxRateLimit_Leave);
            // 
            // LabelRateLimit
            // 
            this.LabelRateLimit.AutoSize = true;
            this.LabelRateLimit.Location = new System.Drawing.Point(18, 141);
            this.LabelRateLimit.Name = "LabelRateLimit";
            this.LabelRateLimit.Size = new System.Drawing.Size(165, 15);
            this.LabelRateLimit.TabIndex = 31;
            this.LabelRateLimit.Text = "Aantal verzoeken per minuut :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 15);
            this.label3.TabIndex = 30;
            this.label3.Text = "label3";
            // 
            // GroupBoxUrl
            // 
            this.GroupBoxUrl.Controls.Add(this.CheckBoxEnableUrl);
            this.GroupBoxUrl.Controls.Add(this.TextBoxUrl1);
            this.GroupBoxUrl.Controls.Add(this.Label1);
            this.GroupBoxUrl.Controls.Add(this.Label2);
            this.GroupBoxUrl.Controls.Add(this.TextBoxUrl2);
            this.GroupBoxUrl.Location = new System.Drawing.Point(8, 17);
            this.GroupBoxUrl.Name = "GroupBoxUrl";
            this.GroupBoxUrl.Size = new System.Drawing.Size(369, 115);
            this.GroupBoxUrl.TabIndex = 29;
            this.GroupBoxUrl.TabStop = false;
            this.GroupBoxUrl.Text = "Connectie Url(s)";
            // 
            // CheckBoxEnableUrl
            // 
            this.CheckBoxEnableUrl.AutoSize = true;
            this.CheckBoxEnableUrl.Location = new System.Drawing.Point(10, 22);
            this.CheckBoxEnableUrl.Name = "CheckBoxEnableUrl";
            this.CheckBoxEnableUrl.Size = new System.Drawing.Size(84, 19);
            this.CheckBoxEnableUrl.TabIndex = 5;
            this.CheckBoxEnableUrl.Text = "Pas Url aan";
            this.CheckBoxEnableUrl.UseVisualStyleBackColor = true;
            this.CheckBoxEnableUrl.Click += new System.EventHandler(this.CheckBoxEnableUrl_Click);
            // 
            // TextBoxUrl1
            // 
            this.TextBoxUrl1.Enabled = false;
            this.TextBoxUrl1.Location = new System.Drawing.Point(69, 50);
            this.TextBoxUrl1.Name = "TextBoxUrl1";
            this.TextBoxUrl1.Size = new System.Drawing.Size(285, 23);
            this.TextBoxUrl1.TabIndex = 3;
            this.TextBoxUrl1.Text = "https://api.bitvavo.com/v2/ticker/price";
            this.TextBoxUrl1.TextChanged += new System.EventHandler(this.TextBoxUrl1_Leave);
            this.TextBoxUrl1.Leave += new System.EventHandler(this.TextBoxUrl1_Leave);
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(10, 53);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(37, 15);
            this.Label1.TabIndex = 1;
            this.Label1.Text = "Url 1 :";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(10, 82);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(37, 15);
            this.Label2.TabIndex = 2;
            this.Label2.Text = "Url 2 :";
            // 
            // TextBoxUrl2
            // 
            this.TextBoxUrl2.Enabled = false;
            this.TextBoxUrl2.Location = new System.Drawing.Point(69, 79);
            this.TextBoxUrl2.Name = "TextBoxUrl2";
            this.TextBoxUrl2.Size = new System.Drawing.Size(285, 23);
            this.TextBoxUrl2.TabIndex = 4;
            this.TextBoxUrl2.Text = "https://api.bitvavo.com/v2/ticker/24h";
            this.TextBoxUrl2.TextChanged += new System.EventHandler(this.TextBoxUrl2_Leave);
            this.TextBoxUrl2.Leave += new System.EventHandler(this.TextBoxUrl2_Leave);
            // 
            // FormConfigure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(710, 408);
            this.Controls.Add(this.TabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormConfigure";
            this.Text = "Opties";
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
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TextBoxWarnPercentage;
        private System.Windows.Forms.Label LabelWarnPercentage;
        private System.Windows.Forms.Label LabelCoinFound;
        private System.Windows.Forms.TextBox TextBoxSearchCoinName;
        private System.Windows.Forms.Button ButtonSearchCoinName;
    }
}