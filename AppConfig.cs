using System.Configuration;

namespace ThoughtKeeper
{
    public class AppConfig
    {
        public static readonly string DbConnectionString = ConfigurationManager.ConnectionStrings["ThoughtKeeperDB"].ConnectionString;
    }
}
