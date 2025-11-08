using CustomEnvironmentConfig;
using static System.String;

namespace AlertVault.Core.Configuration;

public class Configuration
{
    public PostgresConfiguration Postgres { get; set; } = new PostgresConfiguration();
    [ConfigurationItem(Json = true)]
    public string[] Cors { get; set; } = [];
}

public class PostgresConfiguration
{
    public string Host { get; set; } = Empty;
    public int Port { get; set; }
    public string Database { get; set; } = Empty;
    public string User { get; set; } = Empty;
    public string Password { get; set; } = Empty;
}