namespace CM
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Globalization;
    using System.Windows.Forms;

    public class PrepareForm
    {
        #region Properties etc.
        private List<string> CoinName = new();                                                  //List with coin names
        public List<DataGridView> DgvNames = new();                                             // List with created datgagridview objects 
        public List<System.Windows.Forms.DataVisualization.Charting.Chart> ChartNames = new();  // List with created Chart objects
        public List<Label> LabelNames = new();                                                  // List with created Label obects
        public List<CheckBox> CheckBoxNames = new();                                            // List with created CheckBox obects
        public List<GroupBox> GroupBoxNames = new();                                            // List with created GroupBox obects

        private TabControl TabCtrl { get; set; }

        private string DecimalSeperator { get; set; }
        public double WarnPercentage { get; set; }

        private int CntrlPointX = 3;

        private readonly int CbPointY = 10;
        private int CheckTabCount { get; set; }
        #endregion Properties etc.

        #region constructor
        public PrepareForm()
        {
            this.GetCoins();
            this.DecimalSeperator = CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator;  // Move to a static class double code
        }
        #endregion constructor

        private void GetCoins()
        {
            // First check if there is an active internet connection
            if (StartSession.CheckForInternetConnection())
            {
                ApplicationDatabase coinNames = new();
                List<string> allCoinNames = coinNames.GetSelectedCoinNames();

                // The first time the application runs and the database is created this is null
                if (allCoinNames != null)
                {
                    foreach (string aCoin in allCoinNames)
                    {
                        this.CoinName.Add(aCoin);
                    }
                }
            }
        }

        public void CreateTheTabs(TabControl tCtrl)
        {
            this.TabCtrl = tCtrl;

            this.CreateTabPages();
            this.PrepareTabPages();
        }

        #region Create the tab pages
        private void CreateTabPages()
        {
            this.TabCtrl.TabPages.Clear();  // First remove all tabs

            // Create a Tab for the 24hour difference percentage
            string title = "24 uurs percentage";
            TabPage aTabPageFirst = new(title);
            aTabPageFirst.Name = title;
            this.TabCtrl.TabPages.Add(aTabPageFirst);

            // Create a tab per coin
            foreach (string value in this.CoinName)
            {
                title = value;
                TabPage aTabPage = new(title);
                aTabPage.Name = title;              // The name of the tabpage is the coin/market name. "BTC-EUR"
                this.TabCtrl.TabPages.Add(aTabPage);
            }
        }

        #endregion Create the tab pages

        private void PrepareTabPages()
        {
            for (int tabcount = 0; tabcount <= this.TabCtrl.TabCount - 1; tabcount++)
            {
                if (tabcount == 0)
                {
                    this.CreateGroupBox(tabcount, "Geselecteerde_munten");
                    this.CreateGroupBox(tabcount, "Overige_munten");
                    this.CreateDgv24hourPercDiffCoins(tabcount, "Dgv_DifPerc24hourSelected");
                    this.CreateDgv24hourPercDiffCoins(tabcount, "Dgv_DifPerc24hourNotSelected");
                }
                else
                {
                    this.CreateSplitContainer(tabcount);                                                 // Create the splitcontaineers
                    this.CreateDatagridViewPriceData(tabcount);                                          // Create DataGrid view
                    this.CreateChartPanel(tabcount);                                                     // Create the panel on which the chart will be placed. The control with dockstyle = filled must FIRST be placed on the paren control
                    this.CreateBottomPanel(tabcount);                                                    // Create the panel where checkboxes will be placed
                    this.CreateCheckBox(tabcount, "CheckBoxShowStartPrice", "Start prijs aan/uit");      // Create a check box

                    this.CreateCheckBox(tabcount, "CheckBoxShowStartPrice", "Open prijs aan/uit");       // Create a check box
                    this.CreateCheckBox(tabcount, "CheckBoxShowStartPrice", "Sessie hoogste aan/uit");   // Create a check box
                    this.CreateCheckBox(tabcount, "CheckBoxShowStartPrice", "Sessie laagste aan/uit");   // Create a check box

                    this.CreateDatagridViewPriceMonitorPrice(tabcount);                                  // Create DataGrid view
                    this.CreateChart(tabcount);                                                          // Create the charts
                    this.CreateLabels(tabcount, this.TabCtrl.TabPages[tabcount].Text);
                }
            }
        }

        private void CreateSplitContainer(int tabcount)
        {
            // Create the first splitcontainer
            SplitContainer spltcontainer = new();
            TabPage tp = this.TabCtrl.TabPages[tabcount];

            spltcontainer.Orientation = Orientation.Vertical;
            spltcontainer.SplitterDistance = 60;
            spltcontainer.Dock = DockStyle.Fill;
            spltcontainer.SplitterWidth = 10;
            spltcontainer.BackColor = Color.White;
            spltcontainer.BorderStyle = BorderStyle.FixedSingle;
            spltcontainer.SplitterDistance = 30;

            tp.Controls.Add(spltcontainer);  // Add the splitcontainer  componentlist of the tabpage

            // Create the second splitcontainer and place in in panel1 of the first splitcontainer.
            SplitContainer spltcontainer1 = new()
            {
                Orientation = Orientation.Horizontal,
                SplitterDistance = 125,
                Dock = DockStyle.Fill,
                SplitterWidth = 10,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
            };

            spltcontainer.Panel1.Controls.Add(spltcontainer1);
        }

        private void CreateDatagridViewPriceData(int tabcount)
        {
            TabPage tp = this.TabCtrl.TabPages[tabcount];
            DataGridView dgv = new();
            dgv.Name = "Dgv_1_" + tp.Name;
            dgv.Columns.Add("Onderdeel", "Onderdeel");  // = dgv.Columns[0]
            dgv.Columns.Add("Waarde", "Waarde");        // = dgv.Columns[1]
            dgv.Location = new Point(3, 3);
            dgv.Height = 150;
            dgv.TabIndex = 0;
            dgv.Dock = DockStyle.Fill;

            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToOrderColumns = false;
            dgv.EditMode = DataGridViewEditMode.EditProgrammatically;

            foreach (DataGridViewColumn column in dgv.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            this.DgvNames.Add(dgv);

            dgv.Columns[0].Width = 130;
            dgv.Columns[1].Width = 70;

            // align header(s)
            // dgv.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

            // align cell(s)
            dgv.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // Add the rows
            dgv.Rows.Add("Start prijs", "0");               // 0
            dgv.Rows.Add(string.Empty, string.Empty);       // 1
            dgv.Rows.Add("Huidige prijs", "0");             // 2
            dgv.Rows.Add("Verschil [€]", "0");              // 3
            dgv.Rows.Add("Verschil [%]", "0");              // 4
            dgv.Rows.Add(string.Empty, string.Empty);       // 5
            dgv.Rows.Add(string.Format("Koers bij {0}% winst", this.WarnPercentage.ToString()), "0");           // 6
            dgv.Rows.Add(string.Format("Koers bij {0}% verlies", this.WarnPercentage.ToString()), "0");         // 7
            dgv.Rows.Add(string.Empty, string.Empty);       // 8

            dgv.Rows.Add("Hoogste (sessie)", "0");          // 9
            dgv.Rows.Add("Laagste (sessie)", "0");          // 10
            dgv.Rows.Add(string.Empty, string.Empty);       // 11

            dgv.Rows.Add("Open (24 uur geleden)", "0");     // 12
            dgv.Rows.Add("Hoogste", "0");                   // 13
            dgv.Rows.Add("Laagste", "0");                   // 14
            dgv.Rows.Add("Volume", "0");                    // 15
            dgv.Rows.Add("Volume quote", "0");              // 16
            dgv.Rows.Add("Bieden", "0");                    // 17
            dgv.Rows.Add("Vraag", "0");                     // 18
            dgv.Rows.Add("Bied grootte", "0");              // 19
            dgv.Rows.Add("Vraag grootte", "0");             // 20

            dgv.Rows[1].Height = 7;
            dgv.Rows[5].Height = 7;
            dgv.Rows[8].Height = 7;
            dgv.Rows[11].Height = 7;

            dgv.Rows[1].Cells[0].Style.BackColor = Color.LightGray;
            dgv.Rows[1].Cells[1].Style.BackColor = Color.LightGray;

            dgv.Rows[5].Cells[0].Style.BackColor = Color.LightGray;
            dgv.Rows[5].Cells[1].Style.BackColor = Color.LightGray;

            dgv.Rows[8].Cells[0].Style.BackColor = Color.LightGray;
            dgv.Rows[8].Cells[1].Style.BackColor = Color.LightGray;

            dgv.Rows[11].Cells[0].Style.BackColor = Color.LightGray;
            dgv.Rows[11].Cells[1].Style.BackColor = Color.LightGray;

            dgv.RowHeadersVisible = false;

            foreach (Control c in tp.Controls)
            {
                if (c.GetType() == typeof(SplitContainer))
                {
                    SplitContainer splt1 = (SplitContainer)c;
                    foreach (Control c1 in splt1.Panel1.Controls)
                    {
                        if (c1.GetType() == typeof(SplitContainer))
                        {
                            SplitContainer splt2 = (SplitContainer)c1;
                            splt2.Panel1.Controls.Add(dgv);
                        }
                    }
                }
            }
        }

        private void CreateDatagridViewPriceMonitorPrice(int tabcount)
        {
            DataGridView dgv = new();
            TabPage tp = this.TabCtrl.TabPages[tabcount];
            dgv.Name = "Dgv_2_" + tp.Name;
            dgv.Columns.Add("Prijs", "Prijs");
            dgv.Columns.Add("Empty_1", "    ");
            dgv.Columns.Add("Datum_tijd", "Datum - tijd");

            dgv.Columns[0].ToolTipText = "Huidige prijs.";
            dgv.Columns[1].ToolTipText = "Trend ten opzichte van de vorige prijs.";

            dgv.Columns[0].Width = 60;
            dgv.Columns[1].Width = 20;
            dgv.Columns[2].Width = 120;

            dgv.Location = new Point(3, 3);
            dgv.Height = 150;
            dgv.TabIndex = 0;
            dgv.Dock = DockStyle.Fill;

            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToOrderColumns = false;
            dgv.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgv.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.RowHeadersVisible = false;

            this.DgvNames.Add(dgv);

           // add the datagrid view to the splitcontainer (left panel);
            foreach (Control c in tp.Controls)
            {
                if (c.GetType() == typeof(SplitContainer))
                {
                    SplitContainer splt1 = (SplitContainer)c;
                    foreach (Control c1 in splt1.Panel1.Controls)
                    {
                        if (c1.GetType() == typeof(SplitContainer))
                        {
                            SplitContainer splt2 = (SplitContainer)c1;
                            splt2.Panel2.Controls.Add(dgv);
                        }
                    }
                }
            }
        }

        private void CreateDgv24hourPercDiffCoins(int tabcount, string dgvName)
        {
            DataGridView dgv = new();
            TabPage tp = this.TabCtrl.TabPages[tabcount];
            dgv.Name = dgvName;
            dgv.Columns.Add("Coin", "Coin");
            dgv.Columns.Add("Percentage", "Percentage");

            dgv.Columns[0].Width = 70;
            dgv.Columns[1].Width = 80;

            dgv.Location = new Point(3, 3);
            dgv.Height = tp.Height;
            dgv.Width = 193;
            dgv.TabIndex = 0;

            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToOrderColumns = true;
            dgv.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgv.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.RowHeadersVisible = false;
            dgv.Dock = DockStyle.Fill;

            this.DgvNames.Add(dgv);
            this.PlaceControlOnGroupBox(dgv, tabcount, dgvName);
        }

        private void CreateChart(int tabcount)
        {
            // source: https://timbar.blogspot.com/2012/04/creating-chart-programmatically-in-c.html
            // source: https://hirenkhirsaria.blogspot.com/2012/06/dynamically-creating-piebar-chart-in-c.html
            TabPage tp = this.TabCtrl.TabPages[tabcount];
            string CoinName = tp.Name;

            // Chart does not exists in .net 5 !?!? used a nuget download
            System.Windows.Forms.DataVisualization.Charting.Chart aChart = new();
            aChart.Size = new Size(100, 100);
            aChart.Name = "Chart_" + tp.Name;
            aChart.Titles.Add(CoinName);
            aChart.Location = new Point(3, 3);
            aChart.Cursor = Cursors.Cross;

            System.Windows.Forms.DataVisualization.Charting.ChartArea ChrtArea = new("ChartArea_" + CoinName);
            ChrtArea.Area3DStyle.Enable3D = false;
            aChart.ChartAreas.Add(ChrtArea);
            ChrtArea.AxisX.Title = "Tijd";
            ChrtArea.AxisY.Title = "Koers";

            aChart.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            aChart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;

            aChart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;
            aChart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;

            // The current coin price
            System.Windows.Forms.DataVisualization.Charting.Series series = new("Series_" + CoinName)
            {
                Name = "Series_" + CoinName,
                ChartArea = "ChartArea_" + CoinName,
            };
            aChart.Series.Add(CoinName);
            aChart.Series[CoinName].Enabled = true;

            aChart.Series[CoinName].MarkerColor = Color.Red;
            aChart.Series[CoinName].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            aChart.Series[CoinName].MarkerSize = 5;

            // Start_Prijs
            System.Windows.Forms.DataVisualization.Charting.Series SeriesStartPrice = new("Start_Prijs_" + CoinName);
            SeriesStartPrice.Name = "Start_Prijs_" + CoinName;
            SeriesStartPrice.LegendText = "Start_Prijs";
            aChart.Series.Add(SeriesStartPrice);
            aChart.Series["Start_Prijs_" + CoinName].Enabled = true;

            // aChart.Series["Start_Prijs_" + CoinName].ToolTip = "hello world from: € #VAL,   #VALX";

            // Open_Prijs
            System.Windows.Forms.DataVisualization.Charting.Series SeriesOpenPrice = new("Open_Prijs_" + CoinName);
            SeriesOpenPrice.Name = "Open_Prijs_" + CoinName;
            SeriesOpenPrice.LegendText = "Open_Prijs";
            aChart.Series.Add(SeriesOpenPrice);
            aChart.Series["Open_Prijs_" + CoinName].Enabled = true;

            // Sessie_Hoogste_Prijs
            System.Windows.Forms.DataVisualization.Charting.Series SeriesSessionHighestPrice = new("Sessie_Hoogste_Prijs_" + CoinName);
            SeriesSessionHighestPrice.Name = "Sessie_Hoogste_Prijs_" + CoinName;
            SeriesSessionHighestPrice.LegendText = "Sessie_Hoogste";
            aChart.Series.Add(SeriesSessionHighestPrice);
            aChart.Series["Sessie_Hoogste_Prijs_" + CoinName].Enabled = true;

            // Sessie_Laagste_Prijs
            System.Windows.Forms.DataVisualization.Charting.Series SeriesSessionLowestPrice = new("Sessie_Laagste_Prijs_" + CoinName);
            SeriesSessionLowestPrice.Name = "Sessie_Laagste_Prijs_" + CoinName;
            SeriesSessionLowestPrice.LegendText = "Sessie_Laagste";
            aChart.Series.Add(SeriesSessionLowestPrice);
            aChart.Series["Sessie_Laagste_Prijs_" + CoinName].Enabled = true;

            // Create the legend
            aChart.Legends.Add(new System.Windows.Forms.DataVisualization.Charting.Legend("Price"));
            aChart.Legends[0].TableStyle = System.Windows.Forms.DataVisualization.Charting.LegendTableStyle.Auto;
            aChart.Legends[0].Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            aChart.Legends[0].Alignment = System.Drawing.StringAlignment.Center;
            aChart.Series[0].Legend = "Price";

            aChart.ChartAreas[0].AxisX.IsStartedFromZero = false;
            aChart.ChartAreas[0].AxisY.IsStartedFromZero = false;

            aChart.Visible = true;
            aChart.Invalidate();

            this.ChartNames.Add(aChart);

            foreach (Control c in tp.Controls)
            {
                if (c.GetType() == typeof(SplitContainer))
                {
                    SplitContainer splt1 = (SplitContainer)c;

                    foreach (Control c1 in splt1.Panel2.Controls)
                    {
                        if (c1.GetType() == typeof(Panel))
                        {
                            if (c1.Name == "ChartPanel")
                            {
                                Panel aPanel = (Panel)c1;

                                aPanel.Controls.Add(aChart);

                                aChart.Dock = DockStyle.Fill;
                            } 
                        }
                    }

                    // splt1.Panel2.Controls.Add(aChart);
                }
            }
        }

        #region Textbox warning precentage

        private void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.KeyPresstextBox(sender, e);
        }

        private void KeyPresstextBox(object sender, KeyPressEventArgs e)
        {
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

        #endregion Textbox warning precentage

        private void PlaceControleOnSpltcontainerOne(Control newCntrl, int tabCount)
        {
            TabPage tp = this.TabCtrl.TabPages[tabCount];
            foreach (Control c in tp.Controls)
            {
                if (c.GetType() == typeof(SplitContainer))
                {
                    SplitContainer splt2 = (SplitContainer)c;
                    splt2.Panel2.Controls.Add(newCntrl);
                }
            }
        }

        private void PlaceControlOnSpltcontainerOnePanel(Control newCntrl, int tabCount)
        {
            TabPage tp = this.TabCtrl.TabPages[tabCount];
            foreach (Control c in tp.Controls)
            {
                if (c.GetType() == typeof(SplitContainer))
                {
                    SplitContainer splt1 = (SplitContainer)c;
                    foreach (Control c1 in splt1.Panel2.Controls)
                    {
                        if (c1.GetType() == typeof(Panel))
                        {
                            Panel aPanel = (Panel)c1;
                            if (aPanel.Name == "BottomPanel")
                            {
                                aPanel.Controls.Add(newCntrl);
                            }
                        }
                    }
                }
            }
        }

        private void PlaceControlOnChart(Control newCntrl, int tabCount)
        {
            // System.Windows.Forms.DataVisualization.Charting.Chart
            TabPage tp = this.TabCtrl.TabPages[tabCount];
            foreach (Control c in tp.Controls)
            {
                if (c.GetType() == typeof(SplitContainer))
                {
                    SplitContainer splt1 = (SplitContainer)c;
                    foreach (Control c1 in splt1.Panel2.Controls)
                    {
                        if (c1.GetType() == typeof(Panel))
                        {
                            Panel aPanel = (Panel)c1;
                            foreach (Control c2 in aPanel.Controls)
                            {
                                if (c2.GetType() == typeof(System.Windows.Forms.DataVisualization.Charting.Chart))
                                {
                                    System.Windows.Forms.DataVisualization.Charting.Chart aChart = (System.Windows.Forms.DataVisualization.Charting.Chart)c2;
                                    aChart.Controls.Add(newCntrl);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void CreateBottomPanel(int tabcount)
        {
            Panel BottomPanel = new();
            BottomPanel.Name = "BottomPanel";
            BottomPanel.Text = string.Empty;
            BottomPanel.Location = new Point(3, 50);
            BottomPanel.Size = new Size(50, 32);
            BottomPanel.BackColor = Color.LightSteelBlue;
            BottomPanel.Dock = DockStyle.Bottom;

            this.PlaceControleOnSpltcontainerOne(BottomPanel, tabcount);
        }

        private void CreateChartPanel(int tabcount)

        {
            Panel ChartPanel = new();
            ChartPanel.Name = "ChartPanel";
            ChartPanel.Text = string.Empty;
            ChartPanel.Location = new Point(3, 50);
            ChartPanel.Size = new Size(50, 32);
            ChartPanel.BackColor = Color.LightGray;
            ChartPanel.Dock = DockStyle.Fill;

            this.PlaceControleOnSpltcontainerOne(ChartPanel, tabcount);
        }

        private void CreateCheckBox(int tabcount, string Name, string Text)
        {
            if (tabcount != this.CheckTabCount) // Next tab page, reset Y
            {
                this.CheckTabCount = tabcount;
                this.CntrlPointX = 3;
            }

            CheckBox Cb = new();
            Cb.Name = Name;
            Cb.Text = Text;
            Cb.Checked = true;
            Cb.AutoSize = true;
            if (this.CntrlPointX == 0)
            {
                Cb.Location = new Point(this.CntrlPointX, this.CbPointY);
            }
            else
            {
                Cb.Location = new Point(this.CntrlPointX, this.CbPointY);
            }

            this.CntrlPointX += 150;

            Cb.Size = new Size(82, 19);
            Cb.Anchor = (AnchorStyles.Left);

            this.CheckBoxNames.Add(Cb);
            this.PlaceControlOnSpltcontainerOnePanel(Cb, tabcount);
        }

        private void CreateLabels(int tabcount, string name)
        {
            Label Lb = new();
            Lb.Name = "Lb_" + name;
            Lb.Text = "LabelYaxisCur";
            Lb.AutoSize = true;
            Lb.Location = new Point(50, 50);
            Lb.Size = new Size(38, 15);
            Lb.Visible = false;

            Lb.BorderStyle = BorderStyle.FixedSingle;
            Lb.BackColor = Color.LightBlue;
            Lb.Font = new Font("Calibri", 10);
            Lb.ForeColor = Color.DarkBlue;

            this.LabelNames.Add(Lb);
            this.PlaceControlOnChart(Lb, tabcount);
        }

        private void CreateGroupBox(int tabcount, string name)
        {
            TabPage tp = this.TabCtrl.TabPages[tabcount];

            GroupBox Gb = new();
            Gb.Name = "Gb_" + name;
            Gb.Text = name.Replace("_", " ");

            if (this.CntrlPointX == 3)
            {
                Gb.Location = new Point(this.CntrlPointX + 2, 5);
                this.CntrlPointX += 200 + 2;
            }
            else
            {
                Gb.Location = new Point(this.CntrlPointX, 5);
            }

            int GbHeight = tp.Height - 15;

            Gb.Size = new Size(175, GbHeight);
            Gb.Anchor = (AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom);

            this.GroupBoxNames.Add(Gb);

            tp.Controls.Add(Gb);
        }

        private void PlaceControlOnGroupBox(Control newCntrl, int tabCount, string dgvName)
        {
            TabPage tp = this.TabCtrl.TabPages[tabCount];
            foreach (Control c in tp.Controls)
            {
                if (c.GetType() == typeof(GroupBox))
                {
                    GroupBox grpb = (GroupBox)c;
                    if (grpb.Name.Contains("Gb_Geselecteerde") && dgvName == "Dgv_DifPerc24hourSelected")
                    {
                        grpb.Controls.Add(newCntrl);
                    }
                    else if (grpb.Name.Contains("Overige_munten") && dgvName == "Dgv_DifPerc24hourNotSelected")
                    {
                        grpb.Controls.Add(newCntrl);
                    }
                }
            }
        }
    }
}