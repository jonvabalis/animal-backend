using MediatR;
using animal_backend_domain.Types;

namespace animal_backend_core.Commands
{
    public record UpdateAnimalCommand(
        Guid Id,
        string Name,
        AnimalClass Class,
        string PhotoUrl,
        string Breed,
        string Species,
        string SpeciesLatin,
        DateTime DateOfBirth,
        double Weight
    ) : IRequest<Unit>;
}