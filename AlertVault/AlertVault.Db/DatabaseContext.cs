using AlertVault.Core;
using AlertVault.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace AlertVault.Db;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }
    
    private void UpdateEntity()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e is { Entity: BaseEntity, State: EntityState.Modified or EntityState.Added });

        foreach (var entry in entries)
        {
            ((BaseEntity)entry.Entity).UpdatedUtc = DateTime.UtcNow;
        }
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        UpdateEntity();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new())
    {
        UpdateEntity();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override int SaveChanges()
    {
        UpdateEntity();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        UpdateEntity();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public DbSet<Alert> Alert { get; init; }
    public DbSet<AlertNotification> AlertNotifications { get; init; }
    public DbSet<User> User { get; init; }

    public DbSet<Request> Request { get; init; }
}