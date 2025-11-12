using System.ComponentModel.DataAnnotations;
using System.Net;
using AlertVault.Core.Entities.Dto;

namespace AlertVault.Core.Entities;

public class Request : BaseEntity
{
    public int AlertId { get; set; }
    public RequestMethodTypeEnum Method { get; set; }
    public required IPAddress? IpAddress { get; set; }
    
    public int? UserAgentId { get; set; }
    
    [MaxLength(1024)]
    public string? Body { get; set; }
    
    public Alert Alert { get; set; } = null!;
    public UserAgent? UserAgent { get; set; }
}