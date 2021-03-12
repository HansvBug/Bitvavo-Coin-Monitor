using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Data.SQLite;
using System.Data;
using System.IO;
using System.Security.Permissions;
using System.Windows.Forms;

namespace CM
{
    public class SQliteExport : SQliteDatabaseConnection
    {
        public bool DebugMode { get; set; }
        
        private string ExportFileLocation { get; set; }        
        private string CsvSeparator { get; set; }
        
        
        TableInfo TblInfo;
        readonly TablesInfo AllTblInfo;

        public SQliteExport()
        {
            // Default constructor
        }
        public SQliteExport(string FileLocation)
        {
            ExportFileLocation = FileLocation;

            TblInfo = new();
            AllTblInfo = new();
            CsvSeparator = ";";  // TODO make optional
        }

        public void ExportToCsv(bool ActiveTablesOnly)
        {
            string SQL;
            if (ActiveTablesOnly)
            {
                SQL = "select NAME from " + TableName.COIN_NAMES + " order by NAME";
                Logging.WriteToLogInformation("Selecteer alle in gebruik zijnde 'Coin' tabellen.");
            }
            else
            {
                SQL = "select NAME from sqlite_master where type = 'table' and NAME like 'EC%' order by NAME";
                Logging.WriteToLogInformation("Selecteer alle 'Coin' tabellen.");
            }

            dbConnection.Open();           

            SQLiteCommand command = new(SQL, dbConnection);
            try
            {                
                SQLiteDataReader dr = command.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {                    
                    while (dr.Read())
                    {
                        TblInfo = new();
                        //Get the table name
                        string TableName = dr.GetValue(0).ToString();


                        TblInfo.TableName = TableName.Replace("EC_", "");
                        if (ActiveTablesOnly)
                        {
                            //get the columnames
                            GetColumnNames("EC_" + TableName.Replace("-", "_"));                            
                        }
                        else
                        {
                            GetColumnNames(TableName.Replace("-", "_"));
                        }

                        AllTblInfo.Items.Add(TblInfo);
                        TblInfo = null;
                    }                    
                }
                dr.Close();

                ExportAllActiveTablesToCSV();                
            }
            catch (SQLiteException ex)
            {
                Logging.WriteToLogError("Opvragen alle 'coin' tabellen is mislukt.");
                Logging.WriteToLogError("Melding :");
                Logging.WriteToLogError(ex.Message);
                if (DebugMode) { Logging.WriteToLogDebug(ex.ToString()); }
            }
            finally
            {
                command.Dispose();
                dbConnection.Close();
            }
        }

        private void GetColumnNames(string TableName)
        {
            //var cmd = new SQLiteCommand("PRAGMA table_info('TableName');", dbConnection);            
            var cmd = new SQLiteCommand(string.Format("PRAGMA table_info('{0}');", TableName), dbConnection);

            try
            {
                var dr = cmd.ExecuteReader();
                while (dr.Read())//loop through the various columns and their info
                {
                    var value = dr.GetValue(1);//column 1 from the result contains the column names
                    TblInfo.ColumnNames.Add(value.ToString());
                }
                dr.Close();                
            }
            catch (SQLiteException ex)
            {
                Logging.WriteToLogError("Opvragen alle kolomnamen van de tabel '" + TableName + "' is mislukt.");
                Logging.WriteToLogError("Melding :");
                Logging.WriteToLogError(ex.Message);
                if (DebugMode) { Logging.WriteToLogDebug(ex.ToString()); }
            }            
        }

        private void ExportAllActiveTablesToCSV()
        {
            if (AllTblInfo != null)
            {
                foreach (TableInfo TblInfo in AllTblInfo.Items)
                {
                    if (TblInfo != null)
                    {

                        string FileName = Path.Combine(ExportFileLocation, TblInfo.TableName.Replace("-", "_") + ".csv");

                        if (File.Exists(FileName))
                        {
                            DialogResult dialogResult = MessageBox.Show("Het bestand bestaat al. " + Environment.NewLine +
                                FileName + Environment.NewLine + Environment.NewLine +
                                "Wilt u het over schrijven?", "Overschrijven", MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                            {
                                WriteCsvFile(FileName, TblInfo);
                            }
                            else if (dialogResult == DialogResult.No)
                            {
                                // Do Nothing
                            }
                        }
                        else
                        {
                            WriteCsvFile(FileName, TblInfo);
                        }
                    }
                }
            }
        }

        private void WriteCsvFile(string FileName, TableInfo TblInfo)
        {
            try
            {
                using StreamWriter sw = new(new FileStream(FileName, FileMode.Create));
                string aLine = string.Empty;

                foreach (string columnName in TblInfo.ColumnNames)  // Headers
                {
                    //header 
                    int i = 0;
                    aLine += @"""" + columnName + @"""";
                    if (i < TblInfo.ColumnNames.Count - 1)
                    {
                        aLine += this.CsvSeparator;
                        i++;
                    }
                }
                sw.WriteLine(aLine);  //Write the header
                aLine = string.Empty;

                // Loop through the data...
                DataTable Dt = GetTheData(TblInfo.TableName);

                if (Dt != null)
                {
                    foreach (DataRow dr in Dt.Rows)
                    {
                        string FieldStartEnd = string.Empty;
                        for (int j = 0; j < TblInfo.ColumnNames.Count; ++j)
                        {
                            FieldStartEnd = @"""";

                            aLine += FieldStartEnd + dr[j].ToString() + FieldStartEnd;
                            if (j < Dt.Columns.Count - 1)
                                aLine += this.CsvSeparator;
                        }
                        sw.WriteLine(aLine);  //Write the data
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
                if (DebugMode) { Logging.WriteToLogError(ex.ToString()); }
            }
        }

        private DataTable GetTheData(string TableName)
        {
            try
            {
                Logging.WriteToLogInformation("Selecteer alle in gebruik zijnde 'Coin' tabellen.");

                DataTable Dt = new();
                string SQL = "select * from EC_" + TableName.Replace("-","_");  

                SQLiteCommand command = new(SQL, dbConnection);
                Dt.Load(command.ExecuteReader());

                if (Dt != null)
                {
                    return Dt;
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
                if (DebugMode) { Logging.WriteToLogError(ex.ToString()); }

                return null;
            }
        }

        public class TablesInfo
        {
            public List<TableInfo> Items { get; } = new List<TableInfo>();
        }
        public class TableInfo
        {         
            public string TableName { get; set; }
            public List<string> ColumnNames = new();         
        }
    }
}
