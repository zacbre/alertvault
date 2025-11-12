using AlertVault.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace AlertVault.Core.Infrastructure.Database.Repositories;

public class UserCredentialsRepository(DatabaseContext context) : BaseRepository(context)
{
    public async Task<List<UserCredentials>> GetByUserId(int userId)
    {
        return await (from credentials in context.UserCredential
                where credentials.UserId == userId
                select credentials)
            .ToListAsync();
    }
    
    public async Task Add(UserCredentials userCredentials)
    {
        var addedCredentials = await context.UserCredential.AddAsync(userCredentials);
        await Save();
        userCredentials.Id = addedCredentials.Entity.Id;
    }
}