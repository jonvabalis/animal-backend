using animal_backend_core.Queries;
using animal_backend_domain.Dtos;
using animal_backend_infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace animal_backend_core.Handlers;

public class GetAllIlnessesQueryHandler(AnimalDbContext dbContext)
    : IRequestHandler<GetAllIlnessesQuery, List<IlnessInfoDto>>
{
    public async Task<List<IlnessInfoDto>> Handle(GetAllIlnessesQuery query, CancellationToken cancellationToken)
    {
        var ilnesses = await dbContext.Ilnesses.ToListAsync(cancellationToken);

        return ilnesses.Select(i => new IlnessInfoDto
        {
            Id = i.Id,
            Name = i.Name,
            Description = i.Description,
            DateDiagnosed = i.DateDiagnosed,
        }).ToList();
    }
}