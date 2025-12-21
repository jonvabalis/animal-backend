using animal_backend_core.Commands;
using animal_backend_infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace animal_backend_core.Handlers;

public class CancelVisitCommandHandler(AnimalDbContext dbContext)
    : IRequestHandler<CancelVisitCommand, Guid>
{
    public async Task<Guid> Handle(CancelVisitCommand request, CancellationToken cancellationToken)
    {
        var visit = await dbContext.Visits
            .FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);
        
        if (visit == null)
        {
            throw new KeyNotFoundException($"Visit with ID {request.Id} not found");
        }
        
        var veterinarian = await dbContext.Veterinarians
            .Include(v => v.WorkHours)
            .Where(v => v.Id == visit.VeterinarianId)
            .FirstOrDefaultAsync(cancellationToken);

        if (veterinarian == null)
        {
            throw new InvalidOperationException("Veterinarian with this ID does not exist.");
        }

        if (veterinarian.WorkHours.All(wh => wh.Date != DateOnly.FromDateTime(visit.Start)))
        {
            throw new InvalidOperationException("Invalid time selected");
        }
        
        var startHour = visit.Start.Hour;
        var endHour = visit.End.Hour;
        
        var workHours = veterinarian.WorkHours
            .Where(wh => wh.Date == DateOnly.FromDateTime(visit.Start)).ToList();

        for (int i = startHour; i < endHour; i++)
        {
            if (workHours.All(wh => wh.Hour != i))
            {
                throw new InvalidOperationException("Invalid time range selected");
            }
        }

        foreach (var workHour in workHours)
        {
            if (workHour.Hour >= startHour && workHour.Hour <= endHour)
            {
                workHour.Taken = false;
            }
        }
        
        dbContext.Visits.Remove(visit);
        await dbContext.SaveChangesAsync(cancellationToken);

        return visit.Id;
    }
}