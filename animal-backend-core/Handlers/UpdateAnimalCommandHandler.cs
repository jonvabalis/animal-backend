using MediatR;
using Microsoft.EntityFrameworkCore;
using animal_backend_domain.Dtos;
using animal_backend_infrastructure;
using animal_backend_core.Commands;
namespace animal_backend_core.Handlers;

public class UpdateAnimalCommandHandler(AnimalDbContext dbContext)
    : IRequestHandler<UpdateAnimalCommand, Unit>
{
    public async Task<Unit> Handle(UpdateAnimalCommand request, CancellationToken cancellationToken)
    {
        var animal = await dbContext.Animals
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (animal is null)
        {
            throw new KeyNotFoundException($"Animal with ID {request.Id} not found");
        }

        animal.Name = request.Name;
        animal.Class = request.Class;
        animal.PhotoUrl = request.PhotoUrl;
        animal.Breed = request.Breed;
        animal.Species = request.Species;
        animal.SpeciesLatin = request.SpeciesLatin;
        animal.DateOfBirth = request.DateOfBirth;
        animal.Weight = request.Weight;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}