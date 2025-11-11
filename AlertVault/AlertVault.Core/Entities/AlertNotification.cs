namespace AlertVault.Core.Entities;

public class AlertNotification : BaseEntity
{
    public int AlertId { get; set; }
    public Alert? Alert { get; set; }
}