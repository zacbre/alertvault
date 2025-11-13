using AlertVault.Core.Entities;
using AlertVault.Core.Entities.Dto;
using AlertVault.Core.Infrastructure.Database;
using AlertVault.Core.Infrastructure.Database.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AlertVault.Core.Service;

public class UserService(UserRepository userRepository, IValidator<User> validator)
{
    public async Task<List<User>> All() => await userRepository.All();
    public async Task<User?> Get(string email) => await userRepository.Get(email);
    public async Task<User?> GetById(int id) => await userRepository.Get(id);

    public async Task<Result<User?>> Add(User user)
    {
        var validationResult = await validator.ValidateAsync(user);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            return Result<User?>.Failure(errors);
        }
        
        user.CreatedUtc = DateTime.UtcNow;
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        
        await userRepository.Add(user);
        return user;
    }

    public async Task Save() => await userRepository.Save();

    public async Task<Guid?> Login(string email, string password)
    {
        var user = await Get(email);
        if (user is null)
            return null;

        if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
            return null;
        
        var userToken = new UserToken
        {
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };
        
        await userRepository.AddToken(userToken);
        
        return userToken.Token;
    }

    public async Task UpdatePassword(User user, string password)
    {
        user.Password = BCrypt.Net.BCrypt.HashPassword(password);
        await userRepository.Save();
    }

    public async Task<UserToken?> ValidateToken(Guid token) => await userRepository.GetToken(token);
    
    public async Task<ResetPasswordRequest?> GetResetPasswordRequest(Guid token) => await userRepository.GetResetPasswordRequest(token);

    public async Task AddResetPasswordRequest(User user)
    {
        var resetPasswordRequest = new ResetPasswordRequest
        {
            UserId = user.Id,
            Token = Guid.NewGuid(),
            CreatedUtc = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddHours(24)
        };
        
        await userRepository.AddResetPasswordRequest(resetPasswordRequest);
    }
}