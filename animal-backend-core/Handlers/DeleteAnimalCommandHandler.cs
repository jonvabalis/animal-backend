using MediatR;
using Microsoft.EntityFrameworkCore;
using animal_backend_infrastructure;
using animal_backend_core.Commands;
namespace animal_backend_core.Handlers;

public class DeleteAnimalCommandHandler(AnimalDbContext dbContext)
    : IRequestHandler<DeleteAnimalCommand, Unit>
{
    public async Task<Unit> Handle(DeleteAnimalCommand request, CancellationToken cancellationToken)
    {
        var animal = await dbContext.Animals
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (animal is null)
        {
            throw new KeyNotFoundException($"Animal with ID {request.Id} not found");
        }

        dbContext.Animals.Remove(animal);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}