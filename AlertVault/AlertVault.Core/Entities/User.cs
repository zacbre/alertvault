namespace AlertVault.Core.Entities;

public class User : BaseEntity
{
    public required string Email { get; set; }
    public required string Password { get; set; }

    public List<Alert> Alerts { get; set; } = new();
}