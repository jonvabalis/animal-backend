using MediatR;

namespace animal_backend_core.Commands
{
    public record DeleteAnimalCommand(Guid Id) : IRequest<Unit>;
}