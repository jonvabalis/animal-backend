using animal_backend_infrastructure;
using MediatR;
using animal_backend_core.Queries;
using animal_backend_domain.Dtos;

namespace animal_backend_core.Handlers;

public class GetByIdVisitQueryHandler(AnimalDbContext dbContext)
    : IRequestHandler<GetByIdVisitQuery, VisitInfoDto>
{
    public async Task<VisitInfoDto> Handle(GetByIdVisitQuery request, CancellationToken cancellationToken)
    {
        // TODO: adjust entity and mapping according to your domain model
        var visit = await dbContext.Visits.FindAsync(new object[] { request.Id }, cancellationToken);

        if (visit is null)
        {
            return null!;
        }

        return new VisitInfoDto
        {
            Id = visit.Id,
            Type = visit.Type,
            Start = visit.Start,
            End = visit.End,
            Location = visit.Location,
            Price = visit.Price,
        };
    }
}