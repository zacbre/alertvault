using AlertVault.Core.Infrastructure.Database;
using AlertVault.Core.Infrastructure.Database.Repositories;

namespace AlertVault.Core.Service;

public class UserCredentialsService(UserCredentialsRepository userCredentialsRepository)
{
    public async Task<Entities.UserCredentials?> GetByUserId(int userId)
    {
        return await userCredentialsRepository.GetByUserId(userId);
    }
    
    public async Task Add(Entities.UserCredentials userCredentials)
    {
        await userCredentialsRepository.Add(userCredentials);
    }
}