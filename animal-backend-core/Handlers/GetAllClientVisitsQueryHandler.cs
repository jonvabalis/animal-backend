using animal_backend_core.Queries;
using animal_backend_domain.Dtos;
using animal_backend_infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace animal_backend_core.Handlers;

public class GetAllClientVisitsQueryHandler(AnimalDbContext dbContext)
    : IRequestHandler<GetAllClientVisitsQuery, List<VisitInfoDto>>
{
    public async Task<List<VisitInfoDto>> Handle(GetAllClientVisitsQuery query, CancellationToken cancellationToken)
    {
        var visits = await dbContext.Visits.Where(v => v.UserId == query.id).ToListAsync(cancellationToken);
        return visits.Select(v => new VisitInfoDto
        {
            Id = v.Id,
            Type = v.Type,
            Start = v.Start,
            End = v.End,
            Location = v.Location,
            Price = v.Price,
            VeterinarianUuid = v.VeterinarianId
        }).ToList();
    }
}