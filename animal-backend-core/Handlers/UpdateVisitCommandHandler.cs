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
        if (request.Start > request.End || request.Start > DateTime.Now)
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
        visit.Start = request.Start;
        visit.End = request.End;
        visit.Location = request.Location;
        visit.Price = (double)request.Price;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}