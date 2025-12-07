using MediatR;

namespace animal_backend_core.Commands
{
    public record DeleteIlnessCommand(Guid Id) : IRequest<Unit>;
}