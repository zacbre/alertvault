using AlertVault.Core;
using AlertVault.Core.Entities;
using AlertVault.Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace AlertVault.Db;

public class UserRepository(DatabaseContext context) : BaseRepository(context)
{
    public async Task<List<User>> All() => await context.User.Include(p => p.Alerts).ToListAsync();

    public async Task<User?> Get(string email) =>
        await (from user in context.User
                where user.Email == email
                select user)
            .Include(p => p.Alerts)
            .FirstOrDefaultAsync();

    public async Task<User?> Get(int id) =>
        await (from user in context.User
                where user.Id == id
                select user)
            .Include(p => p.Alerts)
            .FirstOrDefaultAsync();

    public async Task Add(User user)
    {
        var addedUser = await context.User.AddAsync(user);
        await Save();
        user.Id = addedUser.Entity.Id;
    }
}