namespace AlertVault.Core.Infrastructure.Database;

public class BaseRepository(DatabaseContext context)
{
    public async Task Save()
    {
        await context.SaveChangesAsync();
    }
}