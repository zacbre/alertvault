namespace AlertVault.Core.Entities;

public class Alert : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; }
    public Guid Uuid { get; set; } = Guid.NewGuid();
    public DateTime LastCheckUtc { get; set; }
    public TimeSpan Interval { get; set; }
    public DateTime? LastReported { get; set; }
    public List<Request> Requests { get; set; } = [];
}