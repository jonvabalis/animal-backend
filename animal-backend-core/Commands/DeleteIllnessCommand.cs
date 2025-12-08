using MediatR;

namespace animal_backend_core.Commands
{
    public record DeleteIllnessCommand(Guid Id) : IRequest<Unit>;
}