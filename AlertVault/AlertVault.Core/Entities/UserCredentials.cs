using System.ComponentModel.DataAnnotations.Schema;
using AlertVault.Core.Entities.Dto;

namespace AlertVault.Core.Entities;

public class UserCredentials : BaseEntity
{
    public int UserId { get; set; }
    public User? User { get; set; }
    
    public UserCredentialTypeEnum CredentialType { get; set; }
    
    [Column(TypeName = "jsonb")]
    public CredentialHelper Credentials { get; set; }
}

public class CredentialHelper
{
    public DiscordCredentials? Discord { get; set; }
}

public class DiscordCredentials
{
    public string WebhookUrl { get; set; }
}