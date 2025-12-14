using MediatR;

namespace animal_backend_core.Commands;

public record UpdateMyProfileCommand(
    Guid UserId,
    string Name,
    string Surname,
    string PhoneNumber,
    string PhotoUrl
) : IRequest<Unit>;