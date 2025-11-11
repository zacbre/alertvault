namespace AlertVault.Core.Entities;

public abstract class BaseEntity
{
    public BaseEntity()
    {
        CreatedUtc = DateTime.UtcNow;
    }
    
    public int Id { get; set; }
    public DateTime CreatedUtc { get; set; }
    public DateTime UpdatedUtc { get; set; }
}