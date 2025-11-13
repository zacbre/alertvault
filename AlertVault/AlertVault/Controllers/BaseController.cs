using AlertVault.Core.Entities;
using AlertVault.Core.Infrastructure.Database.Repositories;
using AlertVault.Core.Service;
using Microsoft.AspNetCore.Mvc;

namespace AlertVault.Controllers;

public class BaseController(UserService userService) : ControllerBase
{
    protected async Task<User?> GetCurrentUserAsync()
    {
        if (HttpContext.Items["UserToken"] is not UserToken token || token.ExpiresAt < DateTimeOffset.Now)
        {
            return null;
        }

        return await userService.GetById(token.UserId);
    } 
}