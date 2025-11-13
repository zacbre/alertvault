using System.Runtime.InteropServices;
using AlertVault.Core.Entities;
using AlertVault.Core.Entities.Dto;
using AlertVault.Core.Infrastructure.Database;
using AlertVault.Core.Infrastructure.Database.Repositories;
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
    public UserAgentRepository UserAgentRepository => fixture.UserAgentRepository;
    
    // Services 
    public AlertService AlertService => new(AlertRepository, AlertNotificationQueueRepository, UserAgentRepository, new AlertValidator());
    public AlertNotificationQueueService AlertNotificationQueueService => new(AlertNotificationQueueRepository);
    public UserCredentialsService UserCredentialsService => new (UserCredentialsRepository);
    public UserService UserService => new(UserRepository, new UserValidator());
    
    protected async Task<User?> CreateUser() => (await UserService.Add(new User{ Email = RandomEmail, Password = "password"})).Unwrap();
    
    private string RandomEmail => $"{Guid.NewGuid()}@test.com";
}