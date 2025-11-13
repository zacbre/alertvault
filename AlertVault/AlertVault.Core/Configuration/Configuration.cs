using CustomEnvironmentConfig;
using static System.String;

namespace AlertVault.Core.Configuration;

public class Configuration
{
    public PostgresConfiguration Postgres { get; set; } = new();
    [ConfigurationItem(Json = true)]
    public string[] Cors { get; set; } = [];
    public required RedisConnection Redis { get; set; }
}

public class RedisConnection
{
    public string Host { get; set; } = Empty;
    public int Port { get; set; }
    public string Password { get; set; } = Empty;
    public override string ToString()
    {
        return $"{Host}:{Port},ssl=False,abortConnect=False";
    }
}

public class PostgresConfiguration
{
    public string Host { get; set; } = Empty;
    public int Port { get; set; }
    public string Database { get; set; } = Empty;
    public string User { get; set; } = Empty;
    public string Password { get; set; } = Empty;
    
    public override string ToString()
    {
        return $"Host={Host};Port={Port};Database={Database};Username={User};Password={Password}";
    }
}