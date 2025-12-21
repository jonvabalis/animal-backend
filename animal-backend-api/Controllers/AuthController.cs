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
    
    [HttpGet("confirm-email/{userId:guid}")]
    public async Task<IActionResult> ConfirmEmail(Guid userId)
    {
        var command = new ConfirmEmailCommand { UserId = userId };
        var result = await mediator.Send(command);

        if (!result.Success)
            return Content(GenerateErrorHtml("Vartotojas nerastas"), "text/html");

        if (result.AlreadyConfirmed)
            return Content(GenerateAlreadyConfirmedHtml(), "text/html");

        return Content(GenerateSuccessHtml(), "text/html");
    }

    private string GenerateSuccessHtml()
    {
        return @"
            <!DOCTYPE html>
            <html>
            <head>
                <style>
                    body { 
                        font-family: Arial, sans-serif; 
                        display: flex; 
                        justify-content: center; 
                        align-items: center; 
                        height: 100vh; 
                        margin: 0;
                        background: #f5f5f5;
                    }
                    .success-box {
                        background: white;
                        padding: 40px;
                        border-radius: 10px;
                        box-shadow: 0 2px 10px rgba(0,0,0,0.1);
                        text-align: center;
                    }
                    .success-icon {
                        font-size: 60px;
                        color: #4CAF50;
                    }
                    h1 { color: #4CAF50; }
                </style>
            </head>
            <body>
                <div class='success-box'>
                    <div class='success-icon'></div>
                    <h1>El. pastas patvirtintas!</h1>
                    <p>Jusu paskyra sekmingai aktyvuota.</p>
                    <p>Galite uzdaryti si langa ir prisijungti.</p>
                </div>
            </body>
            </html>
        ";
    }

    private string GenerateAlreadyConfirmedHtml()
    {
        return @"
            <!DOCTYPE html>
            <html>
            <head>
                <style>
                    body { 
                        font-family: Arial, sans-serif; 
                        display: flex; 
                        justify-content: center; 
                        align-items: center; 
                        height: 100vh; 
                        margin: 0;
                        background: #f5f5f5;
                    }
                    .info-box {
                        background: white;
                        padding: 40px;
                        border-radius: 10px;
                        box-shadow: 0 2px 10px rgba(0,0,0,0.1);
                        text-align: center;
                    }
                    .info-icon {
                        font-size: 60px;
                        color: #2196F3;
                    }
                    h1 { color: #2196F3; }
                </style>
            </head>
            <body>
                <div class='info-box'>
                    <div class='info-icon'>ℹ</div>
                    <h1>El. pastas jau patvirtintas</h1>
                    <p>Jusu paskyra jau buvo aktyvuota anksciau.</p>
                    <p>Galite prisijungti.</p>
                </div>
            </body>
            </html>
        ";
    }

    private string GenerateErrorHtml(string message)
    {
        return $@"
            <!DOCTYPE html>
            <html>
            <head>
                <style>
                    body {{ 
                        font-family: Arial, sans-serif; 
                        display: flex; 
                        justify-content: center; 
                        align-items: center; 
                        height: 100vh; 
                        margin: 0;
                        background: #f5f5f5;
                    }}
                    .error-box {{
                        background: white;
                        padding: 40px;
                        border-radius: 10px;
                        box-shadow: 0 2px 10px rgba(0,0,0,0.1);
                        text-align: center;
                    }}
                    .error-icon {{
                        font-size: 60px;
                        color: #f44336;
                    }}
                    h1 {{ color: #f44336; }}
                </style>
            </head>
            <body>
                <div class='error-box'>
                    <div class='error-icon'>✗</div>
                    <h1>Klaida</h1>
                    <p>{message}</p>
                </div>
            </body>
            </html>
        ";
    }
}