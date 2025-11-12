using System.ComponentModel.DataAnnotations;
using AlertVault.Core.Entities.Dto;

namespace AlertVault.Core.Entities;

public class Alert : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    [MaxLength(50)]
    public string? Name { get; set; }
    
    [MaxLength(200)]
    public string? Description { get; set; }
    public Guid Uuid { get; set; } = Guid.NewGuid();
    public DateTime LastCheckUtc { get; set; }
    public TimeSpan Interval { get; set; }
    public DateTime? LastReported { get; set; }
    public AlertConfiguration? AlertConfiguration { get; set; } = new();
}

public class AlertConfiguration
{
    public List<UserCredentialTypeEnum> EnabledNotifiers { get; set; } = [];
}