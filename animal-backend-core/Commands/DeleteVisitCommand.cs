using MediatR;

namespace animal_backend_core.Commands
{
    public record DeleteVisitCommand(Guid Id) : IRequest<Unit>;
}