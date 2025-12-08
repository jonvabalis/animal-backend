using animal_backend_infrastructure;
using MediatR;
using animal_backend_core.Commands;

namespace animal_backend_core.Handlers;

public class CreateVisitCommandHandler(AnimalDbContext dbContext)
    : IRequestHandler<CreateVisitCommand, Guid>
{
    public async Task<Guid> Handle(CreateVisitCommand request, CancellationToken cancellationToken)
    {
        var visit = new animal_backend_domain.Entities.Visit
        {
            Id = Guid.NewGuid(),
            Type = request.Type,
            Start = request.Start,
            End = request.End,
            Location = request.Location,
            Price = (double)request.Price
        };

        dbContext.Visits.Add(visit);
        await dbContext.SaveChangesAsync(cancellationToken);

        return visit.Id;
    }
}