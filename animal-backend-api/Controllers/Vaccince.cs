using Microsoft.AspNetCore.Mvc;
using animal_backend_core.Queries;
using animal_backend_core.Commands;
using OpenAI.Chat;
using OpenAI;
using System.Text.Json;
using System.ClientModel;
using animal_backend_domain.Dtos;

namespace animal_backend_api.Controllers;

public class VaccinceController : BaseController
{   
    private readonly OpenAIClient _openAIClient;
    public VaccinceController(OpenAIClient openAIClient)
    {
        _openAIClient = openAIClient;
    }

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
            dto.Category,
            dto.LatinName,
            dto.Description
        );

        return Ok(await Mediator.Send(command));
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateVaccineCommand command)
    {
        var updateCommand = new UpdateVaccineCommand(
            id,
            command.Name,
            command.Category,
            command.LatinName,
            command.Description
        );

        return Ok(await Mediator.Send(updateCommand));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        return Ok(await Mediator.Send(new DeleteVaccineCommand(id)));
    }
}