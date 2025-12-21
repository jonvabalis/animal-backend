using animal_backend_core.Commands;
using animal_backend_core.Security;
using animal_backend_domain.Dtos.Auth;
using animal_backend_infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace animal_backend_core.Handlers;

public class LoginUserCommandHandler(
    AnimalDbContext dbContext,
    JwtTokenService jwtTokenService
) : IRequestHandler<LoginUserCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var email = request.Request.Email.Trim().ToLowerInvariant();

        var user = await dbContext.Users.FirstOrDefaultAsync(
            u => u.Email.ToLower() == email,
            cancellationToken);

        if (user is null)
            throw new InvalidOperationException("Invalid email or password.");

        if (!user.Confirmed)
        {
            throw new UnauthorizedAccessException("Please confirm your email before logging in.");
        }
        
        var ok = PasswordHasher.VerifyFromStorage(request.Request.Password, user.Password);
        if (!ok)
            throw new InvalidOperationException("Invalid email or password.");

        var token = jwtTokenService.CreateAccessToken(user);

        return new AuthResponseDto
        {
            AccessToken = token,
            UserId = user.Id,
            Email = user.Email,
            Role = user.Role.ToString()
        };
    }
}
