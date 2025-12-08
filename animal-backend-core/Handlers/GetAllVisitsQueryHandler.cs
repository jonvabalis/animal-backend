using animal_backend_core.Queries;
using animal_backend_domain.Dtos;
using animal_backend_infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace animal_backend_core.Handlers;

public class GetAllVisitsQueryHandler(AnimalDbContext dbContext)
    : IRequestHandler<GetAllVisitsQuery, List<VisitInfoDto>>
{
    public async Task<List<VisitInfoDto>> Handle(GetAllVisitsQuery query, CancellationToken cancellationToken)
    {
        var visits = await dbContext.Visits.ToListAsync(cancellationToken);

        return visits.Select(v => new VisitInfoDto
        {
            Id = v.Id,
            Type = v.Type,
            Start = v.Start,
            End = v.End,
            Location = v.Location,
            Price = v.Price,
        }).ToList();
    }
}