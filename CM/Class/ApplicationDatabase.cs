using System;
using System.IO;
using System.Windows.Forms;
using System.Globalization;
using System.Data.SQLite;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using HvBLogging;
using System.Collections.Generic;
using System.Data;

namespace CM
{
    public class ApplicationDatabase : SQliteDatabaseConnection, IDisposable
    {
        #region Properties
        public bool DebugMode { get; set; }

        private bool Error { get; set; }
        private readonly int LatestDbVersion;

        private string DbFileName { get; set; }

        #endregion Properties

        #region Query strings
        private readonly string CreMetaTbl = "CREATE TABLE IF NOT EXISTS " + TableName.SETTINGS_META + "(" +
                                "KEY                VARCHAR(50)  UNIQUE  ," +
                                "VALUE              VARCHAR(255))";

        private readonly string CreCoinNames = "CREATE TABLE IF NOT EXISTS " + TableName.COIN_NAMES + "(" +
                                "ID                  INTEGER         NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE," +
                                "GUID                VARCHAR(50)  UNIQUE                                      ," +
                                "NAME                VARCHAR(25)                                              ," +                                
                                "DATE_CREATED        DATE                                                     ," +
                                "DATE_ALTERED        DATE                                                     ," +
                                "CREATED_BY          VARCHAR(100)                                             ," +
                                "ALTERED_BY          VARCHAR(100))";


        #endregion Query strings

        #region Constructor
        public ApplicationDatabase()
        {
            Error = false;
            LatestDbVersion = AppSettingsDefault.UpdateVersion;
            DbFileName = AppSettingsDefault.SqlLiteDatabaseName;
        }
        #endregion Constructor


        #region Create SQLite Database file

