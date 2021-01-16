using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using HvBAppEnvironment;
using HvBLogging;

namespace CM
{
    public partial class FormMain : Form
    {
        public dynamic JsonObjSettings { get; set; }        // Holds the user and application setttings
        StartSession Start;
        PrepareForm Prepare;
        public CoinDataAll AllCoinData;

        private MarketPrice CoinMarketPrice;

        private readonly string DoubleFormatString = "#.############";  //TODO make optional

        double timmerTime;
        private double TimerTime
        {
            get
            {
                return timmerTime;
            }
            set
            {
                this.timmerTime = Convert.ToDouble(value * 60 * 1000);    //millisec = minuten * 60 * 1000;
            }
        }

        private bool FirstRun { get; set; }
        private string DecimalSeperator { get; set; }
        private double WarnPercentage { get; set; }


    private bool DebugMode { get; set; }

        public FormMain()
        {
            InitializeComponent();

            FirstRun = true;
            CheckFolders();         // Create in appdata a new folder Settings and/or Database if needed
            CreateSettingsFile();   // Create the settings file if needed
            GetSettings();          // Get the settings a user saved
            StartLogging();         // Start the logging
            ApplySettings();
            CheckAppDatabase();     // Check if the databse file exists and/or it is up to date with the last version.
            BackColor = SystemColors.Window;
            Text = AppSettingsDefault.ApplicationName;
            DoubleBuffered = true;
            StartClock();           // Show the date and time in the statusstrip            
        }

        #region Form load
        private void Form1_Load(object sender, EventArgs e)
        {            
            Initialize();
        }

