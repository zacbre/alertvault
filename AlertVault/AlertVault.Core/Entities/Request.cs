using System.Net;
using AlertVault.Core.Dto;

namespace AlertVault.Core.Entities;

public class Request : BaseEntity
{
    public int AlertId { get; set; }
    public RequestMethodTypeEnum Method { get; set; }
    public required IPAddress IpAddress { get; set; }
    public required string UserAgent { get; set; }

    public required Alert Alert { get; set; }
}