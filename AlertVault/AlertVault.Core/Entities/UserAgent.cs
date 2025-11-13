using Microsoft.EntityFrameworkCore;

namespace AlertVault.Core.Entities;

[Index(nameof(UserAgentString), IsUnique = true)]
public class UserAgent : BaseEntity
{
    public required string UserAgentString { get; set; }
}