namespace AlertVault.Db;

public class BaseRepository(DatabaseContext context)
{
    public async Task Save()
    {
        await context.SaveChangesAsync();
    }
}