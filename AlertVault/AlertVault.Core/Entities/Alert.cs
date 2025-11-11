namespace AlertVault.Core.Entities;

public class Alert
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public Guid Uuid { get; set; } = Guid.NewGuid();
    public DateTime LastCheckUtc { get; set; }
    public TimeSpan Interval { get; set; }
    public DateTime CreatedUtc { get; set; }
    public DateTime UpdatedUtc { get; set; }
    public List<Request> Requests { get; set; } = [];
}