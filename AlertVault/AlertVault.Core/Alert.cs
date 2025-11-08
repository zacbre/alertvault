namespace AlertVault.Core;

public class Alert
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public string Uuid { get; set; } = string.Empty;
    public DateTime LastCheckUtc { get; set; }
    public TimeSpan Interval { get; set; }
    public DateTime CreatedUtc { get; set; }
}