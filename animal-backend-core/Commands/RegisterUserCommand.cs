using animal_backend_domain.Dtos.Auth;
using MediatR;

namespace animal_backend_core.Commands;

public record RegisterUserCommand(RegisterRequestDto Request) : IRequest<AuthResponseDto>;