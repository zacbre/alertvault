using AlertVault.Core.Entities;
using FluentValidation;

namespace AlertVault.Core.Validators;

public class AlertValidator : AbstractValidator<Alert>
{
    public AlertValidator()
    {
        RuleFor(a => a.Interval)
            .Must(i => i.TotalMinutes >= 1)
            .WithMessage("Interval must be at least 1 minute.");
    }
}