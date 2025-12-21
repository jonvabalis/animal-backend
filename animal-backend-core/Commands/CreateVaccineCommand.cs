using MediatR;

namespace animal_backend_core.Commands
{
    public record CreateVaccineCommand(
        string Name,
        DateTime Date,
        string Description,
        string Manufacturer,
        Guid AnimalId
    ) : IRequest<Guid>;
}