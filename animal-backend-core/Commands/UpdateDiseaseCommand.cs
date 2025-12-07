using MediatR;
using animal_backend_domain.Types;

namespace animal_backend_core.Commands
{
    public record UpdateDiseaseCommand(
        Guid Id,
        string Name,
        DiseaseCategory Category,
        string LatinName,
        string? Description
    ) : IRequest<Unit>;
}