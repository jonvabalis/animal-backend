using MediatR;

namespace animal_backend_core.Commands
{
    public record UpdateVaccineCommand(
        Guid Id,
        string Name,
        DateTime Date,
        string Description,
        string Manufacturer
    ) : IRequest<Unit>;
}