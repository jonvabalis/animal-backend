using MediatR;

namespace animal_backend_core.Commands
{
    public record DeleteVaccineCommand(Guid Id) : IRequest<Unit>;
}