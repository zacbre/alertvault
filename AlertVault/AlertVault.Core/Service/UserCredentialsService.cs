using AlertVault.Core.Entities;
using AlertVault.Core.Infrastructure.Database;
using AlertVault.Core.Infrastructure.Database.Repositories;

namespace AlertVault.Core.Service;

public class UserCredentialsService(UserCredentialsRepository userCredentialsRepository)
{
    public async Task<List<UserCredentials>> GetByUserId(int userId)
    {
        return await userCredentialsRepository.GetByUserId(userId);
    }
    
    public async Task Add(UserCredentials userCredentials)
    {
        await userCredentialsRepository.Add(userCredentials);
    }
}