        private void Initialize()
        {
            DecimalSeperator = CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator;

            CreateTheTabs();            
            PrepareChart();            
            LoadFormPosition();     //load the last saved form position

            string DbLocation = JsonObjSettings.AppParam[0].DatabaseLocation;
            string AppDb = Path.Combine(DbLocation, AppSettingsDefault.SqlLiteDatabaseName);           

            if (!File.Exists(AppDb)) // Check to see if the application database exists.
            {                
                MessageBox.Show("De applicatie database is niet gevonden. Controleer het log bestand." + Environment.NewLine +
                               Environment.NewLine +
                               "Het programma wordt afgesloten.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                Application.Exit(); // Ends program major error.
            }
            else
            {
                CopyAppDatabase();      //Every xx times the application starts it makes a copy of the application database
            }
        }
        private void CreateTheTabs()
        {
            WarnPercentage = Convert.ToDouble(TextBoxWarnPercentage.Text);

            Prepare = new PrepareForm();
            Prepare.WarnPercentage = WarnPercentage;

            Prepare.CreateTheTabs(TabControlCharts);

            HandleControlsState(SessionAction.Stop.ToString()); //Set buttons on/off
            TabControlMain.SelectedIndex = 0;
            PutCoinNamesInComboBox();
        }
        
        private void CheckAppDatabase()
        {
            string dbLocation = JsonObjSettings.AppParam[0].DatabaseLocation;
            if (!File.Exists(Path.Combine(dbLocation, AppSettingsDefault.SqlLiteDatabaseName)))
            {
                Logging.WriteToLogWarning("De applicatie database ontbreekt en wordt aangemaakt.");
                CreateAppDatabase();    // if the app databse doesn't exist then create it. Only when the application starts with parameter Install
                CheckAppDbaseVersion(); // check the version of the application database (and update it if needed)                
            }
            else
            {
                Logging.WriteToLogInformation("De applicatie database is aanwezig.");

                CheckAppDbaseVersion(); //check the version of the application database (and update it if needed)
            }
            GetSQliteVersion();     // set de SQlite version in the log file
        }
        private void CheckAppDbaseVersion()
        {
            Cursor.Current = Cursors.WaitCursor;

            string DbLocation = JsonObjSettings.AppParam[0].DatabaseLocation;
            string DBFilePath = Path.Combine(DbLocation, AppSettingsDefault.SqlLiteDatabaseName);

            if (File.Exists(DBFilePath))
            {
                Logging.WriteToLogInformation("De applicatie database is aanwezig op locatie: " + DBFilePath);
                using ApplicationDatabase AppDb = new()
                {
                    DebugMode = DebugMode
                };

                if (AppDb.SelectMeta() < AppSettingsDefault.UpdateVersion)
                {
                    using ProcessArguments getArg = new();
                    string argument = string.Empty;
                    foreach (string arg in getArg.cmdLineArg)
                    {
                        argument = Convert.ToString(arg, CultureInfo.InvariantCulture);
                        if (argument == getArg.ArgIntall)
                        {
                            if (AppDb.UpdateDatabase())
                            {
                                //LockProgram = false;
                            }
                            else
                            {
                                //LockProgram = true;
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
            using ProcessArguments getArg = new();
            string argument = string.Empty;
            string DbLocation = JsonObjSettings.AppParam[0].DatabaseLocation;
            using ApplicationDatabase AppDb = new()
            {
                DebugMode = DebugMode
            };

            foreach (string arg in getArg.cmdLineArg)
            {
                argument = Convert.ToString(arg, CultureInfo.InvariantCulture);
                if (argument == getArg.ArgIntall)
                {
                    if (!File.Exists(Path.Combine(DbLocation, AppSettingsDefault.SqlLiteDatabaseName)))
                    {
                        if (AppDb.CreateNewDatabase())
                        {
                            MessageBox.Show("De Applicatie database is aangemaakt.", "Informatie.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //LockProgram = false;
                        }
                        else
                        {
                            //LockProgram = true;
                        }
                    }
                }
                else
                {
                    if (DebugMode)
                    {
                        Logging.WriteToLogInformation("Geen passende argumenten gevonden.");
                    }
                }
            }
            Cursor.Current = Cursors.Default;
        }
        private void GetSQliteVersion()
        {
            using ApplicationDatabase AppDb = new()
            {
                DebugMode = DebugMode
            };
            Logging.WriteToLogInformation("SQLite versie: " + AppDb.GetSQliteVersion());
        }

        private void CopyAppDatabase()
        {   
            Cursor.Current = Cursors.WaitCursor;
            int counter = JsonObjSettings.AppParam[0].CopyAppDataBaseAfterEveryXStartupsCounter;
            counter++;  //add a new start to the counter          

            int CopyAppDataBaseAfterEveryXStartups = JsonObjSettings.AppParam[0].CopyAppDataBaseAfterEveryXStartups;
            if (this.DebugMode)
            {
                Logging.WriteToLogDebug("De teller voor het aanmaken van een kopie van de applicatie database staat op : " + Convert.ToString(counter, CultureInfo.InvariantCulture));
                Logging.WriteToLogDebug("De database wordt gekopieerd bij elke xx keer opstarten : " + Convert.ToString(CopyAppDataBaseAfterEveryXStartups, CultureInfo.InvariantCulture));
            }

            if (counter >= CopyAppDataBaseAfterEveryXStartups)  //if counter equals the option setting then make a copy
            {
                ApplicationDatabase CopyDatase = new()
                {
                    DebugMode = this.DebugMode
                };

                if (!CopyDatase.CopyDatabaseFile("StartUp"))  //make a copy at the startup of the application
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Het kopiëren van de applicatie database is mislukt.", "Waarschwuwing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    Logging.WriteToLogInformation("De applicatie database is gekopieerd na " + Convert.ToString(counter, CultureInfo.InvariantCulture) + " keer opstarten.");
                }
                //zet de teller naar 0
                JsonObjSettings.AppParam[0].CopyAppDataBaseAfterEveryXStartupsCounter = 0;
            }
            else
            {
                JsonObjSettings.AppParam[0].CopyAppDataBaseAfterEveryXStartupsCounter = counter;
            }
            Cursor.Current = Cursors.Default;
        }
        #endregion Form load

        #region Load form helpers
        private static void CheckFolders()
        {
            AppEnvironment CheckPath = new();
            CheckPath.CreateFolder(AppSettingsDefault.ApplicationName, true);
            CheckPath.CreateFolder(AppSettingsDefault.ApplicationName + @"\" + AppSettingsDefault.SettingsFolder, true);
            CheckPath.CreateFolder(AppSettingsDefault.ApplicationName + @"\" + AppSettingsDefault.DatabaseFolder, true);
            CheckPath.CreateFolder(AppSettingsDefault.ApplicationName + @"\" + AppSettingsDefault.DatabaseFolder + @"\" + AppSettingsDefault.BackUpFolder, true);
            CheckPath.Dispose();
        }
        private static void CreateSettingsFile()
        {
            //Create a settings file with default values. (if the file does not exists)
            using SettingsManager Set = new();
            SettingsManager.CreateSettingsFile();
        }
        private void GetSettings()
        {
            try
            {
                using SettingsManager Set = new();
                Set.LoadSettings();

                if (Set.JsonObjSettings != null)
                {
                    JsonObjSettings = Set.JsonObjSettings;
                }
                else
                {
                    MessageBox.Show("Het settingsbestand is niet gevonden. De default settings worden ingelezen", "Waarschuwing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (AccessViolationException)
            {
                //Logging is not available here
                MessageBox.Show("Fout bij het laden van de instellingen. " + Environment.NewLine +
                                "De default instellingen worden ingeladen.", "Waarschuwing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void StartLogging()
        {
            Logging.NameLogFile = AppSettingsDefault.LogFileName;
            Logging.LogFolder = JsonObjSettings.AppParam[0].LogFileLocation;
            Logging.AppendLogFile = JsonObjSettings.AppParam[0].AppendLogFile;
            Logging.ActivateLogging = JsonObjSettings.AppParam[0].ActivateLogging;

            Logging.ApplicationName = AppSettingsDefault.ApplicationName;
            Logging.ApplicationVersion = AppSettingsDefault.ApplicationVersion;
            Logging.ApplicationBuildDate = AppSettingsDefault.ApplicationBuildDate;
            Logging.Parent = this;

            if (this.DebugMode)
            {
                Logging.DebugMode = true;
            }

            if (!Logging.StartLogging())
            {
                Logging.WriteToFile = false;  //Stop the logging
                Logging.ActivateLogging = false;
                JsonObjSettings.AppParam[0].ActivateLogging = false;
                JsonObjSettings.AppParam[0].AppendLogFile = false;
            }
            if (this.DebugMode)
            {
                Logging.WriteToLogDebug("");
                Logging.WriteToLogDebug("Debug logging staat aan.");
                Logging.WriteToLogDebug("");
            }
        }
        private void ApplySettings()
        {
            //when returning from options form
            string DbLocation = JsonObjSettings.AppParam[0].DatabaseLocation;
            string AppDb = Path.Combine(DbLocation, AppSettingsDefault.SqlLiteDatabaseName);

            //always 
            TextBoxTimeInterval.Text = JsonObjSettings.AppParam[0].RateLimit.ToString();
            TextBoxWarnPercentage.Text = JsonObjSettings.AppParam[0].WarnPercentage.ToString();
        }
        private void LoadFormPosition()
        {
            using FormPosition FormPosition = new(this);
            FormPosition.LoadMainFormPosition();
        }
        #endregion Form load

        #region Start a Session
        private void ToolStripMenuItem_Session_Start_Click(object sender, EventArgs e)
        {
            Start = new StartSession();

            if (!string.IsNullOrEmpty(TextBoxTimeInterval.Text))
            {
                StartSession();
            }
            else
            {
                MessageBox.Show("Tijd interval in minuten moet een waarde hebben.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public enum SessionAction
        {
            Start,     // 0
            Pause,    // 1
            Stop  // 2            
        }

        private void HandleControlsState(string SessionType)
        {
            if (SessionType == SessionAction.Start.ToString())
            {
                ButtonStart.Enabled = false;
                ButtonStop.Enabled = true;                
                ButtonPause.Enabled = true;

                ToolStripMenuItem_Session_Start.Enabled = false;
                ToolStripMenuItem_Session_Stop.Enabled = true;
                ToolStripMenuItem_Session_Pause.Enabled = true;

                ToolStripButton_SessionStart.Enabled = false;
                ToolStripButton_SessionStop.Enabled = true;
                ToolStripButton_SessionPause.Enabled = true;

                TextBoxTimeInterval.Enabled = false;
                ToolStripMenuItem_Options_Configure.Enabled = false;
            }
            else if (SessionType == SessionAction.Pause.ToString())
            {
                ButtonStart.Enabled = false;
                ButtonStop.Enabled = true;

                ToolStripMenuItem_Session_Start.Enabled = true;
                ToolStripMenuItem_Session_Stop.Enabled = true;

                ToolStripButton_SessionStart.Enabled = true;
                ToolStripButton_SessionStop.Enabled = true;

                TextBoxTimeInterval.Enabled = true;
            }
            else if (SessionType == SessionAction.Stop.ToString())
            {
                ButtonStart.Enabled = true;
                ButtonStop.Enabled = false;
                ButtonPause.Enabled = false;

                ToolStripMenuItem_Session_Start.Enabled = true;
                ToolStripMenuItem_Session_Stop.Enabled = false;
                ToolStripMenuItem_Session_Pause.Enabled = false;

                ToolStripButton_SessionStart.Enabled = true;
                ToolStripButton_SessionStop.Enabled = false;
                ToolStripButton_SessionPause.Enabled = false;

                TextBoxTimeInterval.Enabled = true;
                ToolStripMenuItem_Options_Configure.Enabled = true;
            }
        }

        private void StartSession()
        {
            Cursor.Current = Cursors.AppStarting;
            this.Text += "   -->  Bezig...";
            ToolStripStatusLabel1.Text = "Bezig...";

            TextBoxTimeInterval.Enabled = false;

            HandleControlsState(SessionAction.Start.ToString()); //Set buttons on/off

            Refresh();

            if (CM.StartSession.CheckForInternetConnection())  // First check if there is an active internet connection
            {
                Logging.WriteToLogInformation("Start nieuwe Sessie.");
                Start.ClearTextBoxes(this.Controls);    //Set all textbox.text to 0 (except the timer and the percentage).       
                ClearAllDataGridViews();                // clear the datagridviews                                                            

                AllCoinData = new CoinDataAll();    //create the coindata objectlist

                CoinMarketPrice = new MarketPrice(AllCoinData);
                CoinMarketPrice.WarnPercentage = WarnPercentage;

                GetMarketPriceData();   // The current ticker data (price)

                InitTimer();            // 
                //...

                ToolStripStatusLabel1.Text = "Bezig...";
                Refresh();
                Cursor.Current = Cursors.Default;
            }
            else
            {
                ButtonStop.Enabled = false;
                ButtonStart.Enabled = true;

                ToolStripStatusLabel1.Text = "Geen internet verbinding.";
                Refresh();
                Logging.WriteToLogError("Start nieuwe Sessie is mislukt. er is geen internet verbinding.");
            }
        }
        #endregion Start a Session


        private void GetMarketPriceData()
        {
            CoinMarketPrice.WarnPercentage = WarnPercentage;
            CoinMarketPrice.GetCurrentPriceData();    // Get the current ticker data and fill a dictionary with "Market - Price"
                                                      // 
            FilldatagridViews();
            SaveAllCoinData();
            Charting();
        }

        private void FilldatagridViews()
        {
            SetMarketPriceInDataGridView();             // Clear the datagridview "DataGridViewMarketPrice" and add the first record
            SetCoinDataInDataGridView();
        }
        private void ClearAllDataGridViews()
        {
            if (CoinMarketPrice != null)
            {
                foreach (KeyValuePair<string, string> keyValue in CoinMarketPrice.CurrentTickerPrice)
                {
                    string MarketNames = keyValue.Key;

                    foreach (string DgvName in Prepare.DgvNames)    //loop through the datagridvieuws
                    {
                        if (DgvName.Contains(MarketNames))
                        {
                            if (Controls.Find(DgvName, true).FirstOrDefault() is DataGridView cntrl)
                            {
                                cntrl.Rows.Clear();
                            }
                        }
                    }
                }
            }
            Refresh();
            //create the blanc rows in the first datagridview
            if (AllCoinData != null)  //is null with the first start after opening the app.
            {
                foreach (CoinData aCoin in AllCoinData.Items)
                {
                    string CoinName = aCoin.Name;
                    foreach (string DgvName in Prepare.DgvNames)    //loop through the datagridvieuws
                    {
                        if (DgvName.Contains(CoinName) && DgvName.Contains("Dgv_1_"))  //see PrepareForm, create datagridview. this is the second dgv
                        {
                            if (Controls.Find(DgvName, true).FirstOrDefault() is DataGridView dgv)
                            {
                                //Dubbele code, moet een functie worden. (dubbel met PrepareForm.CreateDatagridViewPriceData)
                                dgv.Rows.Add("Start prijs", "0");               //0
                                dgv.Rows.Add("", "");                           //1
                                dgv.Rows.Add("Huidige prijs", "0");             //2                                
                                dgv.Rows.Add("Verschil [€]", "0");              //3
                                dgv.Rows.Add("Verschil [%]", "0");              //4
                                dgv.Rows.Add("", "");                           //5
                                dgv.Rows.Add(string.Format("Koers bij {0}% winst",WarnPercentage.ToString()) , "0");           //6
                                dgv.Rows.Add(string.Format("Koers bij {0}% verlies", WarnPercentage.ToString()), "0");         //7                                
                                dgv.Rows.Add("", "");                           //8

                                dgv.Rows.Add("Hoogste (sessie)", "0");          //9
                                dgv.Rows.Add("Laagste (sessie)", "0");          //10
                                dgv.Rows.Add("", "");                           //11

                                dgv.Rows.Add("Open (24 uur geleden)", "0");     //12
                                dgv.Rows.Add("Hoogste", "0");                   //13
                                dgv.Rows.Add("Laagste", "0");                   //14
                                dgv.Rows.Add("Volume", "0");                    //15
                                dgv.Rows.Add("Volume quote", "0");              //16
                                dgv.Rows.Add("Bieden", "0");                    //17
                                dgv.Rows.Add("Vraag", "0");                     //18
                                dgv.Rows.Add("Bied grootte", "0");              //19
                                dgv.Rows.Add("Vraag grootte", "0");             //20

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

        private void SetCoinDataInDataGridView()
        {
            if (AllCoinData != null)
            {
                foreach (CoinData aCoin in AllCoinData.Items)
                {
                    string CoinName = aCoin.Name;
                    foreach (string DgvName in Prepare.DgvNames)    //loop through the datagridvieuws
                    {
                        if (DgvName.Contains(CoinName) && DgvName.Contains("Dgv_1_"))  //see PrepareForm, create datagridview. this is the second dgv
                        {
                            if (Controls.Find(DgvName, true).FirstOrDefault() is DataGridView cntrl)
                            {
                                if (cntrl.Rows[0].Cells[1].Value.ToString() == "0")//Set Startprice once
                                {
                                    cntrl.Rows[0].Cells[1].Value = FormatDoubleToString(aCoin.CurrentPrice);
                                }

                                cntrl.Rows[2].Cells[1].Value = FormatDoubleToString(aCoin.CurrentPrice);
                                cntrl.Rows[3].Cells[1].Value = FormatDoubleToString(aCoin.DiffValuta);
                                cntrl.Rows[4].Cells[1].Value = FormatDoubleToString(aCoin.DiffPercent);  

                                //recalculate profit/lost 
                                cntrl.Rows[6].Cells[1].Value = MarketPrice.RateWhenProfit(aCoin.SessionStartPrice, aCoin.CurrentPrice, WarnPercentage);
                                cntrl.Rows[7].Cells[1].Value = MarketPrice.RateWhenLost(aCoin.SessionStartPrice, aCoin.CurrentPrice, WarnPercentage);

                                //Change the text
                                cntrl.Rows[6].Cells[0].Value = string.Format("Koers bij {0}% winst", WarnPercentage.ToString()); //New value when profit
                                cntrl.Rows[7].Cells[0].Value = string.Format("Koers bij {0}% verlies", WarnPercentage.ToString());   //New value when lost

                                cntrl.Rows[9].Cells[1].Value = FormatDoubleToString(aCoin.SessionHighPrice);
                                cntrl.Rows[10].Cells[1].Value = FormatDoubleToString(aCoin.SessionLowPrice);

                                cntrl.Rows[12].Cells[1].Value = FormatDoubleToString(aCoin.Open24);
                                cntrl.Rows[13].Cells[1].Value = FormatDoubleToString(aCoin.High);
                                cntrl.Rows[14].Cells[1].Value = FormatDoubleToString(aCoin.Low);
                                cntrl.Rows[15].Cells[1].Value = FormatDoubleToString(aCoin.Volume);
                                cntrl.Rows[16].Cells[1].Value = FormatDoubleToString(aCoin.VolumeQuote);
                                cntrl.Rows[17].Cells[1].Value = FormatDoubleToString(aCoin.Bid);
                                cntrl.Rows[18].Cells[1].Value = FormatDoubleToString(aCoin.Ask);
                                cntrl.Rows[19].Cells[1].Value = FormatDoubleToString(aCoin.BidSize);
                                cntrl.Rows[20].Cells[1].Value = FormatDoubleToString(aCoin.AskSize);
                                //next...

                                //Change color
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
            else  //used when the precentage is changed before start is pressed
            {
                foreach (string DgvName in Prepare.DgvNames)    //loop through the datagridvieuws
                {
                    if (DgvName.Contains("Dgv_1_"))
                    {
                        if (Controls.Find(DgvName, true).FirstOrDefault() is DataGridView cntrl)
                        {
                            //Change the text
                            cntrl.Rows[5].Cells[0].Value = string.Format("Koers bij {0}% winst", WarnPercentage.ToString()); //New value when profit
                            cntrl.Rows[6].Cells[0].Value = string.Format("Koers bij {0}% verlies", WarnPercentage.ToString());   //New value when lost
                        }
                    }
                }
            }
        }        

        private void SetMarketPriceInDataGridView()
        {
            if (AllCoinData != null)
            {
                foreach (CoinData aCoin in AllCoinData.Items)  //CoinDataAll
                {
                    string CoinName = aCoin.Name;

                    foreach (string DgvName in Prepare.DgvNames)    //loop through the datagridvieuws
                    {
                        if (DgvName.Contains(CoinName) && DgvName.Contains("Dgv_2_"))  //see PrepareForm, create datagridview. this is the second dgv
                        {
                            if (Controls.Find(DgvName, true).FirstOrDefault() is DataGridView cntrl)
                            {
                                AddRowToDgvPriceMonitor(cntrl, aCoin.CurrentPrice, aCoin.Trend);
                            }
                        }
                    }
                }
            }
        }       

        private void SaveAllCoinData()
        {
            ApplicationDatabase SaveCoindata = new();
            SaveCoindata.SaveCoinData(AllCoinData);
            SaveCoindata.Dispose();
        }

        #region Charting
        private void PrepareChart()
        {
            foreach (string ChartName in Prepare.ChartNames)
            {
                string CoinName = ChartName.Replace("Chart_", "");
                if (Controls.Find(ChartName, true).FirstOrDefault() is System.Windows.Forms.DataVisualization.Charting.Chart cntrl)
                {
                    string SerieName = CoinName.Replace("_", "-"); //"BTC-EUR";
                    string ChartAreaName = "ChartArea_" + SerieName;

                    //[0] = {Series-BTC-EUR}
                    cntrl.ChartAreas[ChartAreaName].AxisX.IsStartedFromZero = false;
                    cntrl.ChartAreas[ChartAreaName].AxisY.IsStartedFromZero = false;

                    cntrl.Series[SerieName].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;// .Spline;
                    cntrl.Series[SerieName].Color = Color.DarkGreen;
                    cntrl.Series[SerieName].BorderWidth = 3;


                    //[1] = {Series-Start_Prijs_BTC-EUR}
                    cntrl.Series["Start_Prijs_" + SerieName].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    cntrl.Series["Start_Prijs_" + SerieName].Color = Color.LightGreen;
                    cntrl.Series["Start_Prijs_" + SerieName].BorderWidth = 2;


                    //Open_Prijs_       [2] = {Series-Open_Prijs_BTC-EUR}
                    cntrl.Series["Open_Prijs_" + SerieName].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    cntrl.Series["Open_Prijs_" + SerieName].Color = Color.DarkBlue;
                    cntrl.Series["Open_Prijs_" + SerieName].BorderWidth = 2;

                    //Sessie_Hoogste_Prijs      [3] = {Series-Sessie_Hoogste_Prijs_BTC-EUR}
                    cntrl.Series["Sessie_Hoogste_Prijs_" + SerieName].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    cntrl.Series["Sessie_Hoogste_Prijs_" + SerieName].Color = Color.DeepSkyBlue;
                    cntrl.Series["Sessie_Hoogste_Prijs_" + SerieName].BorderWidth = 2;
                    
                    //Sessie_Laagste_Prijs      [4] = {Series-Sessie_Laagste_Prijs_BTC-EUR}
                    cntrl.Series["Sessie_Laagste_Prijs_" + SerieName].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    cntrl.Series["Sessie_Laagste_Prijs_" + SerieName].Color = Color.Tomato;
                    cntrl.Series["Sessie_Laagste_Prijs_" + SerieName].BorderWidth = 2;
                }
            }

        }
        private void Charting()
        {
            foreach (string ChartName in Prepare.ChartNames)  //First loop throug the cahrts and the right chart.  ChartName = "Chart_BTC-EUR"
            {
                string CoinName = ChartName.Replace("Chart_", "");
                if (Controls.Find(ChartName, true).FirstOrDefault() is System.Windows.Forms.DataVisualization.Charting.Chart cntrl)
                {
                    if (AllCoinData != null)
                    {
                        foreach (CoinData aCoin in AllCoinData.Items)  //Then loop through the coindata to findt the right coin and data
                        {
                            if (aCoin.Name == CoinName)
                            {
                                //the current coin price
                                cntrl.Series[CoinName.Replace("_", "-")].Points.AddXY(DateTime.Now.ToString("HH: mm: ss"), aCoin.CurrentPrice);  //[0]

                                if (aCoin.SessionStartPrice != 0)
                                {
                                    cntrl.Series["Start_Prijs_" + CoinName].Points.AddXY(DateTime.Now.ToString("HH: mm: ss"), aCoin.SessionStartPrice);  //[1] = {Series-Start_Prijs_BTC-EUR}
                                }
                                else
                                {
                                    cntrl.Series["Start_Prijs_" + CoinName].Points.AddXY(DateTime.Now.ToString("HH: mm: ss"), aCoin.CurrentPrice);  //first time
                                }
                                /*if (aCoin.Open24 != 0)//
                                {
                                    cntrl.Series["Open_Prijs_" + CoinName].Points.AddXY(DateTime.Now.ToString("HH: mm: ss"), aCoin.Open24);
                                }*/
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

        private void SetChartLine(System.Windows.Forms.DataVisualization.Charting.Chart aChart, string SerieName, double aValue, string ACoin)
        {            
            if (SerieName == "Sessie_Laagste_Prijs_")
            {
                var points = aChart.Series["Sessie_Laagste_Prijs_" + ACoin].Points;
                for (var i = 0; i < points.Count; ++i)
                { points[i].YValues[0] = aValue; }
                aChart.Series.Invalidate();
            }
            if (SerieName == "Sessie_Hoogste_Prijs_")
            {
                var points = aChart.Series["Sessie_Hoogste_Prijs_" + ACoin].Points;
                for (var i = 0; i < points.Count; ++i)
                { points[i].YValues[0] = aValue; }
            }
            
            //aChart.Invalidate();
        }
        #endregion Charting

        public void InitTimer()
        {
            TimerTime = Convert.ToDouble(TextBoxTimeInterval.Text);

            Timer1 = new Timer();
            Timer1.Tick += new EventHandler(Timer1_Tick);
            Timer1.Interval = Convert.ToInt32(Math.Floor(TimerTime));
            Timer1.Start();
        }
        private void PutCoinNamesInComboBox()
        {
            int counter = 0;
            foreach (TabPage tbp in TabControlCharts.TabPages)
            {
                ToolStripComboBoxCoinNames.Items.Add(tbp.Name);
                counter++;
            }

            if (counter > 0)
            {
                ToolStripComboBoxCoinNames.Enabled = true;
            }
            else
            {
                ToolStripComboBoxCoinNames.Enabled = false;
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            GetCoinData();              // Get the current price and add a record in the DataGridViewPriceMonitor            
        }
        private void GetCoinData()  //Current price to Datagridview
        {
            GetMarketPriceData();    //Fill a list with the Currentcoin data
            //en nog meer...............................

        }

        private void AddRowToDgvPriceMonitor(DataGridView dgv, double CurrPrice, String Trend)
        {
            if (!string.IsNullOrEmpty(Trend))
            {
                if (Trend == CoinTrend.Equal.ToString())
                {
                    dgv.Rows.Insert(0, new string[] { FormatDoubleToString(CurrPrice), "=", DateTime.Now.ToString() });                    
                    dgv.Rows[0].Cells[1].Style.BackColor = Color.LightGray;
                }
                else if (Trend == CoinTrend.Up.ToString())
                {
                    string upArrow = "\u25B2";  //= Arrow UP
                    dgv.Rows.Insert(0, new string[] { FormatDoubleToString(CurrPrice), upArrow, DateTime.Now.ToString() });

                    dgv.Rows[0].Cells[1].Style.BackColor = Color.LightGreen;                    
                }
                else if (Trend == CoinTrend.Down.ToString())
                {
                    string downArrow = "\u25BC";  // Arrow down
                    dgv.Rows.Insert(0, new string[] { FormatDoubleToString(CurrPrice), downArrow, DateTime.Now.ToString() });
                    dgv.Rows[0].Cells[1].Style.BackColor = Color.MistyRose;
                }
                else
                {
                    dgv.Rows.Insert(0, new string[] { FormatDoubleToString(CurrPrice), "?", DateTime.Now.ToString() });
                    dgv.Rows[0].Cells[1].Style.BackColor = Color.DarkBlue;
                }
            }            
        }
        private string FormatDoubleToString(double d)
        {
            string FormattedPrice = d.ToString(DoubleFormatString);

            if (FormattedPrice.Length > 0 && FormattedPrice != "0")
            {

                string s = FormattedPrice.Substring(0, 1);

                if (s == "," || s == ".")
                {
                    FormattedPrice = "0" + FormattedPrice;
                }
                return FormattedPrice;
            }
            else
            {
                return "0";
            }
        }
        
        #region Keypress only numbers
        private void TextBoxTimeInterval_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeyPresstextBox(sender, e);
        }
        private void KeyPresstextBox(object sender, KeyPressEventArgs e)
        {
            char seperator = DecimalSeperator[0];

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
            Close();
        }

        private void TextBoxWarnPercentage_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeyPresstextBox(sender, e);
        }

        private void TextBoxTimeInterval_Leave(object sender, EventArgs e)
        {
            //The timer interval can not be empty. Empty when leaving the textbox, then set to the default value = 1
            if (TextBoxTimeInterval.Text == "0" ||
                TextBoxTimeInterval.Text == "," ||
                string.IsNullOrEmpty(TextBoxTimeInterval.Text) || string.IsNullOrWhiteSpace(TextBoxTimeInterval.Text))
            {
                TextBoxTimeInterval.Text = "1";
            }
            TimerTime = Convert.ToDouble(TextBoxTimeInterval.Text);
        }

        private void TextBoxWarnPercentage_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TextBoxWarnPercentage.Text) || !String.IsNullOrWhiteSpace(TextBoxWarnPercentage.Text))
            {
                WarnPercentage = Convert.ToDouble(TextBoxWarnPercentage.Text);
            }
            if (Prepare != null)
            {
                Prepare.WarnPercentage= WarnPercentage;
                SetCoinDataInDataGridView();
            }
        }


        #region Stop a session
        private void ToolStripMenuItem_Session_Stop_Click(object sender, EventArgs e)
        {
            StopCurrentRun();
        }
        private void StopCurrentRun()
        {
            Timer1.Stop();
            HandleControlsState(SessionAction.Stop.ToString()); //Set buttons on/off
            //TODO: //MemuIte disable....

            FirstRun = true;

            this.Text = this.Text.Replace("   -->  Bezig...", "");
            Cursor.Current = Cursors.Default;
        }
        #endregion Stop a session

        #region Pause a session
        private void ToolStripMenuItem_Session_Pause_Click(object sender, EventArgs e)
        {
            HandleControlsState(SessionAction.Pause.ToString()); //Set buttons on/off
            if (ButtonPause.Text == "Pauze")
            {
                this.Text = this.Text.Replace("Bezig", "Pauze");
                ToolStripStatusLabel1.Text = ToolStripStatusLabel1.Text.Replace("Bezig", "Pauze");
                Timer1.Stop();
                ButtonPause.Text = "Verder";
                ToolStripMenuItem_Session_Pause.Text = "Verder";
                ToolStripButton_SessionPause.Text = "Verder";
            }
            else if (ButtonPause.Text == "Verder")
            {
                this.Text = this.Text.Replace("Pauze", "Bezig");
                ToolStripStatusLabel1.Text = ToolStripStatusLabel1.Text.Replace("Pauze", "Bezig");
                ButtonPause.Text = "Pauze";
                ToolStripMenuItem_Session_Pause.Text = "Pauze";
                ToolStripButton_SessionPause.Text = "Pauze";
                InitTimer();
            }

            Cursor.Current = Cursors.Default;
        }
        #endregion Pause a session

        private void ToolStripMenuItem_Options_Configure_Click(object sender, EventArgs e)
        {
            Logging.WriteToLogInformation("Openen configuratie scherm.");

            SettingsManager.SaveSettings(JsonObjSettings);

            FormConfigure _frm = new()
            {
                DebugMode = this.DebugMode
            };
            _frm.ShowDialog();
            _frm.Dispose();

            GetSettings();
            ApplySettings();
            CreateTheTabs();
        }

        #region Form closing
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveFormPosition();
            SaveSettings();
            Logging.StopLogging();
        }
        private void SaveFormPosition()
        {
            using FormPosition FormPosition = new(this)
            {
                DebugMode = this.DebugMode
            };
            FormPosition.SaveMainFormPosition();
        }
        private void SaveSettings()
        {
            SettingsManager.SaveSettings(JsonObjSettings);
        }
        #endregion Form closing

        


        #region Show date and time in form
        System.Windows.Forms.Timer t = null;
        private void StartClock()
        {
            t = new System.Windows.Forms.Timer();
            t.Interval = 1000;
            t.Tick += new EventHandler(t_Tick);
            t.Enabled = true;
        }

        void t_Tick(object sender, EventArgs e)
        {
            ToolStripStatusLabel2.Text = DateTime.Now.Date.ToString("d/M/yyyy") + "   -   " + DateTime.Now.ToString("HH:mm:ss");
        }
        #endregion Show date and time in form

        private void ToolStripComboBoxCoinNames_SelectedIndexChanged(object sender, EventArgs e)
        {   //active the selected tabpage
            Cursor.Current = Cursors.WaitCursor;

            string selected = ToolStripComboBoxCoinNames.SelectedItem.ToString();

            if (!string.IsNullOrEmpty(selected))
            {
                TabControlMain.SelectedIndex = 1;  //Grafieken

                TabPage t = TabControlCharts.TabPages[selected];
                TabControlCharts.SelectTab(t); //go to tab
            }
            Cursor.Current = Cursors.Default;
        }
    }
}
