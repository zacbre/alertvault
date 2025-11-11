namespace AlertVault.Core.Entities;

public class User
{
    public User()
    {
        Email = string.Empty;
        Password = string.Empty;
    }

    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime CreatedUtc { get; set; }
    public DateTime UpdatedUtc { get; set; }

    public List<Alert> Alerts { get; set; } = new();
}