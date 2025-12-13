using animal_backend_core.Commands;
using animal_backend_infrastructure;
using MediatR;

namespace animal_backend_core.Handlers;

public class CreateMyAnimalCommandHandler(AnimalDbContext dbContext)
    : IRequestHandler<CreateMyAnimalCommand, Guid>
{
    public async Task<Guid> Handle(CreateMyAnimalCommand request, CancellationToken ct)
    {
        var animal = new animal_backend_domain.Entities.Animal
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,              // ✅ ownership enforced here
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
        await dbContext.SaveChangesAsync(ct);
        return animal.Id;
    }
}