using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AlertVault.Core.Entities;

[Index(nameof(Email), IsUnique = true)]
public class User : BaseEntity
{
    [MaxLength(200)]
    public required string Email { get; set; }
    
    [MaxLength(200)]
    public required string Password { get; set; }

    public List<Alert> Alerts { get; set; } = [];
}