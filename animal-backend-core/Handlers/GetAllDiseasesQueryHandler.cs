using animal_backend_core.Queries;
using animal_backend_domain.Dtos;
using animal_backend_infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace animal_backend_core.Handlers;

public class GetAllDiseasesQueryHandler(AnimalDbContext dbContext)
    : IRequestHandler<GetAllDiseasesQuery, List<DiseaseInfoDto>>
{
    public async Task<List<DiseaseInfoDto>> Handle(GetAllDiseasesQuery query, CancellationToken cancellationToken)
    {
        var diseases = await dbContext.Diseases.ToListAsync(cancellationToken);

        return diseases.Select(d => new DiseaseInfoDto
        {
            Id = d.Id,
            Name = d.Name,
            Category = d.Category,
            LatinName = d.LatinName,
            Description = d.Description,
        }).ToList();
    }
}