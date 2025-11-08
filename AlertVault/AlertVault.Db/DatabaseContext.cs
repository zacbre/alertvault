using AlertVault.Core;
using Microsoft.EntityFrameworkCore;

namespace AlertVault.Db;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.Entity<Alert>()
        //     .HasOne(e => e.User)
        //     .WithMany(e => e.Alerts)
        //     .HasForeignKey(e => e.UserId)
        //     .HasPrincipalKey(e => e.Id);
    }


    public DbSet<Alert> Alert { get; init; }
    public DbSet<User> User { get; init; }

    public DbSet<Request> Request { get; init; }
}