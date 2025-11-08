namespace AlertVault.Core;

public class User
{
    public User()
    {
        Email = String.Empty;
        Password = String.Empty;
    }
    
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime CreatedUtc { get; set; }
    
    public List<Alert> Alerts { get; set; } = new();
}