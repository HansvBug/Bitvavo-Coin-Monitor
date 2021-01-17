
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.TabControlMain.SuspendLayout();
            this.TabPageConfigure.SuspendLayout();
            this.GroupBoxSessionSettings.SuspendLayout();
            this.TabPageCharts.SuspendLayout();
            this.TabControlCharts.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.MenuStrip1.SuspendLayout();
            this.ToolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            resources.ApplyResources(this.splitContainer1.Panel1, "splitContainer1.Panel1");
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Window;
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            resources.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
            // 
            // splitContainer2
            // 
            resources.ApplyResources(this.splitContainer2, "splitContainer2");
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            resources.ApplyResources(this.splitContainer2.Panel1, "splitContainer2.Panel1");
            // 
            // splitContainer2.Panel2
            // 
            resources.ApplyResources(this.splitContainer2.Panel2, "splitContainer2.Panel2");
            this.splitContainer2.Panel2.Controls.Add(this.dataGridView1);
            // 
            // dataGridView1
            // 
            resources.ApplyResources(this.dataGridView1, "dataGridView1");
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataGridView1.RowTemplate.Height = 25;
            // 
            // Column1
            // 
            resources.ApplyResources(this.Column1, "Column1");
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            resources.ApplyResources(this.Column2, "Column2");
            this.Column2.Name = "Column2";
            // 
            // TabControlMain
            // 
            resources.ApplyResources(this.TabControlMain, "TabControlMain");
            this.TabControlMain.Controls.Add(this.TabPageConfigure);
            this.TabControlMain.Controls.Add(this.TabPageCharts);
            this.TabControlMain.HotTrack = true;
            this.TabControlMain.Multiline = true;
            this.TabControlMain.Name = "TabControlMain";
            this.TabControlMain.SelectedIndex = 0;
            // 
            // TabPageConfigure
            // 
            resources.ApplyResources(this.TabPageConfigure, "TabPageConfigure");
            this.TabPageConfigure.Controls.Add(this.GroupBoxSessionSettings);
            this.TabPageConfigure.Name = "TabPageConfigure";
            this.TabPageConfigure.UseVisualStyleBackColor = true;
            // 
            // GroupBoxSessionSettings
            // 
            resources.ApplyResources(this.GroupBoxSessionSettings, "GroupBoxSessionSettings");
            this.GroupBoxSessionSettings.Controls.Add(this.LabelWarnPercentage);
            this.GroupBoxSessionSettings.Controls.Add(this.TextBoxWarnPercentage);
            this.GroupBoxSessionSettings.Controls.Add(this.ButtonStop);
            this.GroupBoxSessionSettings.Controls.Add(this.ButtonPause);
            this.GroupBoxSessionSettings.Controls.Add(this.ButtonStart);
            this.GroupBoxSessionSettings.Controls.Add(this.TextBoxTimeInterval);
            this.GroupBoxSessionSettings.Controls.Add(this.label2);
            this.GroupBoxSessionSettings.Name = "GroupBoxSessionSettings";
            this.GroupBoxSessionSettings.TabStop = false;
            // 
            // LabelWarnPercentage
            // 
            resources.ApplyResources(this.LabelWarnPercentage, "LabelWarnPercentage");
            this.LabelWarnPercentage.Name = "LabelWarnPercentage";
            // 
            // TextBoxWarnPercentage
            // 
            resources.ApplyResources(this.TextBoxWarnPercentage, "TextBoxWarnPercentage");
            this.TextBoxWarnPercentage.Name = "TextBoxWarnPercentage";
            this.TextBoxWarnPercentage.TextChanged += new System.EventHandler(this.TextBoxWarnPercentage_TextChanged);
            this.TextBoxWarnPercentage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBoxWarnPercentage_KeyPress);
            // 
            // ButtonStop
            // 
            resources.ApplyResources(this.ButtonStop, "ButtonStop");
            this.ButtonStop.Name = "ButtonStop";
            this.ButtonStop.UseVisualStyleBackColor = true;
            this.ButtonStop.Click += new System.EventHandler(this.ToolStripMenuItem_Session_Stop_Click);
            // 
            // ButtonPause
            // 
            resources.ApplyResources(this.ButtonPause, "ButtonPause");
            this.ButtonPause.Name = "ButtonPause";
            this.ButtonPause.UseVisualStyleBackColor = true;
            this.ButtonPause.Click += new System.EventHandler(this.ToolStripMenuItem_Session_Pause_Click);
            // 
            // ButtonStart
            // 
            resources.ApplyResources(this.ButtonStart, "ButtonStart");
            this.ButtonStart.Name = "ButtonStart";
            this.ButtonStart.UseVisualStyleBackColor = true;
            this.ButtonStart.Click += new System.EventHandler(this.ToolStripMenuItem_Session_Start_Click);
            // 
            // TextBoxTimeInterval
            // 
            resources.ApplyResources(this.TextBoxTimeInterval, "TextBoxTimeInterval");
            this.TextBoxTimeInterval.Name = "TextBoxTimeInterval";
            this.TextBoxTimeInterval.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBoxTimeInterval_KeyPress);
            this.TextBoxTimeInterval.Leave += new System.EventHandler(this.TextBoxTimeInterval_Leave);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // TabPageCharts
            // 
            resources.ApplyResources(this.TabPageCharts, "TabPageCharts");
            this.TabPageCharts.Controls.Add(this.TabControlCharts);
            this.TabPageCharts.Name = "TabPageCharts";
            this.TabPageCharts.UseVisualStyleBackColor = true;
            // 
            // TabControlCharts
            // 
            resources.ApplyResources(this.TabControlCharts, "TabControlCharts");
            this.TabControlCharts.Controls.Add(this.tabPage1);
            this.TabControlCharts.Name = "TabControlCharts";
            this.TabControlCharts.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripStatusLabel1,
            this.ToolStripStatusLabel2});
            this.statusStrip1.Name = "statusStrip1";
            // 
            // ToolStripStatusLabel1
            // 
            resources.ApplyResources(this.ToolStripStatusLabel1, "ToolStripStatusLabel1");
            this.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1";
            // 
            // ToolStripStatusLabel2
            // 
            resources.ApplyResources(this.ToolStripStatusLabel2, "ToolStripStatusLabel2");
            this.ToolStripStatusLabel2.Name = "ToolStripStatusLabel2";
            // 
            // MenuStrip1
            // 
            resources.ApplyResources(this.MenuStrip1, "MenuStrip1");
            this.MenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_Program,
            this.ToolStripMenuItem_Session,
            this.ToolStripMenuItem_Options});
            this.MenuStrip1.Name = "MenuStrip1";
            // 
            // ToolStripMenuItem_Program
            // 
            resources.ApplyResources(this.ToolStripMenuItem_Program, "ToolStripMenuItem_Program");
            this.ToolStripMenuItem_Program.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_Program_Close});
            this.ToolStripMenuItem_Program.Name = "ToolStripMenuItem_Program";
            // 
            // ToolStripMenuItem_Program_Close
            // 
            resources.ApplyResources(this.ToolStripMenuItem_Program_Close, "ToolStripMenuItem_Program_Close");
            this.ToolStripMenuItem_Program_Close.Name = "ToolStripMenuItem_Program_Close";
            this.ToolStripMenuItem_Program_Close.Click += new System.EventHandler(this.ToolStripMenuItem_Program_Close_Click);
            // 
            // ToolStripMenuItem_Session
            // 
            resources.ApplyResources(this.ToolStripMenuItem_Session, "ToolStripMenuItem_Session");
            this.ToolStripMenuItem_Session.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_Session_Start,
            this.ToolStripMenuItem_Session_Pause,
            this.ToolStripMenuItem_Session_Stop});
            this.ToolStripMenuItem_Session.Name = "ToolStripMenuItem_Session";
            // 
            // ToolStripMenuItem_Session_Start
            // 
            resources.ApplyResources(this.ToolStripMenuItem_Session_Start, "ToolStripMenuItem_Session_Start");
            this.ToolStripMenuItem_Session_Start.Name = "ToolStripMenuItem_Session_Start";
            this.ToolStripMenuItem_Session_Start.Click += new System.EventHandler(this.ToolStripMenuItem_Session_Start_Click);
            // 
            // ToolStripMenuItem_Session_Pause
            // 
            resources.ApplyResources(this.ToolStripMenuItem_Session_Pause, "ToolStripMenuItem_Session_Pause");
            this.ToolStripMenuItem_Session_Pause.Name = "ToolStripMenuItem_Session_Pause";
            this.ToolStripMenuItem_Session_Pause.Click += new System.EventHandler(this.ToolStripMenuItem_Session_Pause_Click);
            // 
            // ToolStripMenuItem_Session_Stop
            // 
            resources.ApplyResources(this.ToolStripMenuItem_Session_Stop, "ToolStripMenuItem_Session_Stop");
            this.ToolStripMenuItem_Session_Stop.Name = "ToolStripMenuItem_Session_Stop";
            this.ToolStripMenuItem_Session_Stop.Click += new System.EventHandler(this.ToolStripMenuItem_Session_Stop_Click);
            // 
            // ToolStripMenuItem_Options
            // 
            resources.ApplyResources(this.ToolStripMenuItem_Options, "ToolStripMenuItem_Options");
            this.ToolStripMenuItem_Options.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_Options_Configure});
            this.ToolStripMenuItem_Options.Name = "ToolStripMenuItem_Options";
            // 
            // ToolStripMenuItem_Options_Configure
            // 
            resources.ApplyResources(this.ToolStripMenuItem_Options_Configure, "ToolStripMenuItem_Options_Configure");
            this.ToolStripMenuItem_Options_Configure.Name = "ToolStripMenuItem_Options_Configure";
            this.ToolStripMenuItem_Options_Configure.Click += new System.EventHandler(this.ToolStripMenuItem_Options_Configure_Click);
            // 
            // PanelBottom
            // 
            resources.ApplyResources(this.PanelBottom, "PanelBottom");
            this.PanelBottom.Name = "PanelBottom";
            // 
            // ToolStrip1
            // 
            resources.ApplyResources(this.ToolStrip1, "ToolStrip1");
            this.ToolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripButton_SessionStart,
            this.ToolStripButton_SessionPause,
            this.ToolStripButton_SessionStop,
            this.toolStripSeparator1,
            this.ToolStripComboBoxCoinNames});
            this.ToolStrip1.Name = "ToolStrip1";
            // 
            // ToolStripButton_SessionStart
            // 
            resources.ApplyResources(this.ToolStripButton_SessionStart, "ToolStripButton_SessionStart");
            this.ToolStripButton_SessionStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripButton_SessionStart.Name = "ToolStripButton_SessionStart";
            this.ToolStripButton_SessionStart.Click += new System.EventHandler(this.ToolStripMenuItem_Session_Start_Click);
            // 
            // ToolStripButton_SessionPause
            // 
            resources.ApplyResources(this.ToolStripButton_SessionPause, "ToolStripButton_SessionPause");
            this.ToolStripButton_SessionPause.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripButton_SessionPause.Name = "ToolStripButton_SessionPause";
            this.ToolStripButton_SessionPause.Click += new System.EventHandler(this.ToolStripMenuItem_Session_Pause_Click);
            // 
            // ToolStripButton_SessionStop
            // 
            resources.ApplyResources(this.ToolStripButton_SessionStop, "ToolStripButton_SessionStop");
            this.ToolStripButton_SessionStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripButton_SessionStop.Name = "ToolStripButton_SessionStop";
            this.ToolStripButton_SessionStop.Click += new System.EventHandler(this.ToolStripMenuItem_Session_Stop_Click);
            // 
            // toolStripSeparator1
            // 
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            // 
            // ToolStripComboBoxCoinNames
            // 
            resources.ApplyResources(this.ToolStripComboBoxCoinNames, "ToolStripComboBoxCoinNames");
            this.ToolStripComboBoxCoinNames.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.ToolStripComboBoxCoinNames.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ToolStripComboBoxCoinNames.Name = "ToolStripComboBoxCoinNames";
            this.ToolStripComboBoxCoinNames.SelectedIndexChanged += new System.EventHandler(this.ToolStripComboBoxCoinNames_SelectedIndexChanged);
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Controls.Add(this.TabControlMain);
            this.panel1.Name = "panel1";
            // 
            // FormMain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ToolStrip1);
            this.Controls.Add(this.PanelBottom);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.MenuStrip1);
            this.MainMenuStrip = this.MenuStrip1;
            this.Name = "FormMain";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.TabControlMain.ResumeLayout(false);
            this.TabPageConfigure.ResumeLayout(false);
            this.GroupBoxSessionSettings.ResumeLayout(false);
            this.GroupBoxSessionSettings.PerformLayout();
            this.TabPageCharts.ResumeLayout(false);
            this.TabControlCharts.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
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

