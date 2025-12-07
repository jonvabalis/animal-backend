using animal_backend_domain.Types;
using MediatR;

namespace animal_backend_core.Commands
{
    public record CreateDiseaseCommand(
        string Name,
        DiseaseCategory Category,
        string LatinName,
        string Description
    ) : IRequest<Guid>;
}
