using animal_backend_infrastructure;
using MediatR;
using animal_backend_core.Queries;
using animal_backend_domain.Dtos;

namespace animal_backend_core.Handlers;

public class GetByIdAnimalQueryHandler(AnimalDbContext dbContext)
    : IRequestHandler<GetByIdAnimalQuery, AnimalInfoDto?>
{
    public async Task<AnimalInfoDto?> Handle(GetByIdAnimalQuery request, CancellationToken cancellationToken)
    {
        // TODO: adjust entity and mapping according to your domain model
        var animal = await dbContext.Animals.FindAsync(new object[] { request.Id }, cancellationToken);

        if (animal is null)
        {
            return null!;
        }

        return new AnimalInfoDto
        {
            Id = animal.Id,
            Name = animal.Name,
            Species = animal.Species,
            SpeciesLatin = animal.SpeciesLatin,
            Breed = animal.Breed,
            DateOfBirth = animal.DateOfBirth,
            Weight = animal.Weight,
            PhotoUrl = animal.PhotoUrl,
            Class = animal.Class,
        };
    }
}