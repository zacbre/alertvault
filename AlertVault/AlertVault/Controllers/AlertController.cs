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

    [HttpGet("{uuid:guid}")]
    public async Task<IActionResult> Get(Guid uuid)
    {
        var alert = await alertService.Get(uuid);
        if (alert is null)
        {
            return NotFound();
        }

        return Ok(alert);
    }

    [HttpGet("{uuid:guid}/requests")]
    public async Task<IActionResult> GetRequests(Guid uuid)
    {
        return Ok(await alertService.GetRequests(uuid));
    }

    [HttpDelete("{uuid:guid}")]
    public async Task<IActionResult> Delete(Guid uuid)
    {
        var alert = await alertService.Get(uuid);
        if (alert is null)
        {
            return NotFound();
        }

        alertService.Delete(alert);

        return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AlertModel model)
    {
        var addedAlert = await alertService.Add(model.UserId, model.Interval);
        
        return Ok(addedAlert);
    }
}