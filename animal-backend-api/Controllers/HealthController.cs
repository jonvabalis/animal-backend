using Microsoft.AspNetCore.Mvc;

namespace animal_backend_api.Controllers;

public class HealthController : BaseController
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { status = "ok" });
    }
}
