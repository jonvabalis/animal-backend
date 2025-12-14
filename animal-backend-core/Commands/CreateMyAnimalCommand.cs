using animal_backend_domain.Types;
using MediatR;

namespace animal_backend_core.Commands;

public record CreateMyAnimalCommand(
    Guid UserId,
    string Name,
    AnimalClass Class,
    string PhotoUrl,
    string Breed,
    string Species,
    string SpeciesLatin,
    DateTime DateOfBirth,
    double Weight
) : IRequest<Guid>;