using Microsoft.AspNetCore.Mvc;
using animal_backend_core.Queries;
using animal_backend_core.Commands;
using OpenAI.Chat;
using OpenAI;
using System.Text.Json;
using System.ClientModel;
using animal_backend_domain.Dtos;

namespace animal_backend_api.Controllers;

public class VisitController : BaseController
{   

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllVisitsQuery query)
    {
        return Ok(await Mediator.Send(new GetAllVisitsQuery()));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        return Ok(await Mediator.Send(new GetByIdVisitQuery(id)));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] VisitInfoDto dto)
    {
        var command = new CreateVisitCommand(
            dto.Type,
            dto.Start,
            dto.End,
            dto.Location,
            (decimal)dto.Price
        );

        return Ok(await Mediator.Send(command));
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateVisitCommand command)
    {
        var updateCommand = new UpdateVisitCommand(
            id,
            command.Type,
            command.Start,
            command.End,
            command.Location,
            command.Price
        );

        return Ok(await Mediator.Send(updateCommand));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        return Ok(await Mediator.Send(new DeleteVisitCommand(id)));
    }
}