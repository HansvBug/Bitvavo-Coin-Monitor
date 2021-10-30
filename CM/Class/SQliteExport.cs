namespace CM
{
    using System;
    using System.Collections.Generic;
    using System.Data.SQLite;
    using System.Data;
    using System.IO;
    using System.Windows.Forms;

    /// <summary>
    /// Export data.
    /// </summary>
    public class SQliteExport : SQliteDatabaseConnection
    {
        private string ExportFileLocation { get; set; }

        private string CsvSeparator { get; set; }

        TableInfo TblInfo;
        readonly TablesInfo AllTblInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="SQliteExport"/> class.
        /// </summary>
        public SQliteExport()
        {
            // Default constructor
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SQliteExport"/> class.
        /// </summary>
        /// <param name="fileLocation">The location of the export file.</param>
        public SQliteExport(string fileLocation)
        {
            this.ExportFileLocation = fileLocation;

            this.TblInfo = new();
            this.AllTblInfo = new();
            this.CsvSeparator = ";";  // TODO make optional
        }

        /// <summary>
        /// Export to CSV format.
        /// </summary>
        /// <param name="activeTablesOnly">Export only the active tables.</param>
        public void ExportToCsv(bool activeTablesOnly)
        {
            string selectSql;
            if (activeTablesOnly)
            {
                selectSql = "select NAME from " + TableName.COIN_NAMES + " order by NAME";
                Logging.WriteToLogInformation("Selecteer alle in gebruik zijnde 'Coin' tabellen.");
            }
            else
            {
                selectSql = "select NAME from sqlite_master where type = 'table' and NAME like 'EC%' order by NAME";
                Logging.WriteToLogInformation("Selecteer alle 'Coin' tabellen.");
            }

            this.dbConnection.Open();

            SQLiteCommand command = new(selectSql, this.dbConnection);
            try
            {
                SQLiteDataReader dr = command.ExecuteReader();
                dr.Read();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        this.TblInfo = new();

                        string tableName = dr.GetValue(0).ToString(); // Get the table name

                        this.TblInfo.TableName = tableName.Replace("EC_", string.Empty);
                        if (activeTablesOnly)
                        {
                            this.GetColumnNames("EC_" + tableName.Replace("-", "_")); // Get the column names
                        }
                        else
                        {
                            this.GetColumnNames(tableName.Replace("-", "_"));
                        }

                        this.AllTblInfo.Items.Add(this.TblInfo);
                        this.TblInfo = null;
                    }
                }

                dr.Close();

                this.ExportAllActiveTablesToCSV();
            }
            catch (SQLiteException ex)
            {
                Logging.WriteToLogError("Opvragen alle 'coin' tabellen is mislukt.");
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

        private void GetColumnNames(string tableName)
        {
            // var cmd = new SQLiteCommand("PRAGMA table_info('TableName');", dbConnection);
            var cmd = new SQLiteCommand(string.Format("PRAGMA table_info('{0}');", tableName), this.dbConnection);

            try
            {
                var dr = cmd.ExecuteReader();

                // Loop through the various columns and their info.
                while (dr.Read())
                {
                    var value = dr.GetValue(1); // Column 1 from the result contains the column names.
                    this.TblInfo.ColumnNames.Add(value.ToString());
                }

                dr.Close();
            }
            catch (SQLiteException ex)
            {
                Logging.WriteToLogError("Opvragen alle kolomnamen van de tabel '" + tableName + "' is mislukt.");
                Logging.WriteToLogError("Melding :");
                Logging.WriteToLogError(ex.Message);
                if (CmDebugMode.DebugMode)
                {
                    Logging.WriteToLogDebug(ex.ToString());
                }
            }
        }

        private void ExportAllActiveTablesToCSV()
        {
            if (this.AllTblInfo != null)
            {
                foreach (TableInfo TblInfo in this.AllTblInfo.Items)
                {
                    if (TblInfo != null)
                    {
                        string fileName = Path.Combine(this.ExportFileLocation, TblInfo.TableName.Replace("-", "_") + ".csv");

                        if (File.Exists(fileName))
                        {
                            DialogResult dialogResult = MessageBox.Show(
                                "Het bestand bestaat al. " + Environment.NewLine +
                                fileName + Environment.NewLine + Environment.NewLine +
                                "Wilt u het over schrijven?", "Overschrijven",
                                MessageBoxButtons.YesNo);

                            if (dialogResult == DialogResult.Yes)
                            {
                                this.WriteCsvFile(fileName, TblInfo);
                            }
                            else if (dialogResult == DialogResult.No)
                            {
                                // Do Nothing
                            }
                        }
                        else
                        {
                            this.WriteCsvFile(fileName, TblInfo);
                        }
                    }
                }
            }
        }

        private void WriteCsvFile(string fileName, TableInfo tblInfo)
        {
            try
            {
                using StreamWriter sw = new(new FileStream(fileName, FileMode.Create));
                string aLine = string.Empty;

                foreach (string columnName in tblInfo.ColumnNames)
                {
                    // Header
                    int i = 0;
                    aLine += @"""" + columnName + @"""";
                    if (i < tblInfo.ColumnNames.Count - 1)
                    {
                        aLine += this.CsvSeparator;
                        i++;
                    }
                }

                sw.WriteLine(aLine);  // Write the header
                aLine = string.Empty;

                // Loop through the data...
                DataTable dt = this.GetTheData(tblInfo.TableName);

                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string fieldStartEnd = string.Empty;
                        for (int j = 0; j < tblInfo.ColumnNames.Count; ++j)
                        {
                            fieldStartEnd = @"""";

                            aLine += fieldStartEnd + dr[j].ToString() + fieldStartEnd;
                            if (j < dt.Columns.Count - 1)
                            {
                                aLine += this.CsvSeparator;
                            }
                        }

                        sw.WriteLine(aLine);  // Write the data
                        aLine = string.Empty;
                    }
                }

                sw.Flush();
            }
            catch (IOException ex)
            {
                Logging.WriteToLogError("Onverwachte fout bij het exporteren van de tabellen.");
                Logging.WriteToLogError("Melding:");
                Logging.WriteToLogError(ex.Message);
                if (CmDebugMode.DebugMode)
                {
                    Logging.WriteToLogDebug(ex.ToString());
                }
            }
        }

        private DataTable GetTheData(string tableName)
        {
            try
            {
                Logging.WriteToLogInformation("Selecteer alle in gebruik zijnde 'Coin' tabellen.");

                DataTable dt = new();
                string selectSql = "select * from EC_" + tableName.Replace("-","_");

                SQLiteCommand command = new(selectSql, this.dbConnection);
                dt.Load(command.ExecuteReader());

                if (dt != null)
                {
                    return dt;
                }
                else
                {
                    return null;
                }
            }
            catch (SQLiteException ex)
            {
                Logging.WriteToLogError("Onverwachte fout bi het ophalen van alle in gebruik zijnde 'coin' tabellen.");
                Logging.WriteToLogError("Melding:");
                Logging.WriteToLogError(ex.Message);
                if (CmDebugMode.DebugMode)
                {
                    Logging.WriteToLogDebug(ex.ToString());
                }

                return null;
            }
        }

        /// <summary>
        /// Holds references of TableInfo.
        /// </summary>
        public class TablesInfo
        {
            /// <summary>
            /// Gets reffrences of TableInfo.
            /// </summary>
            public List<TableInfo> Items { get; } = new List<TableInfo>();
        }

        /// <summary>
        /// Holds the table name and the column names of the table.
        /// </summary>
        public class TableInfo
        {
            /// <summary>
            /// Gets or sets the table name (= coin name).
            /// </summary>
            public string TableName { get; set; }

            /// <summary>
            /// Gets a list with the column names.
            /// </summary>
            public List<string> ColumnNames = new();
        }
    }
}
