namespace AlertVault.Core.Entities;

public class ResetPasswordRequest : BaseEntity
{
    public int UserId { get; set; }
    public Guid Token { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsUsed { get; set; }
}