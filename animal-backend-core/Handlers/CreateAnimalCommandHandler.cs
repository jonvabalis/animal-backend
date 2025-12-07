using animal_backend_infrastructure;
using MediatR;
using animal_backend_core.Commands;

namespace animal_backend_core.Handlers;

public class CreateAnimalCommandHandler(AnimalDbContext dbContext)
    : IRequestHandler<CreateAnimalCommand, Guid>
{
    public async Task<Guid> Handle(CreateAnimalCommand request, CancellationToken cancellationToken)
    {
        var animal = new animal_backend_domain.Entities.Animal
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Class = request.Class,
            PhotoUrl = request.PhotoUrl,
            Breed = request.Breed,
            Species = request.Species,
            SpeciesLatin = request.SpeciesLatin,
            DateOfBirth = request.DateOfBirth,
            Weight = request.Weight
        };

        dbContext.Animals.Add(animal);
        await dbContext.SaveChangesAsync(cancellationToken);

        return animal.Id;
    }
}