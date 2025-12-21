using MediatR;
using Microsoft.EntityFrameworkCore;
using animal_backend_infrastructure;
using animal_backend_core.Commands;
namespace animal_backend_core.Handlers;

public class DeleteIllnessCommandHandler(AnimalDbContext dbContext)
    : IRequestHandler<DeleteIllnessCommand, Unit>
{
    public async Task<Unit> Handle(DeleteIllnessCommand request, CancellationToken cancellationToken)
    {
        var illness = await dbContext.Illnesses
            .FirstOrDefaultAsync(p => p.Id == request.Id && p.AnimalId == request.AnimalId, cancellationToken);

        if (illness is null)
        {
            throw new KeyNotFoundException($"Illness with ID {request.Id} not found");
        }

        dbContext.Illnesses.Remove(illness);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}