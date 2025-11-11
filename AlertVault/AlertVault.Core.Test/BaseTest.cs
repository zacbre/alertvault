using AlertVault.Core.Entities;
using AlertVault.Core.Infrastructure.Database;
using AlertVault.Core.Service;
using AlertVault.Core.Validators;

namespace AlertVault.Core.Test;

public class BaseTest(Fixture fixture)
{
    public DatabaseContext DatabaseContext => fixture.Context;
    
    // Repositories
    public AlertRepository AlertRepository => fixture.AlertRepository;
    public AlertNotificationQueueRepository AlertNotificationQueueRepository => fixture.AlertNotificationQueueRepository;
    public UserCredentialsRepository UserCredentialsRepository => fixture.UserCredentialsRepository;
    public UserRepository UserRepository => fixture.UserRepository;
    
    // Services 
    public AlertService AlertService => new(AlertRepository, AlertNotificationQueueRepository, new AlertValidator());
    public AlertNotificationQueueService AlertNotificationQueueService => new(AlertNotificationQueueRepository);
    public UserCredentialsService UserCredentialsService => new (UserCredentialsRepository);
    public UserService UserService => new(UserRepository);
    
    internal async Task<User> CreateUser() => await UserService.Add(new User{ Email = "test@test.com", Password = "1234"}); 
    
}