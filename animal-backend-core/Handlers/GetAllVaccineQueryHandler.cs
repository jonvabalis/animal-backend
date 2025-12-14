using animal_backend_core.Queries;
using animal_backend_domain.Dtos;
using animal_backend_infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace animal_backend_core.Handlers;

public class GetAllVaccinesQueryHandler(AnimalDbContext dbContext)
    : IRequestHandler<GetAllVaccinesQuery, List<VaccineInfoDto>>
{
    public async Task<List<VaccineInfoDto>> Handle(GetAllVaccinesQuery query, CancellationToken cancellationToken)
    {
        var q = dbContext.Vaccines.AsQueryable();

        if (query.AnimalId.HasValue)
        {
            q = q.Where(v => v.AnimalId == query.AnimalId.Value);
        }

        var vaccines = await q.ToListAsync(cancellationToken);

        return vaccines.Select(v => new VaccineInfoDto
        {
            Id = v.Id,
            Name = v.Name,
            Date = v.Date,
            Manufacturer = v.Manufacturer,
            Description = v.Description,
            AnimalId = v.AnimalId
        }).ToList();
    }
}