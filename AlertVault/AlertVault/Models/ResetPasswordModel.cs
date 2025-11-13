namespace AlertVault.Models
{
    public class ResetPasswordModel
    {
        public string Email { get; set; } = string.Empty;
        public Guid Token { get; set; }
        public string NewPassword { get; set; } = string.Empty;
    }
}

