using AlertVault.Core.Configuration;
using CustomEnvironmentConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AlertVault.Db;

public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext(string[] args)
    {
        var config = ConfigurationParser.Parse<Configuration>(fileName: "local.env");
        
        var connectionString = $"Host={config.Postgres.Host};Port={config.Postgres.Port};Database={config.Postgres.Database};Username={config.Postgres.User};Password={config.Postgres.Password}";
        
        var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new DatabaseContext(optionsBuilder.Options);
    }
}