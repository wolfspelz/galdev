using Microsoft.AspNetCore.Mvc;

namespace GaldevWeb.Controllers;

[ApiController]
public class ReloadController : GaldevControllerBase
{
    public ReloadController(GaldevApp app) : base(app) { }

    [Route("[controller]")]
    [HttpGet]
    public IActionResult Index()
    {
        App.Timelines.Reload();

        return new ContentResult {
            Content = $"{DateTime.UtcNow}"
        };
    }
}
