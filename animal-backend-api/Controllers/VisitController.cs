using Microsoft.AspNetCore.Mvc;
using animal_backend_core.Queries;
using animal_backend_core.Commands;
using animal_backend_domain.Dtos;
using animal_backend_domain.Dtos.Workday;

namespace animal_backend_api.Controllers;

public class VisitController : BaseController
{

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllVisitsQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        return Ok(await Mediator.Send(new GetByIdVisitQuery(id)));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateVisitCommand request)
    {
        var command = new CreateVisitCommand(
            request.Type,
            request.Start,
            request.End,
            request.Location,
            request.Price,
            request.VeterinarianUuid,
            request.UserUuid);

        return Ok(await Mediator.Send(command));
    }
    
    [HttpPost("cancel")]
    public async Task<IActionResult> Cancel([FromBody] CancelVisitCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateVisitCommand command)
    {
        var updateCommand = command with { Id = id };

        return Ok(await Mediator.Send(updateCommand));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        return Ok(await Mediator.Send(new DeleteVisitCommand(id)));
    }
    
    [HttpPost("workday")]
    public async Task<IActionResult> CreateWorkday([FromBody] CreateWorkday dto)
    {
        var command = new CreateVeterinarianWorkdayCommand(
            dto.VeterinarianId,
            dto.Date,
            dto.StartHour,
            dto.EndHour
        );

        return Ok(await Mediator.Send(command));
    }
    
    [HttpGet("workday")]
    public async Task<IActionResult> GetWorkday([FromQuery] GetVeterinarianAvailableWorkdayQuery query)
    {
        return Ok(await Mediator.Send(query));
    }
    
    [HttpDelete("workday")]
    public async Task<IActionResult> DeleteWorkday([FromQuery] DeleteVeterinarianWorkdayQuery query)
    {
        return Ok(await Mediator.Send(query));
    }
}