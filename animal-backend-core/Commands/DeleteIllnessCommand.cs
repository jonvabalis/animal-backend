using MediatR;

namespace animal_backend_core.Commands
{
    public record DeleteIllnessCommand(Guid AnimalId, Guid Id) : IRequest<Unit>;
}