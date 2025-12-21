using MediatR;

namespace animal_backend_core.Commands
{
    public record CreateIllnessCommand(
        string Name,
        string Description,
        DateTime DateDiagnosed,
        Guid AnimalId,
        Guid? DiseaseId
    ) : IRequest<Guid>;
}
