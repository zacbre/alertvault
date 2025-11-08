namespace AlertVault.Core.Repository;

public interface IUserRepository
{
    Task<List<User>> All();
    Task<User?> Get(string email);
    Task<User?> Get(int id);
    Task Add(User user);
}