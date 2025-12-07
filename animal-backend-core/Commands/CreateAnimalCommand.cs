using animal_backend_domain.Types;
using MediatR;

namespace animal_backend_core.Commands
{
    public record CreateAnimalCommand(
        string Name,
        AnimalClass Class,
        string PhotoUrl,
        string Breed,
        string Species,
        string SpeciesLatin,
        DateTime DateOfBirth,
        double Weight
    ) : IRequest<Guid>;
}
