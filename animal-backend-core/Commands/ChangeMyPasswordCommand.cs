using MediatR;

namespace animal_backend_core.Commands;

public record ChangeMyPasswordCommand(
    Guid UserId,
    string CurrentPassword,
    string NewPassword
) : IRequest<Unit>;