using AlertVault.Core.Entities;
using AlertVault.Core.Infrastructure.Database;
using AlertVault.Core.Service;
using AlertVault.Core.Validators;

namespace AlertVault.Test;

public abstract class BaseTest(Fixture fixture) : IClassFixture<CustomWebApplicationFactory>
{
    public UserRepository UserRepository => fixture.UserRepository;
    public AlertRepository AlertRepository => fixture.AlertRepository;
    public AlertNotificationQueueRepository AlertNotificationQueueRepository => fixture.AlertNotificationQueueRepository;
    public DatabaseContext DatabaseContext => fixture.Context;
    
    public UserService UserService => new(UserRepository);
    public AlertService AlertService => new(AlertRepository, AlertNotificationQueueRepository, new AlertValidator());
    
    internal async Task<User> CreateUser() => await UserService.Add(new User{ Email = "test@test.com", Password = "1234"}); 
    
}