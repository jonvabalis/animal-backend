using MediatR;

namespace animal_backend_core.Commands
{
    public record UpdateIllnessCommand(
        Guid Id,
        string Name,
        string Description,
        DateTime DateDiagnosed,
        Guid AnimalId,
        Guid? DiseaseId
    ) : IRequest<Unit>;
}