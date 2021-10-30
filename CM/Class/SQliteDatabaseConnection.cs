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
            this.LoadSettings();
            this.DbLocation = this.JsonObjSettings.AppParam[0].DatabaseLocation;
            this.DatabaseFileName = Path.Combine(this.DbLocation, AppSettingsDefault.SqlLiteDatabaseName);
            this.dbConnection = new SQLiteConnection("Data Source=" + this.DatabaseFileName);
        }

        private void LoadSettings()
        {
            using SettingsManager set = new();
            set.LoadSettings();
            this.JsonObjSettings = set.JsonObjSettings;
        }
    }
}
