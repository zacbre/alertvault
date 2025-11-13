using Microsoft.EntityFrameworkCore;

namespace AlertVault.Core.Entities
{
    [Index(nameof(Token), IsUnique = true)]
    public class UserToken : BaseEntity
    {
        public int UserId { get; set; }
        public Guid Token { get; set; } = Guid.NewGuid();
        public DateTime? ExpiresAt { get; set; }
    }
}

