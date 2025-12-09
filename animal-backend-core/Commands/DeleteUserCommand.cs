using MediatR;

namespace animal_backend_core.Commands
{
    public record DeleteUserCommand(Guid Id) : IRequest<Unit>;
}