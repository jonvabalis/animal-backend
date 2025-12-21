using animal_backend_core.Commands;
using animal_backend_core.Security;
using animal_backend_core.Services;
using animal_backend_domain.Dtos.Auth;
using animal_backend_domain.Entities;
using animal_backend_domain.Types;
using animal_backend_infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace animal_backend_core.Handlers;

public class RegisterUserCommandHandler(
    AnimalDbContext dbContext,
    JwtTokenService jwtTokenService,
    IEmailConfirmationService emailConfirmationService) : IRequestHandler<RegisterUserCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var email = request.Request.Email.Trim().ToLowerInvariant();

        var exists = await dbContext.Users.AnyAsync(
            u => u.Email.ToLower() == email,
            cancellationToken);

        if (exists)
            throw new InvalidOperationException("User with this email already exists.");

        // ✅ store hash+salt inside one string field (User.Password)
        var storedPassword = PasswordHasher.HashForStorage(request.Request.Password);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = request.Request.Name,
            Surname = request.Request.Surname,
            Email = email,
            Password = storedPassword,
            Confirmed = false,
            Role = RoleType.Client,
            PhoneNumber = request.Request.PhoneNumber,
            PhotoUrl = request.Request.PhotoUrl ?? ""
        };

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        await emailConfirmationService.SendConfirmationEmailAsync(
            user.Email, 
            user.Name, 
            user.Id
        );
        
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
