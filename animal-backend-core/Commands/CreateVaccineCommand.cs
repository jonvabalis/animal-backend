using animal_backend_domain.Types;
using MediatR;

namespace animal_backend_core.Commands
{
    public record CreateVaccineCommand(
        string Name,
        VaccineCategory Category,
        string LatinName,
        string Description
    ) : IRequest<Guid>;
}