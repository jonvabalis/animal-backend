using MediatR;
using Microsoft.EntityFrameworkCore;
using animal_backend_infrastructure;
using animal_backend_core.Commands;
namespace animal_backend_core.Handlers;

public class UpdateVisitCommandHandler(AnimalDbContext dbContext)
    : IRequestHandler<UpdateVisitCommand, Unit>
{
    public async Task<Unit> Handle(UpdateVisitCommand request, CancellationToken cancellationToken)
    {
        // Convert incoming DateTimes to UTC for PostgreSQL
        var startUtc = request.Start.Kind == DateTimeKind.Utc 
            ? request.Start 
            : request.Start.ToUniversalTime();
        var endUtc = request.End.Kind == DateTimeKind.Utc 
            ? request.End 
            : request.End.ToUniversalTime();
        
        if (startUtc > endUtc || startUtc < DateTime.UtcNow)
        {
            throw new InvalidOperationException("Invalid start or end hour.");
        }
        
        var visit = await dbContext.Visits
            .FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);

        if (visit is null)
        {
            throw new KeyNotFoundException($"Visit with ID {request.Id} not found");
        }

        visit.Type = request.Type;
        visit.Start = startUtc;
        visit.End = endUtc;
        visit.Location = request.Location;
        visit.Price = (double)request.Price;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}