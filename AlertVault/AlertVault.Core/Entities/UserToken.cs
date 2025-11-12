namespace AlertVault.Core.Entities
{
    public class UserToken : BaseEntity
    {
        public int UserId { get; set; }
        public Guid Token { get; set; } = Guid.NewGuid();
        public DateTime? ExpiresAt { get; set; }
    }
}

