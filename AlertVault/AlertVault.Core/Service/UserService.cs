using AlertVault.Core.Entities;
using AlertVault.Core.Infrastructure.Database;
using AlertVault.Core.Infrastructure.Database.Repositories;

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
}