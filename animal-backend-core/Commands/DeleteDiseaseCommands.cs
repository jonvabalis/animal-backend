using MediatR;

namespace animal_backend_core.Commands
{
    public record DeleteDiseaseCommand(Guid Id) : IRequest<Unit>;
}