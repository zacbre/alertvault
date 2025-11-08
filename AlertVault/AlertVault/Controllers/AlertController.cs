using AlertVault.Core;
using AlertVault.Core.Service;
using AlertVault.Models;
using Microsoft.AspNetCore.Mvc;

namespace AlertVault.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlertController(AlertService alertService) : ControllerBase
{
    [HttpGet("all")]
    public async Task<IActionResult> All()
    {
        return Ok(await alertService.All());
    }
    
    [HttpGet("{uuid:minlength(36):maxlength(36)}")]
    public async Task<IActionResult> Get(string uuid)
    {
        var alert = await alertService.Get(uuid);
        if (alert is null)
        {
            return NotFound();
        }
        
        return Ok(alert);
    }
    
    [HttpGet("{uuid:minlength(36):maxlength(36)}/requests")]
    public async Task<IActionResult> GetRequests(string uuid)
    {
        var alertRequests = await alertService.GetRequests(uuid);
        if (alertRequests is null)
        {
            return NotFound();
        }
        
        return Ok(alertRequests);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AlertModel alert)
    {
        var addedAlert = await alertService.Add(alert.Interval);
        
        return Ok(addedAlert);
    }
}