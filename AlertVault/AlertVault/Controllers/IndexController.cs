using AlertVault.Core.Service;
using Microsoft.AspNetCore.Mvc;

namespace AlertVault.Controllers;

[ApiController]
[Route("/")]
public class IndexController(AlertService alertService) : ControllerBase
{
    [HttpGet("{uuid:guid}")]
    [HttpPost("{uuid:guid}")]
    [HttpPut("{uuid:guid}")]
    [HttpDelete("{uuid:guid}")]
    [HttpPatch("{uuid:guid}")]
    [HttpOptions("{uuid:guid}")]
    [HttpHead("{uuid:guid}")]
    public async Task<IActionResult> Index(Guid uuid)
    {
        // We need to update the requests table with a new request for this alert. Shall we queue it up?
        var alert = await alertService.UpdateLastChecked(uuid);
        if (alert is null)
        {
            return NotFound();
        }
        
        // Get the method, headers, body, etc. and log them as a request for this alert.
        using var reader = new StreamReader(HttpContext.Request.Body);
        var body = await reader.ReadToEndAsync();
        await alertService.AddRequest(alert.Id,
            HttpContext.Connection.RemoteIpAddress,
            HttpContext.Request.Headers.UserAgent,
            body,
            HttpContext.Request.Method
            );

        return Ok(new
        {
            alert.Uuid,
            alert.LastCheckUtc
        });
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        return File("index.html", "text/html");
    }
}