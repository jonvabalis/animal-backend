using Microsoft.AspNetCore.Mvc;
using animal_backend_core.Queries;
using animal_backend_core.Commands;
using OpenAI.Chat;
using OpenAI;
using System.Text.Json;
using System.ClientModel;
using animal_backend_domain.Dtos;

namespace animal_backend_api.Controllers;

public class DiseaseController : BaseController
{   
    private readonly OpenAIClient _openAIClient;
    public DiseaseController(OpenAIClient openAIClient)
    {
        _openAIClient = openAIClient;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllDiseasesQuery query)
    {
        return Ok(await Mediator.Send(new GetAllDiseasesQuery()));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        return Ok(await Mediator.Send(new GetByIdDiseaseQuery(id)));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DiseaseInfoDto dto)
    {
        var command = new CreateDiseaseCommand(
            dto.Name,
            dto.Category,
            dto.LatinName,
            dto.Description
        );

        return Ok(await Mediator.Send(command));
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDiseaseCommand command)
    {
        var updateCommand = new UpdateDiseaseCommand(
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
        return Ok(await Mediator.Send(new DeleteDiseaseCommand(id)));
    }
}