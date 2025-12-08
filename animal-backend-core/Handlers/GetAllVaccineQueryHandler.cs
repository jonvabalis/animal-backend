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
        var vaccines = await dbContext.Vaccines.ToListAsync(cancellationToken);
        return vaccines.Select(v => new VaccineInfoDto
        {
            Id = v.Id,
            Name = v.Name,
            Category = v.Category,
            LatinName = v.LatinName,
            Description = v.Description,
        }).ToList();
    }
}