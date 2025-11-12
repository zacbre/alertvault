using AlertVault.Core.Entities;
using AlertVault.Core.Service;
using AlertVault.Models;
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

    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        return Ok(HttpContext.Items["UserToken"]);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] User user)
    {
        await userService.Add(user);
        
        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        // Get user, log them in.
        var token = await userService.Login(model.Email, model.Password);
        if (token is null)
            return Unauthorized();

        return Ok(new { token });
    }
}