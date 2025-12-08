using MediatR;

namespace animal_backend_core.Commands
{
    public record DeleteProductUsedCommand(Guid Id) : IRequest<Unit>;
}