using AlertVault.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace AlertVault.Core.Infrastructure.Database.Repositories;

public class UserAgentRepository(DatabaseContext context) : BaseRepository(context)
{
    public async Task<UserAgent?> GetByUserAgentString(string? userAgentString) => await context.UserAgent.FirstOrDefaultAsync(ua => ua.UserAgentString == userAgentString);

    public async Task<UserAgent> Add(string userAgent)
    {
        var newUserAgent = new UserAgent
        {
            UserAgentString = userAgent
        };
        
        var addedUserAgent = await context.UserAgent.AddAsync(newUserAgent);
        await Save();
        return addedUserAgent.Entity;
    }
}