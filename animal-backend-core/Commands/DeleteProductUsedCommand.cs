using MediatR;

namespace animal_backend_core.Commands
{
    public record DeleteProductUsedCommand(Guid AnimalId, Guid Id) : IRequest<Unit>;
}