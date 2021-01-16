using System;
using System.Globalization;

namespace CM
{
    public static class AppSettingsDefault
    {
        public const string ApplicationName = "CM";
        public const string ApplicationVersion = "0.1.0.0";
        public static string ApplicationBuildDate { get { return DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture); } }

        public const string SystemMenu = "2021";  //For the systemmenu line                
        public const string Copyright = "2021-2021";  //started 27 December 2020

        public const int UpdateVersion = 1;

        public const string LogFileName = "CM.log";
        public const string ConfigFile = "CM.cfg";
        public const string SettingsFolder = "Settings\\";
        public const string DatabaseFolder = "Database\\";
        public const string BackUpFolder = "BackUp\\";
        public const string ExportTempFolder = "Export_Tmp\\";
        public const string SqlLiteDatabaseName = "CM.db";

        // not good...
        public const string stringSleutel = "$fvs-sdUo(fwa|fk(d)as(9dvQ=+<P[gth<}]f" + aStringSleutel;
        public const string stringSleutel1 = "$fvs-sdo1(fwa|fk(d)asY1(9dvQ=+<P[gth<}]f" + aStringSleutel1;
        public const string aStringSleutel = "gt#vt&wsUs^cvwsr)0IngsrgKey#tgrj+suci@asd?/vdk";
        public const string aStringSleutel1 = "gt#v1t&wSs^cvwsr)0ingsrgKey#tgrj+1suci@asd?/vdk";
    }
}
