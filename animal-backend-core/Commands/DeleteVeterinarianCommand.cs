using MediatR;

namespace animal_backend_core.Commands
{
    public record DeleteVeterinarianCommand(Guid Id) : IRequest<Unit>;
}