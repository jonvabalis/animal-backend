using MediatR;

namespace animal_backend_core.Commands;

public record DeleteMyProfileCommand(Guid UserId) : IRequest<Unit>;
