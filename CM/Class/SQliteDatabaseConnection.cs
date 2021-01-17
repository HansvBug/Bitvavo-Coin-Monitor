using System.Data.SQLite;
using System.IO;

namespace CM
{
    public class SQliteDatabaseConnection
    {
        protected SQLiteConnection dbConnection;
        protected string DatabaseFileName { get; set; }
        protected string DbLocation { get; set; }
        private dynamic JsonObjSettings { get; set; }


        public SQliteDatabaseConnection()
        {
            LoadSettings();
            DbLocation = JsonObjSettings.AppParam[0].DatabaseLocation;
            DatabaseFileName = Path.Combine(DbLocation, AppSettingsDefault.SqlLiteDatabaseName);
            dbConnection = new SQLiteConnection("Data Source=" + DatabaseFileName);
        }

        private void LoadSettings()
        {
            using SettingsManager Set = new();
            Set.LoadSettings();
            JsonObjSettings = Set.JsonObjSettings;
        }
    }
}
