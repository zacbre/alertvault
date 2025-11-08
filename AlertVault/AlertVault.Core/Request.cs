using System.Net;
using AlertVault.Core.Dto;

namespace AlertVault.Core;

public class Request
{
    public int Id {get; set;}
    public int AlertId { get; set; }
    public RequestMethodTypeEnum Method { get; set; }
    public required IPAddress IpAddress { get; set; }
    public required string UserAgent { get; set; }
    
    public Alert Alert { get; set; }
}