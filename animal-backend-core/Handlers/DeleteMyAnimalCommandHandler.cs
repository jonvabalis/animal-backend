using animal_backend_core.Commands;
using animal_backend_infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace animal_backend_core.Handlers;

public class DeleteMyAnimalCommandHandler(AnimalDbContext dbContext)
    : IRequestHandler<DeleteMyAnimalCommand, Unit>
{
    public async Task<Unit> Handle(DeleteMyAnimalCommand request, CancellationToken ct)
    {
        var animal = await dbContext.Animals
            .FirstOrDefaultAsync(a => a.Id == request.AnimalId, ct);

        if (animal is null)
            throw new KeyNotFoundException("Animal not found.");

        if (animal.UserId != request.UserId)
            throw new UnauthorizedAccessException("You are not the owner of this animal.");

        dbContext.Animals.Remove(animal);
        await dbContext.SaveChangesAsync(ct);
        return Unit.Value;
    }
}