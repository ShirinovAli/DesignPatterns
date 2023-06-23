namespace StrategyPattern.WebApp.Models
{
    public class Settings
    {
        public static string ClaimDatabaseType = "databasetype";

        public DatabaseType DatabaseType;

        public DatabaseType GetDefaultDatabaseType => DatabaseType.SqlServer;
    }
}
