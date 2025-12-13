using animal_backend_core.Commands;
using animal_backend_infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace animal_backend_core.Handlers;

public class UpdateMyAnimalCommandHandler(AnimalDbContext dbContext)
    : IRequestHandler<UpdateMyAnimalCommand, Unit>
{
    public async Task<Unit> Handle(UpdateMyAnimalCommand request, CancellationToken ct)
    {
        var animal = await dbContext.Animals
            .FirstOrDefaultAsync(a => a.Id == request.AnimalId, ct);

        if (animal is null)
            throw new KeyNotFoundException("Animal not found.");

        if (animal.UserId != request.UserId)
            throw new UnauthorizedAccessException("You are not the owner of this animal.");

        animal.Name = request.Name;
        animal.Class = request.Class;
        animal.PhotoUrl = request.PhotoUrl;
        animal.Breed = request.Breed;
        animal.Species = request.Species;
        animal.SpeciesLatin = request.SpeciesLatin;
        animal.DateOfBirth = request.DateOfBirth;
        animal.Weight = request.Weight;

        await dbContext.SaveChangesAsync(ct);
        return Unit.Value;
    }
}