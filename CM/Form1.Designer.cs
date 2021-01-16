
namespace CM
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.TabControlMain = new System.Windows.Forms.TabControl();
            this.TabPageConfigure = new System.Windows.Forms.TabPage();
            this.GroupBoxSessionSettings = new System.Windows.Forms.GroupBox();
            this.LabelWarnPercentage = new System.Windows.Forms.Label();
            this.TextBoxWarnPercentage = new System.Windows.Forms.TextBox();
            this.ButtonStop = new System.Windows.Forms.Button();
            this.ButtonPause = new System.Windows.Forms.Button();
            this.ButtonStart = new System.Windows.Forms.Button();
            this.TextBoxTimeInterval = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TabPageCharts = new System.Windows.Forms.TabPage();
            this.TabControlCharts = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.labelWarnPerc = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ToolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.MenuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ToolStripMenuItem_Program = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Program_Close = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Session = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Session_Start = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Session_Pause = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Session_Stop = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Options = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Options_Configure = new System.Windows.Forms.ToolStripMenuItem();
            this.PanelBottom = new System.Windows.Forms.Panel();
            this.Timer1 = new System.Windows.Forms.Timer(this.components);
            this.ToolStrip1 = new System.Windows.Forms.ToolStrip();
            this.ToolStripButton_SessionStart = new System.Windows.Forms.ToolStripButton();
            this.ToolStripButton_SessionPause = new System.Windows.Forms.ToolStripButton();
            this.ToolStripButton_SessionStop = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripComboBoxCoinNames = new System.Windows.Forms.ToolStripComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.TabControlMain.SuspendLayout();
            this.TabPageConfigure.SuspendLayout();
            this.GroupBoxSessionSettings.SuspendLayout();
            this.TabPageCharts.SuspendLayout();
            this.TabControlCharts.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.MenuStrip1.SuspendLayout();
            this.ToolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TabControlMain
            // 
            this.TabControlMain.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.TabControlMain.Controls.Add(this.TabPageConfigure);
            this.TabControlMain.Controls.Add(this.TabPageCharts);
            this.TabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabControlMain.HotTrack = true;
            this.TabControlMain.Location = new System.Drawing.Point(0, 0);
            this.TabControlMain.Multiline = true;
            this.TabControlMain.Name = "TabControlMain";
            this.TabControlMain.SelectedIndex = 0;
            this.TabControlMain.Size = new System.Drawing.Size(799, 431);
            this.TabControlMain.TabIndex = 0;
            // 
            // TabPageConfigure
            // 
            this.TabPageConfigure.Controls.Add(this.GroupBoxSessionSettings);
            this.TabPageConfigure.Location = new System.Drawing.Point(27, 4);
            this.TabPageConfigure.Name = "TabPageConfigure";
            this.TabPageConfigure.Padding = new System.Windows.Forms.Padding(3);
            this.TabPageConfigure.Size = new System.Drawing.Size(768, 423);
            this.TabPageConfigure.TabIndex = 0;
            this.TabPageConfigure.Text = "Start";
            this.TabPageConfigure.UseVisualStyleBackColor = true;
            // 
            // GroupBoxSessionSettings
            // 
            this.GroupBoxSessionSettings.Controls.Add(this.LabelWarnPercentage);
            this.GroupBoxSessionSettings.Controls.Add(this.TextBoxWarnPercentage);
            this.GroupBoxSessionSettings.Controls.Add(this.ButtonStop);
            this.GroupBoxSessionSettings.Controls.Add(this.ButtonPause);
            this.GroupBoxSessionSettings.Controls.Add(this.ButtonStart);
            this.GroupBoxSessionSettings.Controls.Add(this.TextBoxTimeInterval);
            this.GroupBoxSessionSettings.Controls.Add(this.label2);
            this.GroupBoxSessionSettings.Location = new System.Drawing.Point(14, 14);
            this.GroupBoxSessionSettings.Name = "GroupBoxSessionSettings";
            this.GroupBoxSessionSettings.Size = new System.Drawing.Size(310, 159);
            this.GroupBoxSessionSettings.TabIndex = 8;
            this.GroupBoxSessionSettings.TabStop = false;
            // 
            // LabelWarnPercentage
            // 
            this.LabelWarnPercentage.AutoSize = true;
            this.LabelWarnPercentage.Location = new System.Drawing.Point(11, 58);
            this.LabelWarnPercentage.Name = "LabelWarnPercentage";
            this.LabelWarnPercentage.Size = new System.Drawing.Size(227, 15);
            this.LabelWarnPercentage.TabIndex = 19;
            this.LabelWarnPercentage.Text = "Waarschuwen bij een stijging/daling van :";
            // 
            // TextBoxWarnPercentage
            // 
            this.TextBoxWarnPercentage.Location = new System.Drawing.Point(244, 55);
            this.TextBoxWarnPercentage.Name = "TextBoxWarnPercentage";
            this.TextBoxWarnPercentage.Size = new System.Drawing.Size(42, 23);
            this.TextBoxWarnPercentage.TabIndex = 18;
            this.TextBoxWarnPercentage.Text = "1";
            this.TextBoxWarnPercentage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.TextBoxWarnPercentage.TextChanged += new System.EventHandler(this.TextBoxWarnPercentage_TextChanged);
            this.TextBoxWarnPercentage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBoxWarnPercentage_KeyPress);
            // 
            // ButtonStop
            // 
            this.ButtonStop.Enabled = false;
            this.ButtonStop.Location = new System.Drawing.Point(211, 105);
            this.ButtonStop.Name = "ButtonStop";
            this.ButtonStop.Size = new System.Drawing.Size(75, 23);
            this.ButtonStop.TabIndex = 17;
            this.ButtonStop.Text = "Stop";
            this.ButtonStop.UseVisualStyleBackColor = true;
            this.ButtonStop.Click += new System.EventHandler(this.ToolStripMenuItem_Session_Stop_Click);
            // 
            // ButtonPause
            // 
            this.ButtonPause.Enabled = false;
            this.ButtonPause.Location = new System.Drawing.Point(92, 105);
            this.ButtonPause.Name = "ButtonPause";
            this.ButtonPause.Size = new System.Drawing.Size(75, 23);
            this.ButtonPause.TabIndex = 16;
            this.ButtonPause.Text = "Pauze";
            this.ButtonPause.UseVisualStyleBackColor = true;
            this.ButtonPause.Click += new System.EventHandler(this.ToolStripMenuItem_Session_Pause_Click);
            // 
            // ButtonStart
            // 
            this.ButtonStart.Location = new System.Drawing.Point(11, 105);
            this.ButtonStart.Name = "ButtonStart";
            this.ButtonStart.Size = new System.Drawing.Size(75, 23);
            this.ButtonStart.TabIndex = 15;
            this.ButtonStart.Text = "Start";
            this.ButtonStart.UseVisualStyleBackColor = true;
            this.ButtonStart.Click += new System.EventHandler(this.ToolStripMenuItem_Session_Start_Click);
            // 
            // TextBoxTimeInterval
            // 
            this.TextBoxTimeInterval.Location = new System.Drawing.Point(244, 26);
            this.TextBoxTimeInterval.Name = "TextBoxTimeInterval";
            this.TextBoxTimeInterval.Size = new System.Drawing.Size(42, 23);
            this.TextBoxTimeInterval.TabIndex = 2;
            this.TextBoxTimeInterval.Text = "1";
            this.TextBoxTimeInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.TextBoxTimeInterval.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBoxTimeInterval_KeyPress);
            this.TextBoxTimeInterval.Leave += new System.EventHandler(this.TextBoxTimeInterval_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "Intervaltijd in minuten :";
            // 
            // TabPageCharts
            // 
            this.TabPageCharts.Controls.Add(this.TabControlCharts);
            this.TabPageCharts.Location = new System.Drawing.Point(27, 4);
            this.TabPageCharts.Name = "TabPageCharts";
            this.TabPageCharts.Padding = new System.Windows.Forms.Padding(3);
            this.TabPageCharts.Size = new System.Drawing.Size(768, 423);
            this.TabPageCharts.TabIndex = 1;
            this.TabPageCharts.Text = "Grafieken";
            this.TabPageCharts.UseVisualStyleBackColor = true;
            // 
            // TabControlCharts
            // 
            this.TabControlCharts.Controls.Add(this.tabPage1);
            this.TabControlCharts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabControlCharts.Location = new System.Drawing.Point(3, 3);
            this.TabControlCharts.Name = "TabControlCharts";
            this.TabControlCharts.SelectedIndex = 0;
            this.TabControlCharts.Size = new System.Drawing.Size(762, 417);
            this.TabControlCharts.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(754, 389);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Window;
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(748, 383);
            this.splitContainer1.SplitterDistance = 299;
            this.splitContainer1.SplitterWidth = 10;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.labelWarnPerc);
            this.splitContainer2.Panel1.Controls.Add(this.textBox1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.dataGridView1);
            this.splitContainer2.Size = new System.Drawing.Size(299, 383);
            this.splitContainer2.SplitterDistance = 182;
            this.splitContainer2.SplitterWidth = 10;
            this.splitContainer2.TabIndex = 0;
            // 
            // labelWarnPerc
            // 
            this.labelWarnPerc.AutoSize = true;
            this.labelWarnPerc.Location = new System.Drawing.Point(3, 6);
            this.labelWarnPerc.Name = "labelWarnPerc";
            this.labelWarnPerc.Size = new System.Drawing.Size(227, 15);
            this.labelWarnPerc.TabIndex = 1;
            this.labelWarnPerc.Text = "Waarschuwen bij een stijging/daling van :";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(235, 3);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(40, 23);
            this.textBox1.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataGridView1.RowTemplate.Height = 25;
            this.dataGridView1.Size = new System.Drawing.Size(248, 110);
            this.dataGridView1.TabIndex = 0;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Column1";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Column2";
            this.Column2.Name = "Column2";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripStatusLabel1,
            this.ToolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 520);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(799, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ToolStripStatusLabel1
            // 
            this.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1";
            this.ToolStripStatusLabel1.Size = new System.Drawing.Size(103, 17);
            this.ToolStripStatusLabel1.Text = "                                ";
            // 
            // ToolStripStatusLabel2
            // 
            this.ToolStripStatusLabel2.Name = "ToolStripStatusLabel2";
            this.ToolStripStatusLabel2.Size = new System.Drawing.Size(52, 17);
            this.ToolStripStatusLabel2.Text = "               ";
            // 
            // MenuStrip1
            // 
            this.MenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_Program,
            this.ToolStripMenuItem_Session,
            this.ToolStripMenuItem_Options});
            this.MenuStrip1.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip1.Name = "MenuStrip1";
            this.MenuStrip1.Size = new System.Drawing.Size(799, 24);
            this.MenuStrip1.TabIndex = 2;
            this.MenuStrip1.Text = "menuStrip1";
            // 
            // ToolStripMenuItem_Program
            // 
            this.ToolStripMenuItem_Program.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_Program_Close});
            this.ToolStripMenuItem_Program.Name = "ToolStripMenuItem_Program";
            this.ToolStripMenuItem_Program.Size = new System.Drawing.Size(82, 20);
            this.ToolStripMenuItem_Program.Text = "Programma";
            // 
            // ToolStripMenuItem_Program_Close
            // 
            this.ToolStripMenuItem_Program_Close.Name = "ToolStripMenuItem_Program_Close";
            this.ToolStripMenuItem_Program_Close.Size = new System.Drawing.Size(121, 22);
            this.ToolStripMenuItem_Program_Close.Text = "Afsluiten";
            this.ToolStripMenuItem_Program_Close.Click += new System.EventHandler(this.ToolStripMenuItem_Program_Close_Click);
            // 
            // ToolStripMenuItem_Session
            // 
            this.ToolStripMenuItem_Session.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_Session_Start,
            this.ToolStripMenuItem_Session_Pause,
            this.ToolStripMenuItem_Session_Stop});
            this.ToolStripMenuItem_Session.Name = "ToolStripMenuItem_Session";
            this.ToolStripMenuItem_Session.Size = new System.Drawing.Size(50, 20);
            this.ToolStripMenuItem_Session.Text = "Sessie";
            // 
            // ToolStripMenuItem_Session_Start
            // 
            this.ToolStripMenuItem_Session_Start.Name = "ToolStripMenuItem_Session_Start";
            this.ToolStripMenuItem_Session_Start.Size = new System.Drawing.Size(105, 22);
            this.ToolStripMenuItem_Session_Start.Text = "Start";
            this.ToolStripMenuItem_Session_Start.Click += new System.EventHandler(this.ToolStripMenuItem_Session_Start_Click);
            // 
            // ToolStripMenuItem_Session_Pause
            // 
            this.ToolStripMenuItem_Session_Pause.Name = "ToolStripMenuItem_Session_Pause";
            this.ToolStripMenuItem_Session_Pause.Size = new System.Drawing.Size(105, 22);
            this.ToolStripMenuItem_Session_Pause.Text = "Pauze";
            this.ToolStripMenuItem_Session_Pause.Click += new System.EventHandler(this.ToolStripMenuItem_Session_Pause_Click);
            // 
            // ToolStripMenuItem_Session_Stop
            // 
            this.ToolStripMenuItem_Session_Stop.Name = "ToolStripMenuItem_Session_Stop";
            this.ToolStripMenuItem_Session_Stop.Size = new System.Drawing.Size(105, 22);
            this.ToolStripMenuItem_Session_Stop.Text = "Stop";
            this.ToolStripMenuItem_Session_Stop.Click += new System.EventHandler(this.ToolStripMenuItem_Session_Stop_Click);
            // 
            // ToolStripMenuItem_Options
            // 
            this.ToolStripMenuItem_Options.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_Options_Configure});
            this.ToolStripMenuItem_Options.Name = "ToolStripMenuItem_Options";
            this.ToolStripMenuItem_Options.Size = new System.Drawing.Size(53, 20);
            this.ToolStripMenuItem_Options.Text = "Opties";
            // 
            // ToolStripMenuItem_Options_Configure
            // 
            this.ToolStripMenuItem_Options_Configure.Name = "ToolStripMenuItem_Options_Configure";
            this.ToolStripMenuItem_Options_Configure.Size = new System.Drawing.Size(108, 22);
            this.ToolStripMenuItem_Options_Configure.Text = "Opties";
            this.ToolStripMenuItem_Options_Configure.Click += new System.EventHandler(this.ToolStripMenuItem_Options_Configure_Click);
            // 
            // PanelBottom
            // 
            this.PanelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PanelBottom.Location = new System.Drawing.Point(0, 480);
            this.PanelBottom.Name = "PanelBottom";
            this.PanelBottom.Size = new System.Drawing.Size(799, 40);
            this.PanelBottom.TabIndex = 5;
            // 
            // ToolStrip1
            // 
            this.ToolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripButton_SessionStart,
            this.ToolStripButton_SessionPause,
            this.ToolStripButton_SessionStop,
            this.toolStripSeparator1,
            this.ToolStripComboBoxCoinNames});
            this.ToolStrip1.Location = new System.Drawing.Point(0, 24);
            this.ToolStrip1.Name = "ToolStrip1";
            this.ToolStrip1.Size = new System.Drawing.Size(799, 25);
            this.ToolStrip1.TabIndex = 0;
            // 
            // ToolStripButton_SessionStart
            // 
            this.ToolStripButton_SessionStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripButton_SessionStart.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripButton_SessionStart.Image")));
            this.ToolStripButton_SessionStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripButton_SessionStart.Name = "ToolStripButton_SessionStart";
            this.ToolStripButton_SessionStart.Size = new System.Drawing.Size(23, 22);
            this.ToolStripButton_SessionStart.Text = "Start";
            this.ToolStripButton_SessionStart.Click += new System.EventHandler(this.ToolStripMenuItem_Session_Start_Click);
            // 
            // ToolStripButton_SessionPause
            // 
            this.ToolStripButton_SessionPause.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripButton_SessionPause.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripButton_SessionPause.Image")));
            this.ToolStripButton_SessionPause.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripButton_SessionPause.Name = "ToolStripButton_SessionPause";
            this.ToolStripButton_SessionPause.Size = new System.Drawing.Size(23, 22);
            this.ToolStripButton_SessionPause.Text = "Pause";
            this.ToolStripButton_SessionPause.Click += new System.EventHandler(this.ToolStripMenuItem_Session_Pause_Click);
            // 
            // ToolStripButton_SessionStop
            // 
            this.ToolStripButton_SessionStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripButton_SessionStop.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripButton_SessionStop.Image")));
            this.ToolStripButton_SessionStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripButton_SessionStop.Name = "ToolStripButton_SessionStop";
            this.ToolStripButton_SessionStop.Size = new System.Drawing.Size(23, 22);
            this.ToolStripButton_SessionStop.Text = "Stop";
            this.ToolStripButton_SessionStop.Click += new System.EventHandler(this.ToolStripMenuItem_Session_Stop_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // ToolStripComboBoxCoinNames
            // 
            this.ToolStripComboBoxCoinNames.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.ToolStripComboBoxCoinNames.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ToolStripComboBoxCoinNames.Enabled = false;
            this.ToolStripComboBoxCoinNames.Name = "ToolStripComboBoxCoinNames";
            this.ToolStripComboBoxCoinNames.Size = new System.Drawing.Size(121, 25);
            this.ToolStripComboBoxCoinNames.SelectedIndexChanged += new System.EventHandler(this.ToolStripComboBoxCoinNames_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Controls.Add(this.TabControlMain);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 49);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(799, 431);
            this.panel1.TabIndex = 6;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(799, 542);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ToolStrip1);
            this.Controls.Add(this.PanelBottom);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.MenuStrip1);
            this.MainMenuStrip = this.MenuStrip1;
            this.Name = "FormMain";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.TabControlMain.ResumeLayout(false);
            this.TabPageConfigure.ResumeLayout(false);
            this.GroupBoxSessionSettings.ResumeLayout(false);
            this.GroupBoxSessionSettings.PerformLayout();
            this.TabPageCharts.ResumeLayout(false);
            this.TabControlCharts.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.MenuStrip1.ResumeLayout(false);
            this.MenuStrip1.PerformLayout();
            this.ToolStrip1.ResumeLayout(false);
            this.ToolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl TabControlMain;
        private System.Windows.Forms.TabPage TabPageConfigure;
        private System.Windows.Forms.TabPage TabPageCharts;
        private System.Windows.Forms.TabControl TabControlCharts;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.MenuStrip MenuStrip1;
        private System.Windows.Forms.Panel PanelBottom;
        private System.Windows.Forms.GroupBox GroupBoxSessionSettings;
        private System.Windows.Forms.TextBox TextBoxTimeInterval;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ButtonStop;
        private System.Windows.Forms.Button ButtonPause;
        private System.Windows.Forms.Button ButtonStart;
        private System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel1;
        private System.Windows.Forms.Timer Timer1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Program;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Program_Close;
        private System.Windows.Forms.Label labelWarnPerc;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox TextBoxWarnPercentage;
        private System.Windows.Forms.Label LabelWarnPercentage;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Session;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Session_Start;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Session_Pause;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Session_Stop;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Options;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Options_Configure;
        private System.Windows.Forms.ToolStrip ToolStrip1;
        private System.Windows.Forms.ToolStripButton ToolStripButton_SessionStart;
        private System.Windows.Forms.ToolStripButton ToolStripButton_SessionPause;
        private System.Windows.Forms.ToolStripButton ToolStripButton_SessionStop;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripComboBox ToolStripComboBoxCoinNames;
    }
}

