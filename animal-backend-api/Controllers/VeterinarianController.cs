using Microsoft.AspNetCore.Mvc;
using animal_backend_core.Queries;
using animal_backend_core.Commands;
using animal_backend_domain.Dtos;
using animal_backend_domain.Entities;
using Microsoft.Extensions.Configuration.UserSecrets;
namespace animal_backend_api.Controllers;


public class VeterinarianController : BaseController
{

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllVeterinariansQuery query)
    {
        return Ok(await Mediator.Send(new GetAllVeterinariansQuery()));
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        return Ok(await Mediator.Send(new GetByIdVeterinarianQuery(id)));
    }

    [HttpPost()]
    public async Task<IActionResult> Create([FromBody] VeterinarianInfoDto dto)
    {
        var command = new CreateVeterinarianCommand(
            dto.Name,
            dto.Surname,
            dto.Email,
            dto.Password,
            dto.Role,
            dto.PhoneNumber,
            dto.PhotoUrl,
            dto.BirthDate,
            dto.Rank,
            dto.Responsibilities,
            dto.Education,
            dto.Salary,
            dto.FullTime,
            dto.HireDate,
            dto.ExperienceYears,
            dto.Gender
        );
        
        return Ok(await Mediator.Send(command));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateVeterinarianCommand command)
    {
        var updateCommand = new UpdateVeterinarianCommand(
            id,
            command.BirthDate,
            command.Rank,
            command.Responsibilities,
            command.Education,
            command.Salary,
            command.FullTime,
            command.HireDate,
            command.ExperienceYears,
            command.Gender
        );
        return Ok(await Mediator.Send(updateCommand));
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        return Ok(await Mediator.Send(new DeleteVeterinarianCommand(id)));
    }
}