using AlertVault.Core.Entities;
using AlertVault.Core.Service;
using AlertVault.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace AlertVault.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(UserService userService) : BaseController(userService)
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
        var token = await userService.Login(model.Email, model.Password);
        if (token is null)
            return Unauthorized();

        return Ok(new { token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        var existingUser = await userService.Get(user.Email);
        if (existingUser is not null)
            return BadRequest("User with this email already exists.");

        await userService.Add(user);
        return Ok(user);
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
    {
        var user = await userService.Get(model.Email);
        if (user is null)
            return NoContent();

        await userService.AddResetPasswordRequest(user);
        
        return NoContent();
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
    {
        var resetPasswordRequest = await userService.GetResetPasswordRequest(model.Token);
        if (resetPasswordRequest is null)
        {
            return BadRequest("Invalid reset password request.");
        }
        
        var user = await userService.GetById(resetPasswordRequest.UserId);
        if (user is null)
            return BadRequest("Invalid token or user.");
        
        await userService.UpdatePassword(user, model.NewPassword);
        
        return NoContent();
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
    {
        var user = await GetCurrentUserAsync();
        
        if (user is null || !BCrypt.Net.BCrypt.Verify(model.OldPassword, user.Password))
            return BadRequest("Invalid current password.");
        
        await userService.UpdatePassword(user, model.NewPassword);
        
        return NoContent();
    }
}