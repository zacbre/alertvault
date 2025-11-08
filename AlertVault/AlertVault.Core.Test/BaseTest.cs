using AlertVault.Core;
using AlertVault.Core.Service;
using AlertVault.Core.Test;
using AlertVault.Db;

public class BaseTest
{
    protected readonly Fixture Fixture;

    public BaseTest(Fixture fixture)
    {
        Fixture = fixture;
    }
    
    public UserRepository UserRepository => Fixture.UserRepository;
    public AlertRepository AlertRepository => Fixture.AlertRepository;
    public DatabaseContext DatabaseContext => Fixture.Context;
    
    public UserService UserService => new UserService(UserRepository);
    public AlertService AlertService => new AlertService(AlertRepository);
    
    internal async Task<User> CreateUser() => await UserService.Add(new User{ Email = "test@test.com", Password = "1234"}); 
    
}