using CM.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Hierarchy;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CM
{
    public partial class FormMain : Form
    {
        #region Prperties etc

        /// <summary>
        /// Gets or sets the application settings.
        /// </summary>
        public dynamic JsonObjSettings { get; set; } // Holds the user and application setttings

        private StartSession startSession;

        private PrepareForm prepareFrm;

        public CoinDataAll AllCoinDataSelectedCoins;

        private MarketPrice CoinMarketPrice;

        private readonly string doubleFormatString = "#.############";  // TODO make optional

        double timmerTime;

        private double TimerTime
        {
            get
            {
                return this.timmerTime;
            }

            set
            {
                this.timmerTime = Convert.ToDouble(value * 60 * 1000);    // millisec = minuten * 60 * 1000;
            }
        }

        private bool FirstRun { get; set; }

        private string DecimalSeperator { get; set; }

        private double WarnPercentage { get; set; }

        private string ActiveCoinName { get; set; }

        private int ActivetabpageIndex { get; set; }

        private bool SoundUp { get; set; }

        private bool SoundDown { get; set; }

        private bool SoundUpIsPlayed { get; set; }

        private bool SoundDownIsPlayed { get; set; }

        #endregion Properties etc

        /// <summary>
        /// Initializes a new instance of the <see cref="FormMain"/> class.
        /// The main form of Topdata.
        /// </summary>
        public FormMain()
        {
            this.InitializeComponent();
            GetDebugMode();
            this.FirstRun = true;
            CheckFolders();             // Create in appdata a new folder Settings and/or Database if needed
            CreateSettingsFile();       // Create the settings file if needed
            this.GetSettings();         // Get the settings a user saved
            this.StartLogging();        // Start the logging
            this.ApplySettings();
            this.CheckAppDatabase();    // Check if the database file exists and/or it is up to date with the last version.
            this.BackColor = SystemColors.Window;
            this.Text = AppSettingsDefault.ApplicationName;
            this.DoubleBuffered = true;
            this.StartClock();          // Show the date and time in the statusstrip
            this.PanelBottom.Visible = false;    // Not used
        }

        #region Form load
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Initialize();
        }

        private void Initialize()
        {
            this.DecimalSeperator = CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator;

            this.CreateTheTabs();
            this.PrepareChart();
            this.LoadFormPosition();     // Load the last saved form position.

            string DbLocation = this.JsonObjSettings.AppParam[0].DatabaseLocation;
            string AppDb = Path.Combine(DbLocation, AppSettingsDefault.SqlLiteDatabaseName);

            if (!File.Exists(AppDb))
            {
                // Check to see if the application database exists.
                MessageBox.Show(
                    "De applicatie database is niet gevonden. Controleer het log bestand." + Environment.NewLine +
                    Environment.NewLine +
                    "Het programma wordt afgesloten.",
                    "Fout",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                Application.Exit(); // Ends program major error.
            }
            else
            {
                this.CopyAppDatabase();      // Every xx times the application starts it makes a copy of the application database
            }

            this.ToolStripComboBoxCoinNames.Text = this.TabControlCharts.SelectedTab.Name;
            this.ActiveCoinName = this.TabControlCharts.SelectedTab.Name;

            this.AddEventHandlerToPriceCheckBox();
            this.AddEventHandlerToChart();

            this.CheckBoxSoundPositive.Text = string.Format("Geluid bij een stijging van {0} %", this.WarnPercentage.ToString());
            this.CheckBoxSoundNegative.Text = string.Format("Geluid bij een daling van {0} %", this.WarnPercentage.ToString());
        }

        private static void GetDebugMode()
        {
            using ProcessArguments getArg = new();
            foreach (string arg in getArg.cmdLineArg)
            {
                string argument = Convert.ToString(arg, CultureInfo.InvariantCulture);

                if (argument == getArg.ArgDebug)
                {
                    CmDebugMode.DebugMode = true;
                }
            }
        }

        private void CreateTheTabs()
        {
            this.WarnPercentage = Convert.ToDouble(this.TextBoxWarnPercentage.Text);

            this.prepareFrm = new PrepareForm
            {
                WarnPercentage = this.WarnPercentage,
            };

            this.prepareFrm.CreateTheTabs(this.TabControlCharts);

            this.HandleControlsState(SessionAction.Stop.ToString()); // Set buttons on/off
            this.TabControlMain.SelectedIndex = 0;
            this.PutCoinNamesInComboBox();
        }

        private void CheckAppDatabase()
        {
            string dbLocation = this.JsonObjSettings.AppParam[0].DatabaseLocation;
            if (!File.Exists(Path.Combine(dbLocation, AppSettingsDefault.SqlLiteDatabaseName)))
            {
                Logging.WriteToLogWarning("De applicatie database ontbreekt en wordt aangemaakt.");
                this.CreateAppDatabase();    // if the app databse doesn't exist then create it. Only when the application starts with parameter Install
                this.CheckAppDbaseVersion(); // check the version of the application database (and update it if needed)
            }
            else
            {
                Logging.WriteToLogInformation("De applicatie database is aanwezig.");

                this.CheckAppDbaseVersion(); // Check the version of the application database (and update it if needed)
            }

            GetSQliteVersion();     // Set de SQlite version in the log file
        }

        private void CheckAppDbaseVersion()
        {
            Cursor.Current = Cursors.WaitCursor;

            string DbLocation = this.JsonObjSettings.AppParam[0].DatabaseLocation;
            string DBFilePath = Path.Combine(DbLocation, AppSettingsDefault.SqlLiteDatabaseName);

            if (File.Exists(DBFilePath))
            {
                Logging.WriteToLogInformation("De applicatie database is aanwezig op locatie: " + DBFilePath);
                using ApplicationDatabase AppDb = new();

                if (AppDb.SelectMeta() < AppSettingsDefault.UpdateVersion)
                {
                    string argument = string.Empty;

                    using ProcessArguments getArg = new();
                    foreach (string arg in getArg.cmdLineArg)
                    {
                        argument = Convert.ToString(arg, CultureInfo.InvariantCulture);
                        if (argument == getArg.ArgIntall)
                        {
                            if (AppDb.UpdateDatabase())
                            {
                                // LockProgram = false;
                            }
                            else
                            {
                                // LockProgram = true;
                            }
                        }
                    }

                    MessageBox.Show("De Applicatie database is gewijzigd.", "Informatie.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                Logging.WriteToLogError("Database bestand niet aangetroffen tijdens de versie controle.");
            }

            Cursor.Current = Cursors.Default;
        }

        private void CreateAppDatabase()
        {
            Cursor.Current = Cursors.WaitCursor;
            string argument;

            using ProcessArguments getArg = new();
            string dbLocation = this.JsonObjSettings.AppParam[0].DatabaseLocation;
            using ApplicationDatabase AppDb = new();

            foreach (string arg in getArg.cmdLineArg)
            {
                argument = Convert.ToString(arg, CultureInfo.InvariantCulture);
                if (argument == getArg.ArgIntall)
                {
                    if (!File.Exists(Path.Combine(dbLocation, AppSettingsDefault.SqlLiteDatabaseName)))
                    {
                        if (AppDb.CreateNewDatabase())
                        {
                            MessageBox.Show("De Applicatie database is aangemaakt.", "Informatie.", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // LockProgram = false;
                        }
                        else
                        {
                            // LockProgram = true;
                        }
                    }
                }
                else
                {
                    if (CmDebugMode.DebugMode)
                    {
                        Logging.WriteToLogInformation("Geen passende argumenten gevonden.");
                    }
                }
            }

            Cursor.Current = Cursors.Default;
        }

        private static void GetSQliteVersion()
        {
            using ApplicationDatabase AppDb = new();
            Logging.WriteToLogInformation("SQLite versie: " + AppDb.GetSQliteVersion());
            Logging.WriteToLogInformation(string.Empty);
        }

        private void CopyAppDatabase()
        {
            Cursor.Current = Cursors.WaitCursor;
            int counter = this.JsonObjSettings.AppParam[0].CopyAppDataBaseAfterEveryXStartupsCounter;
            counter++;  // Add a new start to the counter.

            int copyAppDataBaseAfterEveryXStartups = this.JsonObjSettings.AppParam[0].CopyAppDataBaseAfterEveryXStartups;
            if (CmDebugMode.DebugMode)
            {
                Logging.WriteToLogDebug("De teller voor het aanmaken van een kopie van de applicatie database staat op : " + Convert.ToString(counter, CultureInfo.InvariantCulture));
                Logging.WriteToLogDebug("De database wordt gekopieerd bij elke xx keer opstarten : " + Convert.ToString(copyAppDataBaseAfterEveryXStartups, CultureInfo.InvariantCulture));
            }

            if (counter >= copyAppDataBaseAfterEveryXStartups)
            {
                // If counter equals the option setting then make a copy
                ApplicationDatabase copyDatase = new();

                if (!copyDatase.CopyDatabaseFile("StartUp"))
                {
                    // Make a copy at the startup of the application
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Het kopiëren van de applicatie database is mislukt.", "Waarschwuwing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    Logging.WriteToLogInformation("De applicatie database is gekopieerd na " + Convert.ToString(counter, CultureInfo.InvariantCulture) + " keer opstarten.");
                }

                // Counter = 0
                this.JsonObjSettings.AppParam[0].CopyAppDataBaseAfterEveryXStartupsCounter = 0;
            }
            else
            {
                this.JsonObjSettings.AppParam[0].CopyAppDataBaseAfterEveryXStartupsCounter = counter;
            }

            Cursor.Current = Cursors.Default;
        }
        #endregion Form load

        #region Load form helpers
        private static void CheckFolders()
        {
            AppEnvironment checkPath = new();
            checkPath.CreateFolder(AppSettingsDefault.ApplicationName, true);
            checkPath.CreateFolder(AppSettingsDefault.ApplicationName + @"\" + AppSettingsDefault.SettingsFolder, true);
            checkPath.CreateFolder(AppSettingsDefault.ApplicationName + @"\" + AppSettingsDefault.DatabaseFolder, true);
            checkPath.CreateFolder(AppSettingsDefault.ApplicationName + @"\" + AppSettingsDefault.DatabaseFolder + @"\" + AppSettingsDefault.BackUpFolder, true);
            checkPath.Dispose();
        }

        private static void CreateSettingsFile()
        {
            // Create a settings file with default values. (if the file does not exists)
            using SettingsManager set = new();
            SettingsManager.CreateSettingsFile();
        }

        private void GetSettings()
        {
            try
            {
                using SettingsManager set = new();
                set.LoadSettings();

                if (set.JsonObjSettings != null)
                {
                    this.JsonObjSettings = set.JsonObjSettings;
                }
                else
                {
                    MessageBox.Show("Het settingsbestand is niet gevonden. De default settings worden ingelezen", "Waarschuwing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (AccessViolationException)
            {
                // Logging is not available here
                MessageBox.Show(
                    "Fout bij het laden van de instellingen. " + Environment.NewLine +
                    "De default instellingen worden ingeladen.",
                    "Waarschuwing",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void StartLogging()
        {
            Logging.NameLogFile = AppSettingsDefault.LogFileName;
            Logging.LogFolder = this.JsonObjSettings.AppParam[0].LogFileLocation;
            Logging.AppendLogFile = this.JsonObjSettings.AppParam[0].AppendLogFile;
            Logging.ActivateLogging = this.JsonObjSettings.AppParam[0].ActivateLogging;

            Logging.ApplicationName = AppSettingsDefault.ApplicationName;
            Logging.ApplicationVersion = AppSettingsDefault.ApplicationVersion;
            Logging.ApplicationBuildDate = AppSettingsDefault.ApplicationBuildDate;
            Logging.Parent = this;

            if (CmDebugMode.DebugMode)
            {
                Logging.DebugMode = true;
            }

            if (!Logging.StartLogging())
            {
                Logging.WriteToFile = false;  // Stop the logging
                Logging.ActivateLogging = false;
                this.JsonObjSettings.AppParam[0].ActivateLogging = false;
                this.JsonObjSettings.AppParam[0].AppendLogFile = false;
            }

            if (CmDebugMode.DebugMode)
            {
                Logging.WriteToLogDebug(string.Empty);
                Logging.WriteToLogDebug("Debug logging staat aan.");
                Logging.WriteToLogDebug(string.Empty);
            }
        }

        private void ApplySettings()
        {
            // When returning from options form
            // string DbLocation = this.JsonObjSettings.AppParam[0].DatabaseLocation;
            // string AppDb = Path.Combine(DbLocation, AppSettingsDefault.SqlLiteDatabaseName);
            this.TextBoxTimeInterval.Text = this.JsonObjSettings.AppParam[0].RateLimit.ToString();
            this.TextBoxWarnPercentage.Text = this.JsonObjSettings.AppParam[0].WarnPercentage.ToString();
            this.WarnPercentage = this.JsonObjSettings.AppParam[0].WarnPercentage;

            bool hidefromTask = this.JsonObjSettings.AppParam[0].HideFromTaskbar;
            this.HideFromTaskbar(hidefromTask);
        }

        private void LoadFormPosition()
        {
            using FormPosition frmPosition = new(this);
            frmPosition.LoadMainFormPosition();
        }
        #endregion Form load

        #region Start a Session
        private void ToolStripMenuItem_Session_Start_Click(object sender, EventArgs e)
        {
            this.startSession = new StartSession();

            if (!string.IsNullOrEmpty(this.TextBoxTimeInterval.Text))
            {
                this.StartSession();
            }
            else
            {
                MessageBox.Show("Tijd interval in minuten moet een waarde hebben.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public enum SessionAction
        {
            Start,  // 0
            Pause,  // 1
            Stop,   // 2
        }

        private void HandleControlsState(string SessionType)
        {
            if (SessionType == SessionAction.Start.ToString())
            {
                this.ButtonStart.Enabled = false;
                this.ButtonStop.Enabled = true;
                this.ButtonPause.Enabled = true;

                this.ToolStripMenuItem_Session_Start.Enabled = false;
                this.ToolStripMenuItem_Session_Stop.Enabled = true;
                this.ToolStripMenuItem_Session_Pause.Enabled = true;

                this.ToolStripButton_SessionStart.Enabled = false;
                this.ToolStripButton_SessionStop.Enabled = true;
                this.ToolStripButton_SessionPause.Enabled = true;

                this.TextBoxTimeInterval.Enabled = false;
                this.ToolStripMenuItem_Options_Configure.Enabled = false;
            }
            else if (SessionType == SessionAction.Pause.ToString())
            {
                this.ButtonStart.Enabled = false;
                this.ButtonStop.Enabled = true;

                this.ToolStripMenuItem_Session_Start.Enabled = true;
                this.ToolStripMenuItem_Session_Stop.Enabled = true;

                this.ToolStripButton_SessionStart.Enabled = true;
                this.ToolStripButton_SessionStop.Enabled = true;

                this.TextBoxTimeInterval.Enabled = true;
            }
            else if (SessionType == SessionAction.Stop.ToString())
            {
                this.ButtonStart.Enabled = true;
                this.ButtonStop.Enabled = false;
                this.ButtonPause.Enabled = false;

                this.ToolStripMenuItem_Session_Start.Enabled = true;
                this.ToolStripMenuItem_Session_Stop.Enabled = false;
                this.ToolStripMenuItem_Session_Pause.Enabled = false;

                this.ToolStripButton_SessionStart.Enabled = true;
                this.ToolStripButton_SessionStop.Enabled = false;
                this.ToolStripButton_SessionPause.Enabled = false;

                this.TextBoxTimeInterval.Enabled = true;
                this.ToolStripMenuItem_Options_Configure.Enabled = true;
            }
        }

        private void StartSession()
        {
            Cursor.Current = Cursors.AppStarting;
            this.Text += "   -->  Bezig...";
            this.ToolStripStatusLabel1.Text = "Bezig..." + "   interval = " + this.TextBoxTimeInterval.Text + " minuten.";

            this.TextBoxTimeInterval.Enabled = false;

            this.HandleControlsState(SessionAction.Start.ToString()); // Set buttons on/off

            this.Refresh();

            if (CM.StartSession.CheckForInternetConnection())
            {
                // First check if there is an active internet connection
                Logging.WriteToLogInformation("Start nieuwe Sessie.");
                this.startSession.ClearTextBoxes(this.Controls);     // Set all textbox.text to 0. (Except the timer and the percentage).
                this.ClearAllDataGridViews();                        // Clear the datagridviews

                this.AllCoinDataSelectedCoins = new CoinDataAll();    // Create the selected coindata objectlist.

                this.CoinMarketPrice = new MarketPrice(this.AllCoinDataSelectedCoins)
                {
                    WarnPercentage = this.WarnPercentage,
                };

                this.ClearAllCharts();                       // Clear the charts

                this.GetMarketPriceData();   // The current ticker data (price)

                this.InitTimer();

                this.ToolStripStatusLabel1.Text = "Bezig..." + "   interval = " + this.TextBoxTimeInterval.Text + " minuten.";
                this.Refresh();
                Cursor.Current = Cursors.Default;
            }
            else
            {
                this.ButtonStop.Enabled = false;
                this.ButtonStart.Enabled = true;

                this.ToolStripStatusLabel1.Text = "Geen internet verbinding.";
                this.Refresh();
                Logging.WriteToLogError("Start nieuwe Sessie is mislukt. er is geen internet verbinding.");
            }
        }
        #endregion Start a Session

        private void GetMarketPriceData()
        {
            this.CoinMarketPrice.WarnPercentage = this.WarnPercentage;
            this.CoinMarketPrice.GetCurrentPriceData();    // Get the current ticker data and fill a dictionary with "Market - Price"

            this.FilldatagridViews();
            this.PlaySound();
            this.SaveAllCoinData();
            this.Charting();
        }

        private void FilldatagridViews()
        {
            this.SetMarketPriceInDataGridView();             // Clear the datagridview "DataGridViewMarketPrice" and add the first record
            this.SetCoinDataInDataGridView();
            this.Set24HourPercentageDifSelectedCoins();
            this.Set24HourPercentageDifNotSelectedCoins();
        }

        private void ClearAllDataGridViews()
        {
            if (this.CoinMarketPrice != null)
            {
                foreach (KeyValuePair<string, string> keyValue in this.CoinMarketPrice.CurrentTickerPrice)
                {
                    string MarketNames = keyValue.Key;

                    foreach (DataGridView DgvName in this.prepareFrm.DgvNames)
                    {
                        if (this.Controls.Find(DgvName.Name, true).FirstOrDefault() is DataGridView cntrl)
                        {
                            cntrl.Rows.Clear();
                        }
                    }
                }
            }

            this.Refresh();

            // Create the blanc rows in the first datagridview
            if (this.AllCoinDataSelectedCoins != null)
            {
                // Is null with the first start after opening the app.
                foreach (CoinData aCoin in this.AllCoinDataSelectedCoins.Items)
                {
                    string CoinName = aCoin.Name;
                    foreach (DataGridView DgvName in this.prepareFrm.DgvNames)
                    {
                        // see PrepareForm, create datagridview. this is the second dgv
                        if (DgvName.Name.Contains(CoinName) && DgvName.Name.Contains("Dgv_1_"))
                        {
                            if (this.Controls.Find(DgvName.Name, true).FirstOrDefault() is DataGridView dgv)
                            {
                                // Dubbele code, moet een functie worden. (dubbel met PrepareForm.CreateDatagridViewPriceData)
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
                            }
                        }
                    }
                }
            }
        }

        private void ClearAllCharts()
        {
            foreach (System.Windows.Forms.DataVisualization.Charting.Chart aChart in this.prepareFrm.ChartNames)
            {
                foreach (var series in aChart.Series)
                {
                    series.Points.Clear();
                }
            }
        }

        private void SetCoinDataInDataGridView()
        {
            if (this.AllCoinDataSelectedCoins != null)
            {
                foreach (CoinData aCoin in this.AllCoinDataSelectedCoins.Items)
                {
                    string CoinName = aCoin.Name;
                    foreach (DataGridView DgvName in this.prepareFrm.DgvNames)
                    {
                        if (DgvName.Name.Contains(CoinName) && DgvName.Name.Contains("Dgv_1_"))
                        {
                            // See PrepareForm, create datagridview. This is the second dgv
                            if (this.Controls.Find(DgvName.Name, true).FirstOrDefault() is DataGridView cntrl)
                            {
                                // Name = "Dgv_1_ADA-EUR"
                                if (cntrl.Rows[0].Cells[1].Value.ToString() == "0")
                                {
                                    // Set Startprice once
                                    cntrl.Rows[0].Cells[1].Value = this.FormatDoubleToString(aCoin.CurrentPrice);
                                }

                                cntrl.Rows[2].Cells[1].Value = this.FormatDoubleToString(aCoin.CurrentPrice);
                                cntrl.Rows[3].Cells[1].Value = this.FormatDoubleToString(aCoin.DiffValuta);
                                cntrl.Rows[4].Cells[1].Value = this.FormatDoubleToString(aCoin.DiffPercent);

                                // Recalculate profit/lost
                                cntrl.Rows[6].Cells[1].Value = MarketPrice.RateWhenProfit(aCoin.SessionStartPrice, aCoin.CurrentPrice, this.WarnPercentage);
                                cntrl.Rows[7].Cells[1].Value = MarketPrice.RateWhenLost(aCoin.SessionStartPrice, aCoin.CurrentPrice, this.WarnPercentage);

                                // Change the text
                                cntrl.Rows[6].Cells[0].Value = string.Format("Koers bij {0}% winst", this.WarnPercentage.ToString()); // New value when profit
                                cntrl.Rows[7].Cells[0].Value = string.Format("Koers bij {0}% verlies", this.WarnPercentage.ToString());   // New value when lost

                                cntrl.Rows[9].Cells[1].Value = this.FormatDoubleToString(aCoin.SessionHighPrice);
                                cntrl.Rows[10].Cells[1].Value = this.FormatDoubleToString(aCoin.SessionLowPrice);

                                cntrl.Rows[12].Cells[1].Value = this.FormatDoubleToString(aCoin.Open24);
                                cntrl.Rows[13].Cells[1].Value = this.FormatDoubleToString(aCoin.High);
                                cntrl.Rows[14].Cells[1].Value = this.FormatDoubleToString(aCoin.Low);
                                cntrl.Rows[15].Cells[1].Value = this.FormatDoubleToString(aCoin.Volume);
                                cntrl.Rows[16].Cells[1].Value = this.FormatDoubleToString(aCoin.VolumeQuote);
                                cntrl.Rows[17].Cells[1].Value = this.FormatDoubleToString(aCoin.Bid);
                                cntrl.Rows[18].Cells[1].Value = this.FormatDoubleToString(aCoin.Ask);
                                cntrl.Rows[19].Cells[1].Value = this.FormatDoubleToString(aCoin.BidSize);
                                cntrl.Rows[20].Cells[1].Value = this.FormatDoubleToString(aCoin.AskSize);

                                // next...

                                // Change color
                                if (aCoin.DiffPercent == 0)
                                {
                                    cntrl.Rows[4].Cells[1].Style.BackColor = Color.Gray;
                                }
                                else if (aCoin.DiffPercent > 0)
                                {
                                    cntrl.Rows[4].Cells[1].Style.BackColor = Color.Green;
                                }
                                else if (aCoin.DiffPercent < 0)
                                {
                                    cntrl.Rows[4].Cells[1].Style.BackColor = Color.Tomato;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                // Used when the precentage is changed before start is pressed
                foreach (DataGridView dgvName in this.prepareFrm.DgvNames)
                {
                    if (dgvName.Name.Contains("Dgv_1_"))
                    {
                        if (this.Controls.Find(dgvName.Name, true).FirstOrDefault() is DataGridView cntrl)
                        {
                            // Change the text
                            cntrl.Rows[5].Cells[0].Value = string.Format("Koers bij {0}% winst", this.WarnPercentage.ToString());   // New value when profit
                            cntrl.Rows[6].Cells[0].Value = string.Format("Koers bij {0}% verlies", this.WarnPercentage.ToString()); // New value when lost
                        }
                    }
                }
            }
        }

        private void SetMarketPriceInDataGridView()
        {
            if (this.AllCoinDataSelectedCoins != null)
            {
                foreach (CoinData aCoin in this.AllCoinDataSelectedCoins.Items)
                {
                    string coinName = aCoin.Name;

                    foreach (DataGridView DgvName in this.prepareFrm.DgvNames)
                    {
                        if (DgvName.Name.Contains(coinName) && DgvName.Name.Contains("Dgv_2_"))
                        {
                            // See PrepareForm, create datagridview. This is the second dgv
                            if (this.Controls.Find(DgvName.Name, true).FirstOrDefault() is DataGridView cntrl)
                            {
                                this.AddRowToDgvPriceMonitor(cntrl, aCoin.CurrentPrice, aCoin.Trend);
                            }
                        }
                    }
                }
            }
        }

        private void Set24HourPercentageDifSelectedCoins()
        {
            if (this.AllCoinDataSelectedCoins != null)
            {
                if (this.Controls.Find("Dgv_DifPerc24hourSelected", true).FirstOrDefault() is DataGridView Dgv)
                {
                    // First clear the datagridview
                    Dgv.Rows.Clear();
                }

                foreach (CoinData aCoin in this.AllCoinDataSelectedCoins.Items)
                {
                    if (this.Controls.Find("Dgv_DifPerc24hourSelected", true).FirstOrDefault() is DataGridView cntrl)
                    {
                        cntrl.SuspendLayout();
                        this.AddRowToDgv24PercDiffSelected(cntrl, aCoin, aCoin.DiffPercentOpen24);

                        double percentage;
                        foreach (DataGridViewRow row in cntrl.Rows)
                        {
                            if (row.Cells["Percentage"].Value != null)
                            {
                                percentage = Convert.ToDouble(row.Cells["Percentage"].Value.ToString());

                                if (percentage == 0)
                                {
                                    cntrl.Rows[row.Index].Cells[1].Style.BackColor = Color.LightGray;
                                }
                                else if (percentage > 0)
                                {
                                    cntrl.Rows[row.Index].Cells[1].Style.BackColor = Color.LightGreen;
                                }
                                else if (percentage < 0)
                                {
                                    cntrl.Rows[row.Index].Cells[1].Style.BackColor = Color.MistyRose;
                                }
                            }
                        }

                        cntrl.Sort(cntrl.Columns["Coin"], ListSortDirection.Ascending);  // Order on coin name
                        cntrl.ResumeLayout();
                    }
                }
            }
        }

        private void Set24HourPercentageDifNotSelectedCoins()
        {
            if (this.AllCoinDataSelectedCoins != null)
            {
                if (this.Controls.Find("Dgv_DifPerc24hourNotSelected", true).FirstOrDefault() is DataGridView Dgv)
                {
                    // First clear the datagridview
                    Dgv.Rows.Clear();
                }

                foreach (CoinData aCoin in this.AllCoinDataSelectedCoins.Items)
                {
                    if (this.Controls.Find("Dgv_DifPerc24hourNotSelected", true).FirstOrDefault() is DataGridView cntrl)
                    {
                        cntrl.SuspendLayout();
                        this.AddRowToDgv24PercDiffNotSelected(cntrl, aCoin, aCoin.DiffPercentOpen24);

                        double Percentage;
                        foreach (DataGridViewRow row in cntrl.Rows)
                        {
                            if (row.Cells["Percentage"].Value != null)
                            {
                                Percentage = Convert.ToDouble(row.Cells["Percentage"].Value.ToString());

                                if (Percentage == 0)
                                {
                                    cntrl.Rows[row.Index].Cells[1].Style.BackColor = Color.LightGray;
                                }
                                else if (Percentage > 0)
                                {
                                    cntrl.Rows[row.Index].Cells[1].Style.BackColor = Color.LightGreen;
                                }
                                else if (Percentage < 0)
                                {
                                    cntrl.Rows[row.Index].Cells[1].Style.BackColor = Color.MistyRose;
                                }
                            }
                        }

                        cntrl.Sort(cntrl.Columns["Coin"], ListSortDirection.Ascending);  // Order on coin name
                        cntrl.ResumeLayout();
                    }
                }
            }
        }

        private void SaveAllCoinData()
        {
            ApplicationDatabase saveCoinData = new();
            saveCoinData.SaveCoinData(this.AllCoinDataSelectedCoins);
            saveCoinData.Dispose();
        }

        #region Charting
        private void PrepareChart()
        {
            foreach (System.Windows.Forms.DataVisualization.Charting.Chart chartName in this.prepareFrm.ChartNames)
            {
                string coinName = chartName.Name.Replace("Chart_", string.Empty);
                if (this.Controls.Find(chartName.Name, true).FirstOrDefault() is System.Windows.Forms.DataVisualization.Charting.Chart cntrl)
                {
                    string serieName = coinName.Replace("_", "-"); // "BTC-EUR";
                    string chartAreaName = "ChartArea_" + serieName;

                    // [0] = {Series-BTC-EUR}
                    cntrl.ChartAreas[chartAreaName].AxisX.IsStartedFromZero = false;
                    cntrl.ChartAreas[chartAreaName].AxisY.IsStartedFromZero = false;

                    cntrl.Series[serieName].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line; // .Spline;
                    cntrl.Series[serieName].Color = Color.DarkGreen;
                    cntrl.Series[serieName].BorderWidth = 3;

                    // [1] = {Series-Start_Prijs_BTC-EUR}
                    cntrl.Series["Start_Prijs_" + serieName].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    cntrl.Series["Start_Prijs_" + serieName].Color = Color.LightGreen;
                    cntrl.Series["Start_Prijs_" + serieName].BorderWidth = 2;

                    // Open_Prijs_       [2] = {Series-Open_Prijs_BTC-EUR}
                    cntrl.Series["Open_Prijs_" + serieName].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    cntrl.Series["Open_Prijs_" + serieName].Color = Color.DarkBlue;
                    cntrl.Series["Open_Prijs_" + serieName].BorderWidth = 2;

                    // Sessie_Hoogste_Prijs      [3] = {Series-Sessie_Hoogste_Prijs_BTC-EUR}
                    cntrl.Series["Sessie_Hoogste_Prijs_" + serieName].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    cntrl.Series["Sessie_Hoogste_Prijs_" + serieName].Color = Color.DeepSkyBlue;
                    cntrl.Series["Sessie_Hoogste_Prijs_" + serieName].BorderWidth = 2;

                    // Sessie_Laagste_Prijs      [4] = {Series-Sessie_Laagste_Prijs_BTC-EUR}
                    cntrl.Series["Sessie_Laagste_Prijs_" + serieName].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    cntrl.Series["Sessie_Laagste_Prijs_" + serieName].Color = Color.Tomato;
                    cntrl.Series["Sessie_Laagste_Prijs_" + serieName].BorderWidth = 2;
                }
            }
        }

        private void Charting()
        {
            foreach (System.Windows.Forms.DataVisualization.Charting.Chart ChartName in this.prepareFrm.ChartNames)
            {
                string CoinName = ChartName.Name.Replace("Chart_", string.Empty);
                if (this.Controls.Find(ChartName.Name, true).FirstOrDefault() is System.Windows.Forms.DataVisualization.Charting.Chart cntrl)
                {
                    if (this.AllCoinDataSelectedCoins != null)
                    {
                        foreach (CoinData aCoin in this.AllCoinDataSelectedCoins.Items)
                        {
                            // Then loop through the coindata to findt the right coin and data
                            if (aCoin.Name == CoinName)
                            {
                                // The current coin price
                                cntrl.Series[CoinName.Replace("_", "-")].Points.AddXY(DateTime.Now.ToString("HH: mm: ss"), aCoin.CurrentPrice);  // [0]

                                if (aCoin.SessionStartPrice != 0)
                                {
                                    cntrl.Series["Start_Prijs_" + CoinName].Points.AddXY(DateTime.Now.ToString("HH: mm: ss"), aCoin.SessionStartPrice);  // [1] = {Series-Start_Prijs_BTC-EUR}
                                }
                                else
                                {
                                    cntrl.Series["Start_Prijs_" + CoinName].Points.AddXY(DateTime.Now.ToString("HH: mm: ss"), aCoin.CurrentPrice);  // First time
                                }

                                if (aCoin.Open24 != 0)
                                {
                                    cntrl.Series["Open_Prijs_" + CoinName].Points.AddXY(DateTime.Now.ToString("HH: mm: ss"), aCoin.Open24);
                                }

                                if (aCoin.SessionHighPrice != 0)
                                {
                                    cntrl.Series["Sessie_Hoogste_Prijs_" + CoinName].Points.AddXY(DateTime.Now.ToString("HH: mm: ss"), aCoin.SessionHighPrice);
                                    SetChartLine(cntrl, "Sessie_Hoogste_Prijs_", aCoin.SessionHighPrice, aCoin.Name);
                                }

                                if (aCoin.SessionLowPrice != 0)
                                {
                                    cntrl.Series["Sessie_Laagste_Prijs_" + CoinName].Points.AddXY(DateTime.Now.ToString("HH: mm: ss"), aCoin.SessionLowPrice);
                                    SetChartLine(cntrl, "Sessie_Laagste_Prijs_", aCoin.SessionLowPrice, aCoin.Name);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void SetChartLine(System.Windows.Forms.DataVisualization.Charting.Chart aChart, string serieName, double aValue, string aCoin)
        {
            if (serieName == "Sessie_Laagste_Prijs_")
            {
                var points = aChart.Series["Sessie_Laagste_Prijs_" + aCoin].Points;
                for (var i = 0; i < points.Count; ++i)
                {
                    points[i].YValues[0] = aValue;
                }
            }

            if (serieName == "Sessie_Hoogste_Prijs_")
            {
                var points = aChart.Series["Sessie_Hoogste_Prijs_" + aCoin].Points;
                for (var i = 0; i < points.Count; ++i)
                {
                    points[i].YValues[0] = aValue;
                }
            }

            // aChart.Invalidate();
        }
        #endregion Charting

        /// <summary>
        /// initialize the timer.
        /// </summary>
        public void InitTimer()
        {
            this.TimerTime = Convert.ToDouble(this.TextBoxTimeInterval.Text);

            this.Timer1 = new System.Windows.Forms.Timer();
            this.Timer1.Tick += new EventHandler(this.Timer1_Tick);
            this.Timer1.Interval = Convert.ToInt32(Math.Floor(this.TimerTime));
            this.Timer1.Start();
        }

        private void PutCoinNamesInComboBox()
        {
            // Toolstrip combobox and  sound combobox
            int counter = 0;
            this.ToolStripComboBoxCoinNames.Items.Clear();
            this.ComboBoxSoundCoin.Items.Clear();

            foreach (TabPage tbp in this.TabControlCharts.TabPages)
            {
                this.ToolStripComboBoxCoinNames.Items.Add(tbp.Name);
                if (tbp.Name != "24 uurs percentage")
                {
                    this.ComboBoxSoundCoin.Items.Add(tbp.Name);
                }

                counter++;
            }

            if (counter > 0)
            {
                this.ToolStripComboBoxCoinNames.Enabled = true;
            }
            else
            {
                this.ToolStripComboBoxCoinNames.Enabled = false;
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            this.GetCoinData();              // Get the current price and add a record in the DataGridViewPriceMonitor
        }

        private void GetCoinData() // Current price to Datagridview
        {
            this.GetMarketPriceData();   // Fill a list with the Currentcoin data
        }

        private void AddRowToDgvPriceMonitor(DataGridView dgv, double currPrice, string trend)
        {
            if (!string.IsNullOrEmpty(trend))
            {
                if (trend == CoinTrend.Equal.ToString())
                {
                    dgv.Rows.Insert(0, new string[] { this.FormatDoubleToString(currPrice), "=", DateTime.Now.ToString() });
                    dgv.Rows[0].Cells[1].Style.BackColor = Color.LightGray;
                }
                else if (trend == CoinTrend.Up.ToString())
                {
                    string upArrow = "\u25B2";  // = Arrow UP
                    dgv.Rows.Insert(0, new string[] { this.FormatDoubleToString(currPrice), upArrow, DateTime.Now.ToString() });

                    dgv.Rows[0].Cells[1].Style.BackColor = Color.LightGreen;
                }
                else if (trend == CoinTrend.Down.ToString())
                {
                    string downArrow = "\u25BC";  // Arrow down
                    dgv.Rows.Insert(0, new string[] { this.FormatDoubleToString(currPrice), downArrow, DateTime.Now.ToString() });
                    dgv.Rows[0].Cells[1].Style.BackColor = Color.MistyRose;
                }
                else
                {
                    dgv.Rows.Insert(0, new string[] { this.FormatDoubleToString(currPrice), "?", DateTime.Now.ToString() });
                    dgv.Rows[0].Cells[1].Style.BackColor = Color.DarkBlue;
                }
            }
        }

        private string FormatDoubleToString(double d)
        {
            try
            {
                string formattedPrice = d.ToString(this.doubleFormatString);

                if (formattedPrice.Length > 0 && formattedPrice != "0")
                {
                    string s = formattedPrice.Substring(0, 1);

                    if (s == "," || s == ".")
                    {
                        formattedPrice = "0" + formattedPrice;
                    }

                    if (formattedPrice.Length >= 2)
                    {
                        s = formattedPrice.Substring(0, 2);
                        if (s == "-," || s == "-.")
                        {
                            formattedPrice = formattedPrice.Replace("-", "-0");
                        }
                    }

                    return formattedPrice;
                }
                else
                {
                    return "0";
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Logging.WriteToLogError("Fout opgetreden in de functie: FormatDoubleToString");
                Logging.WriteToLogError("Waarde die geformateerd moest worden: " + d.ToString());
                Logging.WriteToLogError("Melding:");
                Logging.WriteToLogError(ex.Message);
                if (CmDebugMode.DebugMode)
                {
                    Logging.WriteToLogError(ex.ToString());
                }

                return "0";
            }
        }

        private void AddRowToDgv24PercDiffSelected(DataGridView dgv, CoinData aCoin, double percentage)
        {
            if (aCoin.IsSelected)
            {
                if (!string.IsNullOrEmpty(aCoin.Name))
                {
                    dgv.Rows.Insert(0, new string[] { aCoin.Name, this.FormatDoubleToString(percentage) });
                }
            }
        }

        private void AddRowToDgv24PercDiffNotSelected(DataGridView dgv, CoinData aCoin, double percentage)
        {
            if (!aCoin.IsSelected)
            {
                if (!string.IsNullOrEmpty(aCoin.Name))
                {
                    dgv.Rows.Insert(0, new string[] { aCoin.Name, this.FormatDoubleToString(percentage) });
                }
            }
        }

        #region Keypress only numbers
        private void TextBoxTimeInterval_KeyPress(object sender, KeyPressEventArgs e)
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
        #endregion Keypress only numbers

        private void ToolStripMenuItem_Program_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TextBoxWarnPercentage_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.KeyPresstextBox(sender, e);
        }

        private void TextBoxTimeInterval_Leave(object sender, EventArgs e)
        {
            // The timer interval can not be empty. Empty when leaving the textbox, then set to the default value = 1
            if (this.TextBoxTimeInterval.Text == "0" ||
                this.TextBoxTimeInterval.Text == "," ||
                string.IsNullOrEmpty(this.TextBoxTimeInterval.Text) || string.IsNullOrWhiteSpace(this.TextBoxTimeInterval.Text))
            {
                this.TextBoxTimeInterval.Text = "1";
            }

            this.TimerTime = Convert.ToDouble(this.TextBoxTimeInterval.Text);
        }

        private void TextBoxWarnPercentage_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.TextBoxWarnPercentage.Text) || !string.IsNullOrWhiteSpace(this.TextBoxWarnPercentage.Text))
            {
                this.WarnPercentage = Convert.ToDouble(this.TextBoxWarnPercentage.Text);
                this.CheckBoxSoundPositive.Text = string.Format("Geluid bij een stijging van {0} %", this.WarnPercentage.ToString());
                this.CheckBoxSoundNegative.Text = string.Format("Geluid bij een daling van {0} %", this.WarnPercentage.ToString());
            }
            else
            {
                this.CheckBoxSoundPositive.Text = "Geluid bij een stijging van ... %";
                this.CheckBoxSoundNegative.Text = "Geluid bij een daling van ... %";
            }

            if (this.prepareFrm != null)
            {
                this.prepareFrm.WarnPercentage = this.WarnPercentage;
                this.SetCoinDataInDataGridView();
            }
        }

        #region Stop a session
        private void ToolStripMenuItem_Session_Stop_Click(object sender, EventArgs e)
        {
            this.StopCurrentRun();
        }

        private void StopCurrentRun()
        {
            this.Timer1.Stop();
            this.HandleControlsState(SessionAction.Stop.ToString()); // Set buttons on/off

            // TODO: //MemuIte disable....
            this.FirstRun = true;

            this.Text = this.Text.Replace("   -->  Bezig...", string.Empty);
            Cursor.Current = Cursors.Default;
        }
        #endregion Stop a session

        #region Pause a session
        private void ToolStripMenuItem_Session_Pause_Click(object sender, EventArgs e)
        {
            this.HandleControlsState(SessionAction.Pause.ToString()); // Set buttons on/off
            if (this.ButtonPause.Text == "Pauze")
            {
                this.Text = this.Text.Replace("Bezig", "Pauze");
                this.ToolStripStatusLabel1.Text = "Pauze";
                this.Timer1.Stop();
                this.ButtonPause.Text = "Verder";
                this.ToolStripMenuItem_Session_Pause.Text = "Verder";
                this.ToolStripButton_SessionPause.Text = "Verder";
            }
            else if (this.ButtonPause.Text == "Verder")
            {
                this.Text = this.Text.Replace("Pauze", "Bezig");
                this.ToolStripStatusLabel1.Text = "Bezig..." + "   interval = " + this.TextBoxTimeInterval.Text + " minuten.";
                this.ButtonPause.Text = "Pauze";
                this.ToolStripMenuItem_Session_Pause.Text = "Pauze";
                this.ToolStripButton_SessionPause.Text = "Pauze";
                this.InitTimer();
            }

            Cursor.Current = Cursors.Default;
        }
        #endregion Pause a session

        private void ToolStripMenuItem_Options_Configure_Click(object sender, EventArgs e)
        {
            Logging.WriteToLogInformation("Openen configuratie scherm.");

            SettingsManager.SaveSettings(this.JsonObjSettings);

            FormConfigure _frm = new();

            _frm.ShowDialog();
            _frm.Dispose();

            this.GetSettings();
            this.ApplySettings();
            this.CreateTheTabs();
            this.PrepareChart();
        }

        #region Form closing
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.SaveFormPosition();
            this.SaveSettings();
            Logging.StopLogging();
        }

        private void SaveFormPosition()
        {
            using FormPosition frmPosition = new(this);
            frmPosition.SaveMainFormPosition();
        }

        private void SaveSettings()
        {
            SettingsManager.SaveSettings(this.JsonObjSettings);
        }
        #endregion Form closing

        #region Show date and time in form
        private System.Windows.Forms.Timer t = null;

        private void StartClock()
        {
            this.t = new System.Windows.Forms.Timer
            {
                Interval = 1000,
            };

            this.t.Tick += new EventHandler(this.TimerClockTick);
            this.t.Enabled = true;
        }

        private void TimerClockTick(object sender, EventArgs e)
        {
            this.ToolStripStatusLabel2.Text = DateTime.Now.Date.ToString("d/M/yyyy") + "   -   " + DateTime.Now.ToString("HH:mm:ss");
        }
        #endregion Show date and time in form

        private void ToolStripComboBoxCoinNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Activate the selected tabpage
            Cursor.Current = Cursors.WaitCursor;

            string selected = this.ToolStripComboBoxCoinNames.SelectedItem.ToString();

            if (!string.IsNullOrEmpty(selected))
            {
                this.TabControlMain.SelectedIndex = 1;  // Grafieken

                TabPage t = this.TabControlCharts.TabPages[selected];
                this.TabControlCharts.SelectTab(t); // Go to tab
            }

            Cursor.Current = Cursors.Default;
        }

        #region Export
        private void ToolStripMenuItem_Option_Export_AllUsedCoinTables_Click(object sender, EventArgs e)
        {
            this.StartExport(true);
        }

        private void ToolStripMenuItem_Option_Export_AllCoinTables_Click(object sender, EventArgs e)
        {
            this.StartExport(false);
        }

        private void StartExport(bool currentCoins)
        {
            string fileLocation = this.ChooseFolder();

            this.ToolStripStatusLabel1.Text = "Bezig met exporteren...";
            Cursor.Current = Cursors.WaitCursor;

            SQliteExport exportToCsv = new(fileLocation);
            if (currentCoins)
            {
                exportToCsv.ExportToCsv(true);  // Export only the selected coin tables (in options)
            }
            else
            {
                exportToCsv.ExportToCsv(false);  // Export all coin tables
            }

            // If a session runs then restore the ToolStripStatusLabel1.Text
            if (this.Timer1.Enabled)
            {
                this.ToolStripStatusLabel1.Text = "Bezig..." + "   interval = " + this.TextBoxTimeInterval.Text + " minuten.";
            }
            else
            {
                this.ToolStripStatusLabel1.Text = string.Empty;
            }

            Cursor.Current = Cursors.Default;
        }

        private string ChooseFolder()
        {
            this.ToolStripStatusLabel1.Text = "Selecteer een map.";
            string selectedFolder = string.Empty;
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    selectedFolder = fbd.SelectedPath;
                }
            }

            this.ToolStripStatusLabel1.Text = string.Empty;
            return selectedFolder;
        }

        #endregion Export

        #region add eventhandlers

        /// <summary>
        /// Get all the controls.
        /// </summary>
        /// <param name="control">The founded control,</param>
        /// <param name="type">Controle type.</param>
        /// <returns>All controles founded.</returns>
        public IEnumerable<Control> GetAll(Control control, Type type = null)
        {
            var controls = control.Controls.Cast<Control>();

            // Check the all value, if true then get all the controls
            // Otherwise get the controls of the specified type
            if (type == null)
            {
                return controls.SelectMany(ctrl => this.GetAll(ctrl, type)).Concat(controls);
            }
            else
            {
                return controls.SelectMany(ctrl => this.GetAll(ctrl, type)).Concat(controls).Where(c => c.GetType() == type);
            }
        }

        private void AddEventHandlerToPriceCheckBox()
        {
            foreach (CheckBox cb in this.prepareFrm.CheckBoxNames)
            {
                switch (cb.Text)
                {
                    case "Start prijs aan/uit":
                        cb.Click += new EventHandler(this.CheckBoxStartPrice_Click);
                        break;
                    case "Open prijs aan/uit":
                        cb.Click += new EventHandler(this.CheckBoxOpenPrice_Click);
                        break;
                    case "Sessie hoogste aan/uit":
                        cb.Click += new EventHandler(this.CheckBoxSessionHighPrice_Click);
                        break;
                    case "Sessie laagste aan/uit":
                        cb.Click += new EventHandler(this.CheckBoxSessionLowPrice_Click);
                        break;
                    default:
                        // code block
                        break;
                }
            }
        }

        private void CheckBoxStartPrice_Click(object sender, EventArgs e)
        {
            this.AddCheckBoxClickEvent(sender, e);   // Start price On/Off
        }

        private void CheckBoxOpenPrice_Click(object sender, EventArgs e)
        {
            this.AddCheckBoxClickEvent(sender, e);   // Open price On/Off
        }

        private void CheckBoxSessionHighPrice_Click(object sender, EventArgs e)
        {
            this.AddCheckBoxClickEvent(sender, e);   // Session High price On/Off
        }

        private void CheckBoxSessionLowPrice_Click(object sender, EventArgs e)
        {
            this.AddCheckBoxClickEvent(sender, e);   // Session low price On/Off
        }

        private void AddCheckBoxClickEvent(object sender, EventArgs e)
        {
            string seriesStartPriceName = string.Empty;
            CheckBox Cb = sender as CheckBox;
            switch (Cb.Text)
            {
                case "Start prijs aan/uit":
                    seriesStartPriceName = "Start_Prijs_" + this.ActiveCoinName;
                    break;
                case "Open prijs aan/uit":
                    seriesStartPriceName = "Open_Prijs_" + this.ActiveCoinName;
                    break;
                case "Sessie hoogste aan/uit":
                    seriesStartPriceName = "Sessie_Hoogste_Prijs_" + this.ActiveCoinName;
                    break;
                case "Sessie laagste aan/uit":
                    seriesStartPriceName = "Sessie_Laagste_Prijs_" + this.ActiveCoinName;
                    break;
                default:
                    // code block
                    break;
            }

            var c1 = this.GetAll(this, typeof(System.Windows.Forms.DataVisualization.Charting.Chart));
            foreach (System.Windows.Forms.DataVisualization.Charting.Chart aChart in c1)
            {
                if (aChart.Name == "Chart_" + this.ActiveCoinName)
                {
                    if (Cb.Checked)
                    {
                        aChart.Series[seriesStartPriceName].Enabled = true;
                    }
                    else
                    {
                        aChart.Series[seriesStartPriceName].Enabled = false;
                    }
                }
            }
        }
        #endregion add eventhandlers

        #region Show the coin name in the toolbox combobox
        private void TabControlCharts_SelectedIndexChanged(object sender, EventArgs e)
        {
            // TabControlCharts.SelectedTab = null when the coin tabs are open en the option forms is closed
            if (this.TabControlCharts.SelectedTab != null)
            {
                this.ToolStripComboBoxCoinNames.Text = this.TabControlCharts.SelectedTab.Name;
                this.ActiveCoinName = this.TabControlCharts.SelectedTab.Name;
                this.ActivetabpageIndex = this.TabControlCharts.SelectedIndex;
            }
            else
            {
                foreach (TabPage tp in this.TabControlCharts.TabPages)
                {
                    if (tp.Name == this.ActiveCoinName)
                    {
                        this.ToolStripComboBoxCoinNames.Text = this.ActiveCoinName;
                        this.TabControlCharts.SelectedIndex = this.ActivetabpageIndex;
                    }
                }
            }
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            this.ToolStripComboBoxCoinNames.Text = this.TabControlCharts.SelectedTab.Name;
        }

        #endregion Show the coin name in the toobox combobox

        #region Hide from taskbar
        private void ToolStripMenuItemHideFromTaskbar_Click(object sender, EventArgs e)
        {
            if (this.ToolStripMenuItemHideFromTaskbar.Checked)
            {
                this.HideFromTaskbar(false);
            }
            else
            {
                this.HideFromTaskbar(true);
            }
        }

        private void HideFromTaskbar(bool Hide)
        {
            if (Hide)
            {
                this.ShowInTaskbar = false;
                this.ToolStripMenuItemHideFromTaskbar.Checked = true;
                this.JsonObjSettings.AppParam[0].HideFromTaskbar = true;
            }
            else
            {
                this.ShowInTaskbar = true;
                this.ToolStripMenuItemHideFromTaskbar.Checked = false;
                this.JsonObjSettings.AppParam[0].HideFromTaskbar = false;
            }
        }
        #endregion Hide from taskbar

        #region show tooltip when mouse hoovers over a chart point
        private Point? prevPosition = null;
        private ToolTip tooltip = new();

        private void ChartShowHints(object sender, MouseEventArgs e)
        {
            System.Windows.Forms.DataVisualization.Charting.Chart aChart = sender as System.Windows.Forms.DataVisualization.Charting.Chart;

            var pos = e.Location;
            if (this.prevPosition.HasValue && pos == this.prevPosition.Value)
            {
                return;
            }

            this.tooltip.RemoveAll();
            this.prevPosition = pos;

            var results = aChart.HitTest(pos.X, pos.Y, false, System.Windows.Forms.DataVisualization.Charting.ChartElementType.DataPoint);
            foreach (var result in results)
            {
                if (result.ChartElementType == System.Windows.Forms.DataVisualization.Charting.ChartElementType.DataPoint)
                {
                    var yVal = result.ChartArea.AxisY.PixelPositionToValue(pos.Y);
                    var prop = result.Object as System.Windows.Forms.DataVisualization.Charting.DataPoint;

                    double value = Math.Round(yVal, 2); // Coin value
                    var yValue = prop.AxisLabel;        // Time

                    this.tooltip.Show("€ " + value.ToString() + " - " + yValue.ToString(), aChart, pos.X, pos.Y - 15);
                }
            }
        }

        private void AddEventHandlerToChart()
        {
            var c1 = this.GetAll(this, typeof(System.Windows.Forms.DataVisualization.Charting.Chart));

            foreach (System.Windows.Forms.DataVisualization.Charting.Chart aChart in this.prepareFrm.ChartNames)
            {
                aChart.MouseMove += new MouseEventHandler(this.Chart_MouseMove);     // Used for showing label with coin value
                aChart.MouseWheel += new MouseEventHandler(this.Chart_MouseWheel);   // Used for zoomable chart
            }
        }

        private void Chart_MouseMove(object sender, MouseEventArgs e)
        {
            this.ChartShowHints(sender, e);
            this.MousCrossHairs(sender, e);
        }

        private void MousCrossHairs(object sender, MouseEventArgs e)
        {
            System.Windows.Forms.DataVisualization.Charting.Chart aChart = (System.Windows.Forms.DataVisualization.Charting.Chart)sender;

            int x = aChart.ClientSize.Width;
            int y = aChart.ClientSize.Height;

            Label labelYaxisCur = new();

            foreach (Label aLabel in this.prepareFrm.LabelNames)
            {
                if (aLabel.Name == aChart.Name.Replace("Chart_", "Lb_"))
                {
                    labelYaxisCur = aLabel;
                }
            }

            // Make the label visible if the cursur is above the chart.
            //  left                          || bottom        ||top        ||right
            if (e.X <= aChart.Location.X + 75 || e.Y >= y - 85 || e.Y <= 35 || e.X >= aChart.Location.X + x - 50)
            {
                labelYaxisCur.Visible = false;
            }
            else
            {
                string ChartAreaName = aChart.Name.Replace("Chart_", "ChartArea_");

                double yValue = aChart.ChartAreas[ChartAreaName].AxisY.PixelPositionToValue(e.Y);

                if (HasValue(yValue))
                {
                    labelYaxisCur.Visible = true;

                    if (yValue > 0 && yValue < 50)
                    {
                        labelYaxisCur.Text = "€" + Math.Round(yValue, 4).ToString();
                    }
                    else if (yValue >= 50 && yValue < 100)
                    {
                        labelYaxisCur.Text = "€" + Math.Round(yValue, 2).ToString();
                    }
                    else if (yValue >= 100 && yValue < 1000)
                    {
                        labelYaxisCur.Text = "€" + Math.Round(yValue, 1).ToString();
                    }
                    else if (yValue >= 1000)
                    {
                        labelYaxisCur.Text = "€" + Math.Round(yValue, 0).ToString();
                    }

                    labelYaxisCur.Location = new Point(aChart.Right - 80, e.Y - 5);
                }
                else
                {
                    labelYaxisCur.Visible = false;
                }
            }
        }

        public static bool HasValue(double value)
        {
            return !Double.IsNaN(value) && !Double.IsInfinity(value);
        }

        #endregion show tooltip when mouse hoovers over a chart point

        #region play sound
        private void ButtonTestSoundPositive_Click(object sender, EventArgs e)
        {
            PlayWav(true);
        }

        private void ButtonTestSoundNegative_Click(object sender, EventArgs e)
        {
            PlayWav(false);
        }

        private static void PlayWav(bool PlayPositiveSound)
        {
            try
            {
                if (PlayPositiveSound)
                {
                    System.Media.SoundPlayer player = new(Path.GetDirectoryName(Application.ExecutablePath) + @"\Sound" + "\\CASHREG.WAV");
                    player.Play();
                }
                else
                {
                    System.Media.SoundPlayer player = new(Path.GetDirectoryName(Application.ExecutablePath) + @"\Sound" + "\\Ambulance.WAV");
                    player.Play();
                }
            }
            catch (FileNotFoundException ex)
            {
                Logging.WriteToLogError("Het 'WAV' bestand is niet gevonden.");
                Logging.WriteToLogError("Melding");
                Logging.WriteToLogError(ex.Message);
            }
        }

        private void PlaySound()
        {
            if (!string.IsNullOrEmpty(this.ComboBoxSoundCoin.Text))
            {
                if (this.WarnPercentage > 0)
                {
                    if (this.AllCoinDataSelectedCoins != null)
                    {
                        string SoundCoin = this.ComboBoxSoundCoin.Text;

                        foreach (CoinData aCoin in this.AllCoinDataSelectedCoins.Items)
                        {
                            double PercentageDif = aCoin.DiffPercent;

                            if (PercentageDif >= this.WarnPercentage && aCoin.Name == SoundCoin)
                            {
                                if (this.SoundUp)
                                {
                                    if (!this.SoundUpIsPlayed)
                                    {
                                        PlayWav(true);

                                        this.SoundUpIsPlayed = true;
                                        this.SoundDownIsPlayed = false;

                                        Logging.WriteToLogInformation("positief: " + aCoin.Name);
                                    }
                                }
                            }

                            if (PercentageDif <= 0 - this.WarnPercentage && aCoin.Name == SoundCoin)
                            {
                                if (this.SoundDown)
                                {
                                    if (!this.SoundDownIsPlayed)
                                    {
                                        PlayWav(false);

                                        this.SoundUpIsPlayed = false;
                                        this.SoundDownIsPlayed = true;

                                        Logging.WriteToLogInformation("Negatief: " + aCoin.Name);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void CheckBoxSoundPositive_CheckedChanged(object sender, EventArgs e)
        {
            if (this.CheckBoxSoundPositive.Checked)
            {
                this.SoundUp = true;
                this.ComboBoxSoundCoin.Enabled = true;
            }
            else
            {
                this.SoundUp = false;
                if (!this.CheckBoxSoundNegative.Checked)
                {
                    this.ComboBoxSoundCoin.Enabled = false;
                    this.ComboBoxSoundCoin.Text = string.Empty;
                }
            }

            this.SoundDownIsPlayed = false;
            this.SoundUpIsPlayed = false;
        }

        private void CheckBoxSoundNegative_CheckedChanged(object sender, EventArgs e)
        {
            if (this.CheckBoxSoundNegative.Checked)
            {
                this.SoundDown = true;
                this.ComboBoxSoundCoin.Enabled = true;
            }
            else
            {
                this.SoundDown = false;
                if (!this.CheckBoxSoundPositive.Checked)
                {
                    this.ComboBoxSoundCoin.Enabled = false;
                    this.ComboBoxSoundCoin.Text = string.Empty;
                }
            }

            this.SoundDownIsPlayed = false;
            this.SoundUpIsPlayed = false;
        }
        #endregion Play sound

        #region Zoomable chart

        private void Chart_MouseWheel(object sender, MouseEventArgs e)
        {
            var chart = (System.Windows.Forms.DataVisualization.Charting.Chart)sender;
            var xAxis = chart.ChartAreas[0].AxisX;
            var yAxis = chart.ChartAreas[0].AxisY;

            try
            {
                if (e.Delta < 0)
                {
                    // Scrolled down.
                    xAxis.ScaleView.ZoomReset();
                    yAxis.ScaleView.ZoomReset();
                }
                else if (e.Delta > 0)
                {
                    // Scrolled up.
                    var xMin = xAxis.ScaleView.ViewMinimum;
                    var xMax = xAxis.ScaleView.ViewMaximum;
                    var yMin = yAxis.ScaleView.ViewMinimum;
                    var yMax = yAxis.ScaleView.ViewMaximum;

                    var posXStart = xAxis.PixelPositionToValue(e.Location.X) - ((xMax - xMin) / 4);
                    var posXFinish = xAxis.PixelPositionToValue(e.Location.X) + ((xMax - xMin) / 4);
                    var posYStart = yAxis.PixelPositionToValue(e.Location.Y) - ((yMax - yMin) / 4);
                    var posYFinish = yAxis.PixelPositionToValue(e.Location.Y) + ((yMax - yMin) / 4);

                    xAxis.ScaleView.Zoom(posXStart, posXFinish);
                    yAxis.ScaleView.Zoom(posYStart, posYFinish);
                }
            }
            catch { }
        }
        #endregion Zoomable chart
    }
}
