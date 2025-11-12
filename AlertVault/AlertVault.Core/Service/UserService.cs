using AlertVault.Core.Entities;
using AlertVault.Core.Infrastructure.Database;
using AlertVault.Core.Infrastructure.Database.Repositories;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AlertVault.Core.Service;

public class UserService(UserRepository userRepository)
{
    public async Task<List<User>> All() => await userRepository.All();
    public async Task<User?> Get(string email) => await userRepository.Get(email);

    public async Task<User> Add(User user)
    {
        user.CreatedUtc = DateTime.UtcNow;
        await userRepository.Add(user);
        return user;
    }

    public async Task<Guid?> Login(string email, string password)
    {
        var user = await Get(email);
        if (user is null)
            return null;

        if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
            return null;
        
        var userToken = new UserToken
        {
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };
        
        await userRepository.AddToken(userToken);
        
        return userToken.Token;
    }

    public async Task<UserToken?> ValidateToken(Guid token) => await userRepository.GetToken(token);
}