        public bool CreateNewDatabase()
        {
            CreateDbFile();
            if (!Error)
            {
                CreateTable(CreMetaTbl, TableName.SETTINGS_META, "0");
                InsertMeta("0");
                return true;
            }
            else
            {
                return false;
            }
        }
        private void CreateDbFile()
        {
            if (!string.IsNullOrEmpty(DatabaseFileName))
            {
                try
                {
                    if (!File.Exists(DatabaseFileName))  //Only with a first install. (unless a user removed the database file)
                    {
                        SQLiteConnection.CreateFile(DatabaseFileName);  //the ceation of the new empty database file
                        Logging.WriteToLogInformation("De database '" + DatabaseFileName + "' is aangemaakt.");
                    }
                    else
                    {
                        Logging.WriteToLogInformation("Het database bestand is aanwezig, er is géén nieuw leeg database bestand aangemaakt.");
                    }
                }
                catch (IOException ex)
                {
                    Error = true;
                    Logging.WriteToLogError("De database '" + DbFileName + "' is niet aangemaakt.");
                    Logging.WriteToLogError("Melding :");
                    Logging.WriteToLogError(ex.Message);
                    if (DebugMode) { Logging.WriteToLogDebug(ex.ToString()); }
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Aanmaken leeg database bestand is mislukt."
                                    , "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    Error = true;
                    Logging.WriteToLogError("De database '" + DbFileName + "' is niet aangemaakt.");
                    Logging.WriteToLogError("Melding :");
                    Logging.WriteToLogError(ex.Message);
                    if (DebugMode) { Logging.WriteToLogDebug(ex.ToString()); }
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Aanmaken leeg database bestand is mislukt."
                                    , "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                Logging.WriteToLogError("De SQlite database is niet aangemaakt omdat er geen locatie of database naam is opgegeven.");
                Cursor.Current = Cursors.Default;
                MessageBox.Show("De Applicatie database is niet aangemaakt omdat er geen locatie en of database naam is opgegeven."
                                , "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void InsertMeta(string Version)
        {
            if (!Error)
            {
                dbConnection.Open();
                string insertSQL = "INSERT INTO SETTINGS_META VALUES('VERSION', @VERSION)";

                SQLiteCommand command = new(insertSQL, dbConnection);
                try
                {
                    command.Parameters.Add(new SQLiteParameter("@VERSION", Version));

                    command.ExecuteNonQuery();
                    Logging.WriteToLogInformation("De tabel SETTINGS_META is gewijzigd. (Versie " + Version + ").");
                }
                catch (SQLiteException ex)
                {
                    Logging.WriteToLogError("Het invoeren van het database versienummer in de tabel SETTINGS_META is misukt. (Versie " + Convert.ToString(LatestDbVersion, CultureInfo.InvariantCulture) + ").");
                    Logging.WriteToLogError("Melding :");
                    Logging.WriteToLogError(ex.Message);
                    if (this.DebugMode) { Logging.WriteToLogDebug(ex.ToString()); }
                    this.Error = true;
                }
                finally
                {
                    command.Dispose();
                    dbConnection.Close();
                }
            }
            else
            {
                Logging.WriteToLogError("Het invoeren van het database versienummer in de tabel SETTINGS_META is mislukt.");
            }
        }
        #endregion Create SQLite Database file

        #region create all the tables
        private void CreateTable(string SqlCreateString, string TableName, string Version)
        {
            if (!Error)
            {
                dbConnection.Open();

                SQLiteCommand command = new(SqlCreateString, dbConnection);
                try
                {
                    command.ExecuteNonQuery();
                    Logging.WriteToLogInformation(string.Format("De tabel {0} is aangemaakt. (Versie {1}).", TableName, Version));
                }
                catch (SQLiteException ex)
                {
                    Logging.WriteToLogError(string.Format("Aanmaken van de tabel {0} is misukt. (Versie {1}).", TableName, Version));
                    Logging.WriteToLogError("Melding :");
                    Logging.WriteToLogError(ex.Message);
                    if (this.DebugMode) { Logging.WriteToLogDebug(ex.ToString()); }
                    this.Error = true;
                }
                finally
                {
                    command.Dispose();
                    dbConnection.Close();
                }
            }
            else
            {
                Logging.WriteToLogError(string.Format("Het aanmaken van de tabel {0} is niet uitgevoerd.", TableName));
            }
        }

        public void CreateCoinTable(string TableName)
        {
            if (!Error)
            {
                string SqlCreateString = "CREATE TABLE IF NOT EXISTS EC_" + TableName.Replace("-","_") + "(" +
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

                dbConnection.Open();

                SQLiteCommand command = new(SqlCreateString, dbConnection);
                try
                {
                    command.ExecuteNonQuery();
                    Logging.WriteToLogInformation(string.Format("De tabel {0} is aangemaakt.", TableName));
                }
                catch (SQLiteException ex)
                {
                    Logging.WriteToLogError(string.Format("Aanmaken van de tabel {0} is misukt", TableName));
                    Logging.WriteToLogError("Melding :");
                    Logging.WriteToLogError(ex.Message);
                    if (this.DebugMode) { Logging.WriteToLogDebug(ex.ToString()); }
                    this.Error = true;
                }
                finally
                {
                    command.Dispose();
                    dbConnection.Close();
                }
            }
            else
            {
                Logging.WriteToLogError(string.Format("Het aanmaken van de tabel {0} is niet uitgevoerd.", TableName));
            }
        }

        public bool UpdateDatabase()
        {
            bool Succes = false;
            string Version;
            if (LatestDbVersion >= 1 && SelectMeta() == 0)
            {
                Version = "1";
                CreateTable(CreCoinNames, TableName.COIN_NAMES, Version);
                

                UpdateMeta(Version);  //Set the version 1
            }



            if (!Error)
            {
                Succes = true;
            }
            return Succes;
        }


        private void UpdateMeta(string Version)
        {
            dbConnection.Open();

            using var tr = dbConnection.BeginTransaction();
            string insertSQL = "UPDATE " + TableName.SETTINGS_META + " SET VALUE  = @VERSION WHERE KEY = @KEY";

            SQLiteCommand command = new(insertSQL, dbConnection);
            try
            {
                command.Parameters.Add(new SQLiteParameter("@VERSION", Version));
                command.Parameters.Add(new SQLiteParameter("@KEY", "VERSION"));

                command.ExecuteNonQuery();
                Logging.WriteToLogInformation("De tabel " + TableName.SETTINGS_META + " is gewijzigd. (Versie " + Version + ").");
                command.Dispose();
                tr.Commit();
            }
            catch (SQLiteException ex)
            {
                Logging.WriteToLogError("Wijzigen tabel " + TableName.SETTINGS_META + " versie is misukt. (Versie " + Version + ").");
                Logging.WriteToLogError("Melding :");
                Logging.WriteToLogError(ex.Message);
                if (this.DebugMode) { Logging.WriteToLogDebug(ex.ToString()); }
                command.Dispose();
                tr.Rollback();
            }
            finally
            {
                dbConnection.Close();
            }
        }
        public int SelectMeta()  //made public so you can check the version on every application start
        {
            int SQLLiteMetaVersion = 0;

            string SQL = "SELECT VALUE FROM " + TableName.SETTINGS_META + " WHERE KEY = 'VERSION'";

            dbConnection.Open();
            Logging.WriteToLogInformation("Controle op versie van de query database.");

            SQLiteCommand command = new(SQL, dbConnection);
            try
            {
                SQLiteDataReader dr = command.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    SQLLiteMetaVersion = Int32.Parse(dr[0].ToString(), CultureInfo.InvariantCulture);
                }
                dr.Close();
            }
            catch (SQLiteException ex)
            {
                Logging.WriteToLogError("Opvragen meta versie is misukt. (Versie " + Convert.ToString(LatestDbVersion, CultureInfo.InvariantCulture) + ").");
                Logging.WriteToLogError("Melding :");
                Logging.WriteToLogError(ex.Message);
                if (DebugMode) { Logging.WriteToLogDebug(ex.ToString()); }

                Error = true;
                SQLLiteMetaVersion = 9999;
            }
            finally
            {
                command.Dispose();
                dbConnection.Close();
            }
            return SQLLiteMetaVersion;
        }
        #endregion create all the tables

        #region Insert
        public void SaveCoinNames(List<string> CoinNames)
        {
            DeleteCoinNames();  //First delete the existing coin names.

            dbConnection.Open();

            using (var tr = dbConnection.BeginTransaction())
            {
                try
                {
                    foreach (string CoinName in CoinNames)
                    {

                        string InsertSql = string.Format("insert into {0}(GUID, NAME, DATE_CREATED, CREATED_BY) ", TableName.COIN_NAMES);
                        InsertSql += "values(@GUID, @NAME, @DATE_CREATED, @CREATED_BY)";

                        SQLiteCommand command = new(InsertSql, dbConnection);

                        command.Prepare();
                        command.Parameters.Add(new SQLiteParameter("@GUID", Guid.NewGuid().ToString()));
                        command.Parameters.Add(new SQLiteParameter("NAME", CoinName));
                        //command.Parameters.Add(new SQLiteParameter("QUERY_ID", Id));
                        command.Parameters.Add(new SQLiteParameter("@DATE_CREATED", DateTime.Now));
                        command.Parameters.Add(new SQLiteParameter("@CREATED_BY", ""));  //Later when orders etc are added. Then some restrictions are needed

                        command.ExecuteNonQuery();
                        command.Dispose();                        
                    }
                    tr.Commit();
                }
                catch (SQLiteException ex)
                {
                    tr.Rollback();
                    dbConnection.Close();

                    Logging.WriteToLogError("Het invoeren van de Coin namen is mislukt.");
                    Logging.WriteToLogError("Melding :");
                    Logging.WriteToLogError(ex.Message);
                    if (this.DebugMode) { Logging.WriteToLogDebug(ex.ToString()); }
                }                           
            }
            dbConnection.Close();
        }
        private void DeleteCoinNames()
        {
            dbConnection.Open();

            using (var tr = dbConnection.BeginTransaction())
            {
                try
                {
                    string DeleteSql = string.Format("delete from {0}", TableName.COIN_NAMES);

                    SQLiteCommand command = new(DeleteSql, dbConnection);

                    command.ExecuteNonQuery();
                    command.Dispose();
                    tr.Commit();
                }
                catch (SQLiteException ex)
                {
                    Logging.WriteToLogError("Het verwijderen van de Coin namen is mislukt.");
                    Logging.WriteToLogError("Melding :");
                    Logging.WriteToLogError(ex.Message);
                    if (this.DebugMode) { Logging.WriteToLogDebug(ex.ToString()); }
                    tr.Rollback();
                    dbConnection.Close();
                }                
            }
            dbConnection.Close();
        }

        public void SaveCoinData(CoinDataAll AllCoinData)
        {
            dbConnection.Open();

            using (var tr = dbConnection.BeginTransaction())
            {
                try
                {
                    foreach (CoinData aCoin in AllCoinData.Items)
                    {
                        string InsertSql = "insert into EC_" + aCoin.Name.Replace("-", "_") + "(GUID, NAME, CURRENT_PRICE, PREVIOUS_PRICE, DIFFERENCE_PRICE, DIFFERENCE_PERCENTAGE, ";
                        InsertSql += "DIFFERENCE, SESSION_OPENPRICE, SESSION_HIGH, SESSION_LOW, OPEN, LOW, HIGH, VOLUME, DATE_CREATED) ";
                        InsertSql += "values(@GUID, @NAME, @CURRENT_PRICE, @PREVIOUS_PRICE, @DIFFERENCE_PRICE, @DIFFERENCE_PERCENTAGE, ";
                        InsertSql += "@DIFFERENCE, @SESSION_OPENPRICE, @SESSION_HIGH, @SESSION_LOW, @OPEN, @LOW, @HIGH, @VOLUME, @DATE_CREATED)";

                        SQLiteCommand command = new(InsertSql, dbConnection);

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
                    dbConnection.Close();

                    Logging.WriteToLogError("Het invoeren van de Coin data is mislukt.");
                    Logging.WriteToLogError("Melding :");
                    Logging.WriteToLogError(ex.Message);
                    if (this.DebugMode) { Logging.WriteToLogDebug(ex.ToString()); }
                }
                
            }
            dbConnection.Close();
        }

        #endregion Insert

        #region Get
        public List<string> GetCoinNames()
        {
            List<string> CoinNames = new();

            dbConnection.Open();

            using (var tr = dbConnection.BeginTransaction())
            {
                try
                {
                    string SelectSql = string.Format("select NAME from {0}", TableName.COIN_NAMES);

                    SQLiteCommand command = new(SelectSql, dbConnection);

                    SQLiteDataReader dr = command.ExecuteReader();
                    DataTable dt = new();
                    dt.Load(dr);

                    dr.Close();
                    command.Dispose();

                    if (dt.IsInitialized == true)
                    {
                        foreach (DataRow _row in dt.Rows)
                        {
                            CoinNames.Add(_row["NAME"].ToString());                            
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
                    if (this.DebugMode) { Logging.WriteToLogDebug(ex.ToString()); }
                    tr.Rollback();
                    dbConnection.Close();
                    return null;
                }                
            }
            dbConnection.Close();
            if (CoinNames.Count > 0)
            {
                return CoinNames;
            }
            else
            {
                return null;
            }
        }

        public string GetSQliteVersion()
        {
            dbConnection.Open();

            try
            {
                string SelectSql = "select sqlite_version()";

                SQLiteCommand command = new(SelectSql, dbConnection);

                
                string version = command.ExecuteScalar().ToString();

                if (!string.IsNullOrEmpty(version))
                {
                    dbConnection.Close();
                    return version;
                }
                else
                {
                    dbConnection.Close();
                    return null;
                }
            }
            catch (SQLiteException ex)
            {
                Logging.WriteToLogError("Het opvragen van de SQLite versie is mislukt.");
                Logging.WriteToLogError("Melding :");
                Logging.WriteToLogError(ex.Message);
                if (this.DebugMode) { Logging.WriteToLogDebug(ex.ToString()); }
                dbConnection.Close();
                return null;
            }
        }


        #endregion Get

        #region Maintenance
        public bool CopyDatabaseFile(string type)
        {
            string fileToCopy = DatabaseFileName;

            DateTime dateTime = DateTime.UtcNow.Date;
            string CurrentDate = dateTime.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);

            string newLocation = Path.Combine(this.DbLocation, AppSettingsDefault.BackUpFolder) + CurrentDate + "_" + AppSettingsDefault.SqlLiteDatabaseName;

            bool result = false;

            if (Directory.Exists(Path.Combine(this.DbLocation, AppSettingsDefault.BackUpFolder)))
            {
                if (File.Exists(fileToCopy))
                {
                    if (type == "StartUp")
                    {
                        File.Copy(fileToCopy, newLocation, true);  //overwrite file = true
                        Logging.WriteToLogInformation("Het kopiëren van het bestand '" + AppSettingsDefault.SqlLiteDatabaseName + "' is gereed.");
                        result = true;
                    }
                    else
                    {
                        if (File.Exists(newLocation))
                        {
                            DialogResult dialogResult = MessageBox.Show("Het bestand bestaat al. Wilt u het bestand overschrijven?", "Kopiëren bestand.", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dialogResult == DialogResult.Yes)
                            {
                                File.Copy(fileToCopy, newLocation, true);  //overwrite file = true
                                Logging.WriteToLogInformation("Het kopiëren van het bestand '" + AppSettingsDefault.SqlLiteDatabaseName + "' is gereed.");
                                result = true;
                            }
                            else if (dialogResult == DialogResult.No)
                            {
                                Logging.WriteToLogInformation("Het kopiëren van het bestand '" + AppSettingsDefault.SqlLiteDatabaseName + "' is afgebroken.");
                                Logging.WriteToLogInformation("Het bestand komt reeds voor.");
                                result = false;
                            }
                        }
                        else
                        {
                            File.Copy(fileToCopy, newLocation, false);  //overwrite file = false
                            Logging.WriteToLogInformation("Het kopiëren van het bestand '" + AppSettingsDefault.SqlLiteDatabaseName + "' is gereed.");
                            result = true;
                        }
                    }
                }
                else
                {
                    Logging.WriteToLogInformation("Het te kopiëren bestand '" + AppSettingsDefault.SqlLiteDatabaseName + "' is niet aanwezig.");
                    MessageBox.Show("Het bestand '" + AppSettingsDefault.SqlLiteDatabaseName + "' is niet aanwezig."
                               , "Fout bij kopiëren bestand.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    result = false;
                }
            }
            else
            {
                MessageBox.Show("De map '" + AppSettingsDefault.BackUpFolder + "' is niet aanwezig."
                               , "Fout bij kopiëren bestand.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                result = false;
            }
            return result;
        }
        public void CompressDatabase()
        {
            dbConnection.Open();
            SQLiteCommand command = new(dbConnection);
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
                if (this.DebugMode) { Logging.WriteToLogDebug(ex.ToString()); }
            }
            finally
            {
                command.Dispose();
                dbConnection.Close();
            }
        }
        #endregion Maintenance


        #region Dispose
        // Flag: Has Dispose already been called?
        bool disposed;

        // Instantiate a SafeHandle instance.
        readonly SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                // Free any other managed objects here.
                //
            }

            disposed = true;
        }
        #endregion Dispose
    }

    public static class TableName
    {
        public static String SETTINGS_META { get { return "SETTINGS_META"; } }
        public static String COIN_NAMES { get { return "COIN_NAMES"; } }        
    }
}
