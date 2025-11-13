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
        var query = from user in context.User
                where user.Email == email
                select user;
            
        return await query
            .Include(p => p.Alerts)
            .FirstOrDefaultAsync();
    }

    public async Task<User?> Get(int id)
    {
        var query = from user in context.User
            where user.Id == id
            select user;
        
        return await query
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
        var query = from userToken in context.UserToken
                where userToken.Token == token && userToken.ExpiresAt > DateTime.UtcNow
                select userToken;
        
        return await query.FirstOrDefaultAsync();
    }
    
    public async Task<ResetPasswordRequest?> GetResetPasswordRequest(Guid token)
    {
        var query = (from request in context.ResetPasswordRequest
            where request.Token == token &&
                  request.ExpiresAt > DateTime.UtcNow &&
                  !request.IsUsed
            select request);
        
        return await query.FirstOrDefaultAsync();
    }

    public async Task AddResetPasswordRequest(ResetPasswordRequest resetPasswordRequest)
    {
        var addedRequest = await context.ResetPasswordRequest.AddAsync(resetPasswordRequest);
        await Save();
        resetPasswordRequest.Id = addedRequest.Entity.Id;
    }
}