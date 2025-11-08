using AlertVault.Core.Service;
using Microsoft.AspNetCore.Mvc;

namespace AlertVault.Controllers;

[ApiController]
[Route("/")]
public class IndexController(AlertService alertService) : ControllerBase
{
    [HttpGet("{uuid:minlength(36):maxlength(36)}")]
    public async Task<IActionResult> Index(string uuid)
    {
        var alert = await alertService.UpdateLastChecked(uuid);
        if (alert is null)
        {
            return NotFound();
        }

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