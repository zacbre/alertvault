using System.ComponentModel.DataAnnotations;

namespace AlertVault.Core.Entities;

public class User : BaseEntity
{
    [MaxLength(200)]
    public required string Email { get; set; }
    
    [MaxLength(200)]
    public required string Password { get; set; }

    public ICollection<Alert> Alerts { get; set; } = [];
}