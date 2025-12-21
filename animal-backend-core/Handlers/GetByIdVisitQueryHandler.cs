using animal_backend_infrastructure;
using MediatR;
using animal_backend_core.Queries;
using animal_backend_domain.Dtos;

namespace animal_backend_core.Handlers;

public class GetByIdVisitQueryHandler(AnimalDbContext dbContext)
    : IRequestHandler<GetByIdVisitQuery, VisitInfoDto?>
{
    public async Task<VisitInfoDto?> Handle(GetByIdVisitQuery request, CancellationToken cancellationToken)
    {
        var visit = await dbContext.Visits.FindAsync([request.Id], cancellationToken);

        if (visit is null)
        {
            return null;
        }

        return new VisitInfoDto
        {
            Id = visit.Id,
            Type = visit.Type,
            Start = visit.Start,
            End = visit.End,
            Location = visit.Location,
            Price = visit.Price,
            VeterinarianUuid = visit.VeterinarianId
        };
    }
}