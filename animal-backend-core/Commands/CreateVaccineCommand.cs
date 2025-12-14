using MediatR;

namespace animal_backend_core.Commands
{
    public record CreateVaccineCommand(
        Guid AnimalId,
        string Name,
        DateTime Date,
        string Description,
        string Manufacturer
    ) : IRequest<Guid>;
}