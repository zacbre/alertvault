using AlertVault.Core;
using AlertVault.Core.Entities;
using AlertVault.Core.Repository;
using AlertVault.Core.Service;
using AlertVault.Db;
using Microsoft.AspNetCore.Mvc;

namespace AlertVault.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(UserService userService) : ControllerBase
{
    [HttpGet("all")]
    public async Task<IActionResult> All()
    {
        return Ok(await userService.All());
    }
    
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Hello World");
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] User user)
    {
        await userService.Add(user);
        
        return Ok(user);
    }
}