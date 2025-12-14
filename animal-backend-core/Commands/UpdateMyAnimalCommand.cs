using animal_backend_domain.Types;
using MediatR;

namespace animal_backend_core.Commands;

public record UpdateMyAnimalCommand(
    Guid UserId,
    Guid AnimalId,
    string Name,
    AnimalClass Class,
    string PhotoUrl,
    string Breed,
    string Species,
    string SpeciesLatin,
    DateTime DateOfBirth,
    double Weight
) : IRequest<Unit>;