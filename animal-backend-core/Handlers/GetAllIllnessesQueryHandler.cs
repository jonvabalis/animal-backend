using animal_backend_core.Queries;
using animal_backend_domain.Dtos;
using animal_backend_infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace animal_backend_core.Handlers;

public class GetAllIllnessesQueryHandler(AnimalDbContext dbContext)
    : IRequestHandler<GetAllIllnessesQuery, List<IllnessInfoDto>>
{
    public async Task<List<IllnessInfoDto>> Handle(GetAllIllnessesQuery query, CancellationToken cancellationToken)
    {
        var illnesses = await dbContext.Illnesses.ToListAsync(cancellationToken);

        return illnesses.Select(i => new IllnessInfoDto
        {
            Id = i.Id,
            Name = i.Name,
            Description = i.Description,
            DateDiagnosed = i.DateDiagnosed,
        }).ToList();
    }
}