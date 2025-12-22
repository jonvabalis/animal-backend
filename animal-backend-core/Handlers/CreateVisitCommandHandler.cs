using animal_backend_infrastructure;
using MediatR;
using animal_backend_core.Commands;
using Microsoft.EntityFrameworkCore;

namespace animal_backend_core.Handlers;

public class CreateVisitCommandHandler(AnimalDbContext dbContext)
    : IRequestHandler<CreateVisitCommand, Guid>
{
    public async Task<Guid> Handle(CreateVisitCommand request, CancellationToken cancellationToken)
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
        
        var veterinarian = await dbContext.Veterinarians
            .Include(v => v.WorkHours)
            .Where(v => v.Id == request.VeterinarianUuid)
            .FirstOrDefaultAsync(cancellationToken);

        if (veterinarian == null)
        {
            throw new InvalidOperationException("Veterinarian with this ID does not exist.");
        }

        if (veterinarian.WorkHours.All(wh => wh.Date != DateOnly.FromDateTime(request.Start)))
        {
            throw new InvalidOperationException("Invalid time selected");
        }
        
        var startHour = request.Start.Hour;
        var endHour = request.End.Hour;
        
        var workHours = veterinarian.WorkHours
            .Where(wh => wh.Date == DateOnly.FromDateTime(request.Start)).ToList();

        for (int i = startHour; i < endHour; i++)
        {
            if (workHours.All(wh => wh.Hour != i || wh.Taken))
            {
                throw new InvalidOperationException("Invalid time range selected");
            }
        }

        foreach (var workHour in workHours)
        {
            if (workHour.Hour >= startHour && workHour.Hour < endHour)
            {
                workHour.Taken = true;
            }
        }

        var visit = new animal_backend_domain.Entities.Visit
        {
            Id = Guid.NewGuid(),
            Type = request.Type,
            Start = startUtc,
            End = endUtc,
            Location = request.Location,
            ReminderSent = false,
            Price = (double)request.Price,
            VeterinarianId = request.VeterinarianUuid,
            UserId = request.UserUuid
        };
        
        dbContext.Visits.Add(visit);
        await dbContext.SaveChangesAsync(cancellationToken);

        return visit.Id;
    }
}