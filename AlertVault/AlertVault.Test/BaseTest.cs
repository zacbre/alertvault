using AlertVault.Core;
using AlertVault.Core.Entities;
using AlertVault.Core.Service;
using AlertVault.Core.Validators;
using AlertVault.Db;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AlertVault.Test;

public abstract class BaseTest(Fixture fixture) : IClassFixture<CustomWebApplicationFactory>
{
    public UserRepository UserRepository => fixture.UserRepository;
    public AlertRepository AlertRepository => fixture.AlertRepository;
    public DatabaseContext DatabaseContext => fixture.Context;
    
    public UserService UserService => new UserService(UserRepository);
    public AlertService AlertService => new AlertService(AlertRepository, new AlertValidator());
    
    internal async Task<User> CreateUser() => await UserService.Add(new User{ Email = "test@test.com", Password = "1234"}); 
    
}