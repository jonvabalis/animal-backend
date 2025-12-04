using Microsoft.AspNetCore.Mvc;
using animal_backend_core.Queries;

namespace animal_backend_api.Controllers;

public class ProductController : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllProductsQuery query)
    {
        return Ok(await Mediator.Send(new GetAllProductsQuery()));
    }

}