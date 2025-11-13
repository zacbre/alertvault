using System.ComponentModel.DataAnnotations.Schema;
using AlertVault.Core.Entities.Dto;
using Microsoft.EntityFrameworkCore;

namespace AlertVault.Core.Entities;

[Index(nameof(UserId), nameof(CredentialType), IsUnique = true)]
public class UserCredentials : BaseEntity
{
    public int UserId { get; set; }
    
    public UserCredentialTypeEnum CredentialType { get; set; }

    [Column(TypeName = "jsonb")] 
    public CredentialHelper Credentials { get; set; } = null!;
    
    public User User { get; set; } = null!;
}

public class CredentialHelper
{
    public DiscordWebhookCredentials? DiscordWebhook { get; set; }
}

public class DiscordWebhookCredentials
{
    public required string Url { get; set; }
}