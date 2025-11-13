using AlertVault.Core.Entities;
using FluentValidation;

namespace AlertVault.Core.Validators;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(a => a.Email)
            .Must(i => !string.IsNullOrWhiteSpace(i))
            .WithMessage("Email must not be empty.");
        
        RuleFor(a => a.Password)
            .Must(i => !string.IsNullOrWhiteSpace(i))
            .WithMessage("Password must not be empty.");
    }
}