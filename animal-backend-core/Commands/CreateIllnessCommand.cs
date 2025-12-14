using MediatR;

namespace animal_backend_core.Commands
{
    public record CreateIllnessCommand(
        Guid AnimalId,
        string Name,
        string Description,
        DateTime DateDiagnosed,
        Guid? DiseaseId
    ) : IRequest<Guid>;
}
