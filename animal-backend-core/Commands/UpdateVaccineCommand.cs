using MediatR;
using animal_backend_domain.Types;

namespace animal_backend_core.Commands
{
    public record UpdateVaccineCommand(
        Guid Id,
        string Name,
        VaccineCategory Category,
        string LatinName,
        string? Description
    ) : IRequest<Unit>;
}