namespace CM
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SQLite;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using Microsoft.Win32.SafeHandles;

    /// <summary>
    /// Create and maint the application database.
    /// </summary>
    public class ApplicationDatabase : SQliteDatabaseConnection, IDisposable
    {
        #region fields
        private readonly int latestDbVersion;

        #endregion fields

        #region Query strings (fields)
        private readonly string creMetaTbl = "CREATE TABLE IF NOT EXISTS " + TableName.SETTINGS_META + "(" +
                                "KEY                VARCHAR(50)  UNIQUE  ," +
                                "VALUE              VARCHAR(255))";

        private readonly string creCoinNames = "CREATE TABLE IF NOT EXISTS " + TableName.COIN_NAMES + "(" +
                                "ID                  INTEGER         NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE," +
                                "GUID                VARCHAR(50)  UNIQUE                                      ," +
                                "NAME                VARCHAR(25)                                              ," +
                                "DEFAULT_NAME        BOOL                                                     ," +
                                "DATE_CREATED        DATE                                                     ," +
                                "DATE_ALTERED        DATE                                                     ," +
                                "CREATED_BY          VARCHAR(100)                                             ," +
                                "ALTERED_BY          VARCHAR(100))";

        #endregion Query strings (fields)

        #region Properties

        private bool Error { get; set; }

        private string DbFileName { get; set; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDatabase"/> class.
        /// </summary>
        public ApplicationDatabase()
        {
            this.Error = false;
            this.latestDbVersion = AppSettingsDefault.UpdateVersion;
            this.DbFileName = AppSettingsDefault.SqlLiteDatabaseName;
        }
        #endregion Constructor

        #region Create SQLite Database file

        /// <summary>
        /// Create a new database file and the tables.
        /// </summary>
        /// <returns>True if succeeded.</returns>
        public bool CreateNewDatabase()
        {
            this.CreateDbFile();
            if (!this.Error)
            {
                this.CreateTable(this.creMetaTbl, TableName.SETTINGS_META, "0");
                this.InsertMeta("0");
                return true;
            }
            else
            {
                return false;
            }
        }

        private void CreateDbFile()
        {
            if (!string.IsNullOrEmpty(this.DatabaseFileName))
            {
                try
                {
                    // Only with a first install. (Unless a user removed the database file).
                    if (!File.Exists(this.DatabaseFileName))
                    {
                        SQLiteConnection.CreateFile(this.DatabaseFileName);  // The ceation of the new empty database file
                        Logging.WriteToLogInformation("De database '" + this.DatabaseFileName + "' is aangemaakt.");
                    }
                    else
                    {
                        Logging.WriteToLogInformation("Het database bestand is aanwezig, er is géén nieuw leeg database bestand aangemaakt.");
                    }
                }
                catch (IOException ex)
                {
                    this.Error = true;
                    Logging.WriteToLogError("De database '" + this.DbFileName + "' is niet aangemaakt.");
                    Logging.WriteToLogError("Melding :");
                    Logging.WriteToLogError(ex.Message);
                    if (CmDebugMode.DebugMode)
                    {
                        Logging.WriteToLogDebug(ex.ToString());
                    }

                    Cursor.Current = Cursors.Default;
                    MessageBox.Show(
                        "Aanmaken leeg database bestand is mislukt.",
                        "Fout",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    this.Error = true;
                    Logging.WriteToLogError("De database '" + this.DbFileName + "' is niet aangemaakt.");
                    Logging.WriteToLogError("Melding :");
                    Logging.WriteToLogError(ex.Message);
                    if (CmDebugMode.DebugMode)
                    {
                        Logging.WriteToLogDebug(ex.ToString());
                    }

                    Cursor.Current = Cursors.Default;
                    MessageBox.Show(
                        "Aanmaken leeg database bestand is mislukt.",
                        "Fout",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            else
            {
                Logging.WriteToLogError("De SQlite database is niet aangemaakt omdat er geen locatie of database naam is opgegeven.");
                Cursor.Current = Cursors.Default;
                MessageBox.Show(
                    "De Applicatie database is niet aangemaakt omdat er geen locatie en of database naam is opgegeven.",
                    "Fout",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void InsertMeta(string version)
        {
            if (!this.Error)
            {
                this.dbConnection.Open();
                string insertSQL = string.Format("INSERT INTO {0} VALUES('VERSION', @VERSION)", TableName.SETTINGS_META);

                SQLiteCommand command = new(insertSQL, this.dbConnection);
                try
                {
                    command.Parameters.Add(new SQLiteParameter("@VERSION", version));

                    command.ExecuteNonQuery();
                    Logging.WriteToLogInformation(string.Format("De tabel {0} is gewijzigd. (Versie ", TableName.SETTINGS_META) + version + ").");
                }
                catch (SQLiteException ex)
                {
                    Logging.WriteToLogError(string.Format("Het invoeren van het database versienummer in de tabel {0} is misukt. (Versie ", TableName.SETTINGS_META) + Convert.ToString(this.latestDbVersion, CultureInfo.InvariantCulture) + ").");
                    Logging.WriteToLogError("Melding :");
                    Logging.WriteToLogError(ex.Message);
                    if (CmDebugMode.DebugMode)
                    {
                        Logging.WriteToLogDebug(ex.ToString());
                    }

                    this.Error = true;
                }
                finally
                {
                    command.Dispose();
                    this.dbConnection.Close();
                }
            }
            else
            {
                Logging.WriteToLogError(string.Format("Het invoeren van het database versienummer in de tabel {0} is mislukt.", TableName.SETTINGS_META));
            }
        }
        #endregion Create SQLite Database file

        #region create all the tables
        private void CreateTable(string sqlCreate, string tableName, string version)
        {
            if (!this.Error)
            {
                this.dbConnection.Open();

                SQLiteCommand command = new(sqlCreate, this.dbConnection);
                try
                {
                    command.ExecuteNonQuery();
                    Logging.WriteToLogInformation(string.Format("De tabel {0} is aangemaakt. (Versie {1}).", tableName, version));
                }
                catch (SQLiteException ex)
                {
                    Logging.WriteToLogError(string.Format("Aanmaken van de tabel {0} is misukt. (Versie {1}).", tableName, version));
                    Logging.WriteToLogError("Melding :");
                    Logging.WriteToLogError(ex.Message);
                    if (CmDebugMode.DebugMode)
                    {
                        Logging.WriteToLogDebug(ex.ToString());
                    }

                    this.Error = true;
                }
                finally
                {
                    command.Dispose();
                    this.dbConnection.Close();
                }
            }
            else
            {
                Logging.WriteToLogError(string.Format("Het aanmaken van de tabel {0} is niet uitgevoerd.", tableName));
            }
        }

        /// <summary>
        /// Create the table which holds the coin data.
        /// </summary>
        /// <param name="tableName">The name of the coin is the table name.</param>
        public void CreateCoinTable(string tableName)
        {
            if (!this.Error)
            {
                string sqlCreate = "CREATE TABLE IF NOT EXISTS EC_" + tableName.Replace("-", "_") + "(" +
                                "ID                         INTEGER         NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE," +
                                "GUID                       VARCHAR(50)  UNIQUE                                      ," +
                                "NAME                       VARCHAR(25)                                              ," +
                                "CURRENT_PRICE              REAL                ," +
                                "PREVIOUS_PRICE             REAL                ," +
                                "DIFFERENCE_PRICE           REAL                ," +
                                "DIFFERENCE_PERCENTAGE      REAL                ," +
                                "DIFFERENCE                 VARCHAR(5)          ," +
                                "SESSION_OPENPRICE          REAL                ," +
                                "SESSION_HIGH               REAL                ," +
                                "SESSION_LOW                REAL                ," +
                                "OPEN                       REAL                ," +
                                "LOW                        REAL                ," +
                                "HIGH                       REAL                ," +
                                "VOLUME                     REAL                ," +

                                "DATE_CREATED        DATE                                                     ," +
                                "DATE_ALTERED        DATE                                                     ," +
                                "CREATED_BY          VARCHAR(100)                                             ," +
                                "ALTERED_BY          VARCHAR(100))";

                this.dbConnection.Open();

                SQLiteCommand command = new(sqlCreate, this.dbConnection);
                try
                {
                    command.ExecuteNonQuery();
                    Logging.WriteToLogInformation(string.Format("De tabel {0} is aangemaakt.", tableName));
                }
                catch (SQLiteException ex)
                {
                    Logging.WriteToLogError(string.Format("Aanmaken van de tabel {0} is misukt", tableName));
                    Logging.WriteToLogError("Melding :");
                    Logging.WriteToLogError(ex.Message);
                    if (CmDebugMode.DebugMode)
                    {
                        Logging.WriteToLogDebug(ex.ToString());
                    }

                    this.Error = true;
                }
                finally
                {
                    command.Dispose();
                    this.dbConnection.Close();
                }
            }
            else
            {
                Logging.WriteToLogError(string.Format("Het aanmaken van de tabel {0} is niet uitgevoerd.", tableName));
            }
        }

        /// <summary>
        /// Update the application database when the version is updated to a higher version number.
        /// </summary>
        /// <returns>True if update has succeeded.</returns>
        public bool UpdateDatabase()
        {
            bool succes = false;
            string version;
            if (this.latestDbVersion >= 1 && this.SelectMeta() == 0)
            {
                version = "1";
                this.CreateTable(this.creCoinNames, TableName.COIN_NAMES, version);
                this.UpdateMeta(version);  // Set the version 1
            }

            if (!this.Error)
            {
                succes = true;
            }

            return succes;
        }

        private void UpdateMeta(string version)
        {
            this.dbConnection.Open();

            using var tr = this.dbConnection.BeginTransaction();
            string insertSQL = string.Format("UPDATE {0} SET VALUE  = @VERSION WHERE KEY = @KEY", TableName.SETTINGS_META);

            SQLiteCommand command = new(insertSQL, this.dbConnection);
            try
            {
                command.Parameters.Add(new SQLiteParameter("@VERSION", version));
                command.Parameters.Add(new SQLiteParameter("@KEY", "VERSION"));

                command.ExecuteNonQuery();
                Logging.WriteToLogInformation("De tabel " + TableName.SETTINGS_META + " is gewijzigd. (Versie " + version + ").");
                command.Dispose();
                tr.Commit();
            }
            catch (SQLiteException ex)
            {
                Logging.WriteToLogError("Wijzigen tabel " + TableName.SETTINGS_META + " versie is misukt. (Versie " + version + ").");
                Logging.WriteToLogError("Melding :");
                Logging.WriteToLogError(ex.Message);
                if (CmDebugMode.DebugMode)
                {
                    Logging.WriteToLogDebug(ex.ToString());
                }

                command.Dispose();
                tr.Rollback();
            }
            finally
            {
                this.dbConnection.Close();
            }
        }

        /// <summary>
        /// Check the application database version.
        /// </summary>
        /// <returns>the application database version.</returns>
        public int SelectMeta()
        {
            int sqlLiteMetaVersion = 0;

            string sqlSelect = "SELECT VALUE FROM " + TableName.SETTINGS_META + " WHERE KEY = 'VERSION'";

            this.dbConnection.Open();
            Logging.WriteToLogInformation("Controle op versie van de query database.");

            SQLiteCommand command = new(sqlSelect, this.dbConnection);
            try
            {
                SQLiteDataReader dr = command.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    sqlLiteMetaVersion = int.Parse(dr[0].ToString(), CultureInfo.InvariantCulture);
                }

                dr.Close();
            }
            catch (SQLiteException ex)
            {
                Logging.WriteToLogError("Opvragen meta versie is misukt. (Versie " + Convert.ToString(this.latestDbVersion, CultureInfo.InvariantCulture) + ").");
                Logging.WriteToLogError("Melding :");
                Logging.WriteToLogError(ex.Message);
                if (CmDebugMode.DebugMode)
                {
                    Logging.WriteToLogDebug(ex.ToString());
                }

                this.Error = true;
                sqlLiteMetaVersion = 9999;
            }
            finally
            {
                command.Dispose();
                this.dbConnection.Close();
            }

            return sqlLiteMetaVersion;
        }
        #endregion create all the tables

        #region Insert

        /// <summary>
        /// Save the selected coin names in the table COIN_NAMES.
        /// </summary>
        /// <param name="coinNames">A list with the coin names.</param>
        public void SaveCoinNames(List<string> coinNames)
        {
            this.DeleteCoinNames();  // First delete the existing coin names.

            this.dbConnection.Open();

            using (var tr = this.dbConnection.BeginTransaction())
            {
                try
                {
                    foreach (string coinName in coinNames)
                    {
                        string insertSql = string.Format("insert into {0}(GUID, NAME, DATE_CREATED, CREATED_BY) ", TableName.COIN_NAMES);
                        insertSql += "values(@GUID, @NAME, @DATE_CREATED, @CREATED_BY)";

                        SQLiteCommand command = new(insertSql, this.dbConnection);

                        command.Prepare();
                        command.Parameters.Add(new SQLiteParameter("@GUID", Guid.NewGuid().ToString()));
                        command.Parameters.Add(new SQLiteParameter("NAME", coinName));
                        command.Parameters.Add(new SQLiteParameter("@DATE_CREATED", DateTime.Now));
                        command.Parameters.Add(new SQLiteParameter("@CREATED_BY", string.Empty));  // Later when orders etc are added. Then some restrictions are needed

                        command.ExecuteNonQuery();
                        command.Dispose();
                    }

                    tr.Commit();
                }
                catch (SQLiteException ex)
                {
                    tr.Rollback();
                    Logging.WriteToLogError("Het invoeren van de Coin namen is mislukt.");
                    Logging.WriteToLogError("Melding :");
                    Logging.WriteToLogError(ex.Message);
                    if (CmDebugMode.DebugMode)
                    {
                        Logging.WriteToLogDebug(ex.ToString());
                    }
                }
                finally
                {
                    this.dbConnection.Close();
                }
            }
        }

        private void DeleteCoinNames()
        {
            this.dbConnection.Open();

            using var tr = this.dbConnection.BeginTransaction();
            try
            {
                string deleteSql = string.Format("delete from {0}", TableName.COIN_NAMES);

                SQLiteCommand command = new(deleteSql, this.dbConnection);

                command.ExecuteNonQuery();
                command.Dispose();
                tr.Commit();
            }
            catch (SQLiteException ex)
            {
                Logging.WriteToLogError("Het verwijderen van de Coin namen is mislukt.");
                Logging.WriteToLogError("Melding :");
                Logging.WriteToLogError(ex.Message);
                if (CmDebugMode.DebugMode)
                {
                    Logging.WriteToLogDebug(ex.ToString());
                }

                tr.Rollback();
            }
            finally
            {
                this.dbConnection.Close();
            }
        }

        /// <summary>
        /// Save the data of the coins.
        /// </summary>
        /// <param name="allCoinData">object with all the coin data.</param>
        public void SaveCoinData(CoinDataAll allCoinData)
        {
            this.dbConnection.Open();

            using (var tr = this.dbConnection.BeginTransaction())
            {
                try
                {
                    foreach (CoinData aCoin in allCoinData.Items)
                    {
                        string insertSql = "insert into EC_" + aCoin.Name.Replace("-", "_") + "(GUID, NAME, CURRENT_PRICE, PREVIOUS_PRICE, DIFFERENCE_PRICE, DIFFERENCE_PERCENTAGE, ";
                        insertSql += "DIFFERENCE, SESSION_OPENPRICE, SESSION_HIGH, SESSION_LOW, OPEN, LOW, HIGH, VOLUME, DATE_CREATED) ";
                        insertSql += "values(@GUID, @NAME, @CURRENT_PRICE, @PREVIOUS_PRICE, @DIFFERENCE_PRICE, @DIFFERENCE_PERCENTAGE, ";
                        insertSql += "@DIFFERENCE, @SESSION_OPENPRICE, @SESSION_HIGH, @SESSION_LOW, @OPEN, @LOW, @HIGH, @VOLUME, @DATE_CREATED)";

                        SQLiteCommand command = new(insertSql, this.dbConnection);

                        command.Prepare();
                        command.Parameters.Add(new SQLiteParameter("@GUID", Guid.NewGuid().ToString()));
                        command.Parameters.Add(new SQLiteParameter("NAME", aCoin.Name));

                        command.Parameters.Add(new SQLiteParameter("@CURRENT_PRICE", aCoin.CurrentPrice));
                        command.Parameters.Add(new SQLiteParameter("@PREVIOUS_PRICE", aCoin.PreviousPrice));
                        command.Parameters.Add(new SQLiteParameter("@DIFFERENCE_PRICE", aCoin.DiffValuta));
                        command.Parameters.Add(new SQLiteParameter("@DIFFERENCE_PERCENTAGE", aCoin.DiffPercent));
                        command.Parameters.Add(new SQLiteParameter("@DIFFERENCE", aCoin.Trend));
                        command.Parameters.Add(new SQLiteParameter("@SESSION_OPENPRICE", aCoin.SessionStartPrice));
                        command.Parameters.Add(new SQLiteParameter("@SESSION_HIGH", aCoin.SessionHighPrice));
                        command.Parameters.Add(new SQLiteParameter("@SESSION_LOW", aCoin.SessionLowPrice));
                        command.Parameters.Add(new SQLiteParameter("@OPEN", aCoin.Open24));
                        command.Parameters.Add(new SQLiteParameter("@LOW", aCoin.Low));
                        command.Parameters.Add(new SQLiteParameter("@HIGH", aCoin.High));
                        command.Parameters.Add(new SQLiteParameter("@VOLUME", aCoin.Volume));
                        command.Parameters.Add(new SQLiteParameter("@DATE_CREATED", DateTime.Now));

                        command.ExecuteNonQuery();
                        command.Dispose();
                    }

                    tr.Commit();
                }
                catch (SQLiteException ex)
                {
                    tr.Rollback();

                    Logging.WriteToLogError("Het invoeren van de Coin data is mislukt.");
                    Logging.WriteToLogError("Melding :");
                    Logging.WriteToLogError(ex.Message);
                    if (CmDebugMode.DebugMode)
                    {
                        Logging.WriteToLogDebug(ex.ToString());
                    }
                }
                finally
                {
                    this.dbConnection.Close();
                }
            }
        }

        #endregion Insert

        #region Get

        /// <summary>
        /// Get the selected coin names.
        /// </summary>
        /// <returns>A list with the coin names.</returns>
        public List<string> GetSelectedCoinNames()
        {
            List<string> coinNames = new();

            this.dbConnection.Open();

            using (var tr = this.dbConnection.BeginTransaction())
            {
                try
                {
                    string selectSql = string.Format("select NAME from {0}", TableName.COIN_NAMES);

                    SQLiteCommand command = new(selectSql, this.dbConnection);

                    SQLiteDataReader dr = command.ExecuteReader();
                    DataTable dt = new();
                    dt.Load(dr);

                    dr.Close();
                    command.Dispose();

                    if (dt.IsInitialized == true)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            coinNames.Add(row["NAME"].ToString());
                        }
                    }
                    else
                    {
                        Logging.WriteToLogInformation("Er zijn geen Coin namen gevonden in de tabel COIN_NAMES.");
                    }

                    dt.Dispose();
                    tr.Commit();
                }
                catch (SQLiteException ex)
                {
                    Logging.WriteToLogError("Het verwijderen van de Coin namen is mislukt.");
                    Logging.WriteToLogError("Melding :");
                    Logging.WriteToLogError(ex.Message);
                    if (CmDebugMode.DebugMode)
                    {
                        Logging.WriteToLogDebug(ex.ToString());
                    }

                    tr.Rollback();
                    return null;
                }
                finally
                {
                    this.dbConnection.Close();
                }
            }

            if (coinNames.Count > 0)
            {
                return coinNames;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get the SQLite version.
        /// </summary>
        /// <returns>The Sqllite version.</returns>
        public string GetSQliteVersion()
        {
            this.dbConnection.Open();

            try
            {
                string selectSql = "select sqlite_version()";

                SQLiteCommand command = new(selectSql, this.dbConnection);

                string version = command.ExecuteScalar().ToString();

                if (!string.IsNullOrEmpty(version))
                {
                    this.dbConnection.Close();
                    return version;
                }
                else
                {
                    this.dbConnection.Close();
                    return null;
                }
            }
            catch (SQLiteException ex)
            {
                Logging.WriteToLogError("Het opvragen van de SQLite versie is mislukt.");
                Logging.WriteToLogError("Melding:");
                Logging.WriteToLogError(ex.Message);
                if (CmDebugMode.DebugMode)
                {
                    Logging.WriteToLogDebug(ex.ToString());
                }

                this.dbConnection.Close();
                return null;
            }
        }

        #endregion Get

        #region Maintenance

        /// <summary>
        /// Make a copy of the database file.
        /// </summary>
        /// <param name="type">Type means copy automatic whithin the startup of te application or copy when choosen the copy function.</param>
        /// <returns>True if succeeded.</returns>
        public bool CopyDatabaseFile(string type)
        {
            string fileToCopy = this.DatabaseFileName;

            DateTime dateTime = DateTime.UtcNow.Date;
            string currentDate = dateTime.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);

            string newLocation = Path.Combine(this.DbLocation, AppSettingsDefault.BackUpFolder) + currentDate + "_" + AppSettingsDefault.SqlLiteDatabaseName;

            bool result = false;

            if (Directory.Exists(Path.Combine(this.DbLocation, AppSettingsDefault.BackUpFolder)))
            {
                if (File.Exists(fileToCopy))
                {
                    if (type == "StartUp")
                    {
                        File.Copy(fileToCopy, newLocation, true);  // Overwrite file = true
                        Logging.WriteToLogInformation(string.Format("Het kopiëren van het bestand '{0}' is gereed.", AppSettingsDefault.SqlLiteDatabaseName));
                        result = true;
                    }
                    else
                    {
                        if (File.Exists(newLocation))
                        {
                            DialogResult dialogResult = MessageBox.Show("Het bestand bestaat al. Wilt u het bestand overschrijven?", "Kopiëren bestand.", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dialogResult == DialogResult.Yes)
                            {
                                File.Copy(fileToCopy, newLocation, true);  // Overwrite file = true
                                Logging.WriteToLogInformation(string.Format("Het kopiëren van het bestand '{0}' is gereed.", AppSettingsDefault.SqlLiteDatabaseName));
                                result = true;
                            }
                            else if (dialogResult == DialogResult.No)
                            {
                                Logging.WriteToLogInformation(string.Format("Het kopiëren van het bestand '{0}' is afgebroken.", AppSettingsDefault.SqlLiteDatabaseName));
                                Logging.WriteToLogInformation("Het bestand komt reeds voor.");
                                result = false;
                            }
                        }
                        else
                        {
                            File.Copy(fileToCopy, newLocation, false);  // Overwrite file = false
                            Logging.WriteToLogInformation(string.Format("Het kopiëren van het bestand '{0}' is gereed.", AppSettingsDefault.SqlLiteDatabaseName));
                            result = true;
                        }
                    }
                }
                else
                {
                    Logging.WriteToLogInformation("Het te kopiëren bestand '" + AppSettingsDefault.SqlLiteDatabaseName + "' is niet aanwezig.");
                    MessageBox.Show(
                        "Het bestand '" + AppSettingsDefault.SqlLiteDatabaseName + "' is niet aanwezig.",
                        "Fout bij kopiëren bestand.",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    result = false;
                }
            }
            else
            {
                MessageBox.Show(
                    "De map '" + AppSettingsDefault.BackUpFolder + "' is niet aanwezig.",
                    "Fout bij kopiëren bestand.",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Compress the database.
        /// </summary>
        public void CompressDatabase()
        {
            this.dbConnection.Open();
            SQLiteCommand command = new(this.dbConnection);
            command.Prepare();
            command.CommandText = "vacuum;";
            try
            {
                command.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                Logging.WriteToLogError("Het comprimeren van de applicatie database is mislukt.");
                Logging.WriteToLogError("Melding :");
                Logging.WriteToLogError(ex.Message);
                if (CmDebugMode.DebugMode)
                {
                    Logging.WriteToLogDebug(ex.ToString());
                }
            }
            finally
            {
                command.Dispose();
                this.dbConnection.Close();
            }
        }
        #endregion Maintenance

        #region Dispose

        private readonly SafeHandle safeHandle = new SafeFileHandle(IntPtr.Zero, true);

        private bool disposed;

        /// <summary>
        /// Implement IDisposable.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected implementation of Dispose pattern.
        /// </summary>
        /// <param name="disposing">Has Dispose already been called.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.safeHandle?.Dispose();

                // Free any other managed objects here.
            }

            this.disposed = true;
        }
        #endregion Dispose
    }

    /// <summary>
    /// Holds the table names.
    /// </summary>
    public static class TableName
    {
        /// <summary>
        /// Gets the tablename: SETTINGS_META.
        /// </summary>
        public static string SETTINGS_META { get { return "SETTINGS_META"; } }

        /// <summary>
        /// Gets the tablename: COIN_NAMES.
        /// </summary>
        public static string COIN_NAMES { get { return "COIN_NAMES"; } }
    }
}
