using Microsoft.AspNetCore.Mvc;
using animal_backend_core.Queries;
using animal_backend_core.Commands;

using animal_backend_domain.Dtos;

namespace animal_backend_api.Controllers;

public class AnimalController : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllAnimalsQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        return Ok(await Mediator.Send(new GetByIdAnimalQuery(id)));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AnimalInfoDto dto)
    {
        var command = new CreateAnimalCommand(
            dto.Name,
            dto.Class,
            dto.PhotoUrl,
            dto.Breed,
            dto.Species,
            dto.SpeciesLatin,
            dto.DateOfBirth,
            dto.Weight
        );

        return Ok(await Mediator.Send(command));
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAnimalCommand command)
    {
        var updateCommand = new UpdateAnimalCommand(
            id,
            command.Name,
            command.Class,
            command.PhotoUrl,
            command.Breed,
            command.Species,
            command.SpeciesLatin,
            command.DateOfBirth,
            command.Weight
        );

        return Ok(await Mediator.Send(updateCommand));
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        return Ok(await Mediator.Send(new DeleteAnimalCommand(id)));
    }

    // Additional endpoints related to animals (Illnesses, ProductsUsed)
    
    
}