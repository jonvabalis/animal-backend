using Microsoft.AspNetCore.Mvc;
using animal_backend_core.Queries;
using animal_backend_core.Commands;
using OpenAI;
using animal_backend_domain.Dtos;

namespace animal_backend_api.Controllers;

public class VaccineController(OpenAIClient openAIClient) : BaseController
{   

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllVaccinesQuery query)
    {
        return Ok(await Mediator.Send(new GetAllVaccinesQuery()));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        return Ok(await Mediator.Send(new GetByIdVaccineQuery(id)));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] VaccineInfoDto dto)
    {
        var command = new CreateVaccineCommand(
            dto.Name,
            dto.Date,
            dto.Description,
            dto.Manufacturer
        );

        return Ok(await Mediator.Send(command));
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateVaccineCommand command)
    {
        var updateCommand = new UpdateVaccineCommand(
            id,
            command.Name,
            command.Date,
            command.Description,
            command.Manufacturer
        );

        return Ok(await Mediator.Send(updateCommand));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        return Ok(await Mediator.Send(new DeleteVaccineCommand(id)));
    }
}