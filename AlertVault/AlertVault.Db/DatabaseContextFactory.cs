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
        
        var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
        optionsBuilder.UseNpgsql(config.Postgres.ToString());

        return new DatabaseContext(optionsBuilder.Options);
    }
}