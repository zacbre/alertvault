using AlertVault.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace AlertVault.Core.Infrastructure.Database.Repositories;

public class UserRepository(DatabaseContext context) : BaseRepository(context)
{
    public async Task<List<User>> All()
    {
        return await context.User.Include(p => p.Alerts).ToListAsync();
    }

    public async Task<User?> Get(string email)
    {
        return await (from user in context.User
                where user.Email == email
                select user)
            .Include(p => p.Alerts)
            .FirstOrDefaultAsync();
    }

    public async Task<User?> Get(int id)
    {
        return await (from user in context.User
                where user.Id == id
                select user)
            .Include(p => p.Alerts)
            .FirstOrDefaultAsync();
    }

    public async Task Add(User user)
    {
        var addedUser = await context.User.AddAsync(user);
        await Save();
        user.Id = addedUser.Entity.Id;
    }

    public async Task AddToken(UserToken token)
    {
        var addedToken = await context.UserToken.AddAsync(token);
        await Save();
        token.Id = addedToken.Entity.Id;
    }

    public async Task<UserToken?> GetToken(Guid token)
    {
        return await (from userToken in context.UserToken
                where userToken.Token == token && userToken.ExpiresAt > DateTime.UtcNow
                select userToken)
            .FirstOrDefaultAsync();
    }
}