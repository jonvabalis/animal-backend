using animal_backend_core.Commands;
using animal_backend_core.Queries;
using Microsoft.AspNetCore.Mvc;

namespace animal_backend_api.Controllers;

public class UserController : BaseController
{
    
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllUsersQuery query)
    {
        return Ok(await Mediator.Send(new GetAllUsersQuery()));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        return Ok(await Mediator.Send(new GetByIdUserQuery(id)));
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserCommand command)
    {
        var updateCommand = new UpdateUserCommand(
            id,
            command.Name,
            command.Surname,
            command.Email,
            command.Password,
            command.Role,
            command.PhoneNumber,
            command.PhotoUrl
        );
        return Ok(await Mediator.Send(updateCommand));
    }   
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        return Ok(await Mediator.Send(new DeleteUserCommand(id)));
    }
}