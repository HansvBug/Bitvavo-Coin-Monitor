namespace CM
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Globalization;
    using System.Windows.Forms;

    public class PrepareForm
    {
        #region Properties etc.
        private List<string> coinName = new();                                                  // List with coin names
        public List<DataGridView> DgvNames = new();                                             // List with created datgagridview objects 
        public List<System.Windows.Forms.DataVisualization.Charting.Chart> ChartNames = new();  // List with created Chart objects
        public List<Label> LabelNames = new();                                                  // List with created Label obects
        public List<CheckBox> CheckBoxNames = new();                                            // List with created CheckBox obects
        public List<GroupBox> GroupBoxNames = new();                                            // List with created GroupBox obects

        private TabControl TabCtrl { get; set; }

        private string DecimalSeperator { get; set; }

        /// <summary>
        /// Gets or sets the warn percentage.
        /// Notify the user when the price has risen or fallen a certain percentage.
        /// </summary>
        public double WarnPercentage { get; set; }

        private int controlPointX = 3;

        private readonly int chkbPointY = 3;  // TODO; make a setting

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
                        this.coinName.Add(aCoin);
                    }
                }
            }
        }

        /// <summary>
        /// Create the tabpages.
        /// </summary>
        /// <param name="tCtrl">TabControl which gets the new pages.</param>
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
            TabPage aTabPageFirst = new(title)
            {
                Name = title,
            };
            this.TabCtrl.TabPages.Add(aTabPageFirst);

            // Create a tab per coin
            foreach (string value in this.coinName)
            {
                title = value;
                TabPage aTabPage = new(title)
                {
                    Name = title,              // The name of the tabpage is the coin/market name. "BTC-EUR"
                };
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

            DataGridView dgv = new()
            {
                Name = "Dgv_1_" + tp.Name,
                Location = new Point(3, 3),
                Height = 150,
                TabIndex = 0,
                Dock = DockStyle.Fill,
                AllowUserToDeleteRows = false,
                AllowUserToAddRows = false,
                AllowUserToOrderColumns = false,
                AllowUserToResizeRows = false,
                EditMode = DataGridViewEditMode.EditProgrammatically,
            };

            dgv.Columns.Add("Onderdeel", "Onderdeel");  // = dgv.Columns[0]
            dgv.Columns.Add("Waarde", "Waarde");        // = dgv.Columns[1]
            dgv.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

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
            TabPage tp = this.TabCtrl.TabPages[tabcount];
            DataGridView dgv = new()
            {
                Name = dgvName,
                Location = new Point(3, 3),
                Height = tp.Height,
                Width = 193,
                TabIndex = 0,
                AllowUserToDeleteRows = false,
                AllowUserToAddRows = false,
                AllowUserToOrderColumns = true,
                EditMode = DataGridViewEditMode.EditProgrammatically,
                RowHeadersVisible = false,
                Dock = DockStyle.Fill,
            };

            dgv.Columns.Add("Coin", "Coin");
            dgv.Columns.Add("Percentage", "Percentage");

            dgv.Columns[0].Width = 170; //TODO make 170 a setting
            dgv.Columns[1].Width = 100; //TODO make 100 a setting
            dgv.Columns[0].CellTemplate.ValueType = typeof(int);  // TODO; sorting.... this line should improve sorting but is does not. Negative an positive numers are not sorted correct. 
            dgv.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgv.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.DgvNames.Add(dgv);
            this.PlaceControlOnGroupBox(dgv, tabcount, dgvName);
        }

        private void CreateChart(int tabcount)
        {
            // source: https://timbar.blogspot.com/2012/04/creating-chart-programmatically-in-c.html
            // source: https://hirenkhirsaria.blogspot.com/2012/06/dynamically-creating-piebar-chart-in-c.html
            TabPage tp = this.TabCtrl.TabPages[tabcount];
            string coinName = tp.Name;

            // Chart does not exists in .net 5 !?!? used a nuget download
            System.Windows.Forms.DataVisualization.Charting.Chart aChart = new()
            {
                Size = new Size(100, 100),
                Name = "Chart_" + tp.Name,
                Location = new Point(3, 3),
                Cursor = Cursors.Cross,
            };
            aChart.Titles.Add(coinName);

            System.Windows.Forms.DataVisualization.Charting.ChartArea chrtArea = new("ChartArea_" + coinName);
            chrtArea.Area3DStyle.Enable3D = false;
            aChart.ChartAreas.Add(chrtArea);
            chrtArea.AxisX.Title = "Tijd";
            chrtArea.AxisY.Title = "Koers";

            aChart.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            aChart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;

            aChart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;
            aChart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;

            // The current coin price
            System.Windows.Forms.DataVisualization.Charting.Series series = new("Series_" + coinName)
            {
                Name = "Series_" + coinName,
                ChartArea = "ChartArea_" + coinName,
            };
            aChart.Series.Add(coinName);
            aChart.Series[coinName].Enabled = true;

            aChart.Series[coinName].MarkerColor = Color.Red;
            aChart.Series[coinName].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            aChart.Series[coinName].MarkerSize = 5;

            // Start_Prijs
            System.Windows.Forms.DataVisualization.Charting.Series seriesStartPrice = new("Start_Prijs_" + coinName)
            {
                Name = "Start_Prijs_" + coinName,
                LegendText = "Start_Prijs",
            };
            aChart.Series.Add(seriesStartPrice);
            aChart.Series["Start_Prijs_" + coinName].Enabled = true;

            // aChart.Series["Start_Prijs_" + CoinName].ToolTip = "hello world from: € #VAL,   #VALX";

            // Open_Prijs
            System.Windows.Forms.DataVisualization.Charting.Series seriesOpenPrice = new("Open_Prijs_" + coinName)
            {
                Name = "Open_Prijs_" + coinName,
                LegendText = "Open_Prijs",
            };
            aChart.Series.Add(seriesOpenPrice);
            aChart.Series["Open_Prijs_" + coinName].Enabled = true;

            // Sessie_Hoogste_Prijs
            System.Windows.Forms.DataVisualization.Charting.Series seriesSessionHighestPrice = new("Sessie_Hoogste_Prijs_" + coinName)
            {
                Name = "Sessie_Hoogste_Prijs_" + coinName,
                LegendText = "Sessie_Hoogste",
            };
            aChart.Series.Add(seriesSessionHighestPrice);
            aChart.Series["Sessie_Hoogste_Prijs_" + coinName].Enabled = true;

            // Sessie_Laagste_Prijs
            System.Windows.Forms.DataVisualization.Charting.Series seriesSessionLowestPrice = new("Sessie_Laagste_Prijs_" + coinName)
            {
                Name = "Sessie_Laagste_Prijs_" + coinName,
                LegendText = "Sessie_Laagste",
            };
            aChart.Series.Add(seriesSessionLowestPrice);
            aChart.Series["Sessie_Laagste_Prijs_" + coinName].Enabled = true;

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
            Panel bottomPanel = new()
            {
                Name = "BottomPanel",
                Text = string.Empty,
                Location = new Point(3, 50),
                Size = new Size(50, 40),  // ToDo; make 40  a setting
                BackColor = Color.LightSteelBlue,
                Dock = DockStyle.Bottom,
            };

            this.PlaceControleOnSpltcontainerOne(bottomPanel, tabcount);
        }

        private void CreateChartPanel(int tabcount)
        {
            Panel chartPanel = new()
            {
                Name = "ChartPanel",
                Text = string.Empty,
                Location = new Point(3, 50),
                Size = new Size(50, 32),
                BackColor = Color.LightGray,
                Dock = DockStyle.Fill,
            };

            this.PlaceControleOnSpltcontainerOne(chartPanel, tabcount);
        }

        private void CreateCheckBox(int tabcount, string name, string text)
        {
            if (tabcount != this.CheckTabCount)
            {
                this.CheckTabCount = tabcount;
                this.controlPointX = 3;
            }

            CheckBox cb = new()
            {
                Name = name,
                Text = text,
                Checked = true,
                AutoSize = true,
                Size = new Size(82, 19),
                Anchor = AnchorStyles.Left,
            };

            if (this.controlPointX == 0)
            {
                cb.Location = new Point(this.controlPointX, this.chkbPointY);
            }
            else
            {
                cb.Location = new Point(this.controlPointX, this.chkbPointY);
            }

            this.controlPointX += 275;  // TODO make 250 a setting

            this.CheckBoxNames.Add(cb);
            this.PlaceControlOnSpltcontainerOnePanel(cb, tabcount);
        }

        private void CreateLabels(int tabcount, string name)
        {
            Label lb = new()
            {
                Name = "Lb_" + name,
                Text = "LabelYaxisCur",
                AutoSize = true,
                Location = new Point(50, 50),
                Size = new Size(38, 15),
                Visible = false,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.LightBlue,
                Font = new Font("Calibri", 10),
                ForeColor = Color.DarkBlue,
            };

            this.LabelNames.Add(lb);
            this.PlaceControlOnChart(lb, tabcount);
        }

        private void CreateGroupBox(int tabcount, string name)
        {
            const int groupboxwith = 330; //TODO make 330a setting
            TabPage tp = this.TabCtrl.TabPages[tabcount];

            GroupBox gb = new()
            {
                Name = "Gb_" + name,
                Text = name.Replace("_", " "),
            };

            if (this.controlPointX == 3)
            {
                gb.Location = new Point(this.controlPointX + 5, 5); //TODO make the first 5 a setting
                this.controlPointX += (groupboxwith + 5) + 2;
            }
            else
            {
                gb.Location = new Point(this.controlPointX + 10, 5);
            }

            int gbHeight = tp.Height - 15;

            gb.Size = new Size(groupboxwith, gbHeight);
            gb.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom;

            this.GroupBoxNames.Add(gb);

            tp.Controls.Add(gb);
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