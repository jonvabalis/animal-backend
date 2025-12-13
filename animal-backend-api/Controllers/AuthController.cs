using animal_backend_core.Commands;
using animal_backend_domain.Dtos.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace animal_backend_api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterRequestDto dto, CancellationToken ct)
        => Ok(await mediator.Send(new RegisterUserCommand(dto), ct));

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequestDto dto, CancellationToken ct)
        => Ok(await mediator.Send(new LoginUserCommand(dto), ct));
}