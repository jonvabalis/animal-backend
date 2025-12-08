using MediatR;
using Microsoft.EntityFrameworkCore;
using animal_backend_infrastructure;
using animal_backend_core.Commands;
namespace animal_backend_core.Handlers;

public class DeleteVisitCommandHandler(AnimalDbContext dbContext)
    : IRequestHandler<DeleteVisitCommand, Unit>
{
    public async Task<Unit> Handle(DeleteVisitCommand request, CancellationToken cancellationToken)
    {
        var visit = await dbContext.Visits
            .FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);

        if (visit is null)
        {
            throw new KeyNotFoundException($"Visit with ID {request.Id} not found");
        }

        dbContext.Visits.Remove(visit);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}