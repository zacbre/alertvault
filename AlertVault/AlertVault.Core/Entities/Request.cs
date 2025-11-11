using System.Net;
using AlertVault.Core.Dto;

namespace AlertVault.Core.Entities;

public class Request
{
    public int Id { get; set; }
    public int AlertId { get; set; }
    public RequestMethodTypeEnum Method { get; set; }
    public required IPAddress IpAddress { get; set; }
    public required string UserAgent { get; set; }
    public DateTime CreatedUtc { get; set; }
    public DateTime UpdatedUtc { get; set; }

    public required Alert Alert { get; set; }
}