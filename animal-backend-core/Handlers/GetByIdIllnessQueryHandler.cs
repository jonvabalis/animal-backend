using animal_backend_infrastructure;
using MediatR;
using animal_backend_core.Queries;
using animal_backend_domain.Dtos;
using Microsoft.EntityFrameworkCore;
namespace animal_backend_core.Handlers;

public class GetByIdIllnessQueryHandler(AnimalDbContext dbContext)
    : IRequestHandler<GetByIdIllnessQuery, IllnessInfoDto?>
{
    public async Task<IllnessInfoDto?> Handle(GetByIdIllnessQuery request, CancellationToken cancellationToken)
    {
        var illness = await dbContext.Illnesses.Where(i => i.AnimalId == request.animalId && i.Id == request.id).FirstOrDefaultAsync(cancellationToken);

        if (illness is null)
        {
            return null;
        }

        return new IllnessInfoDto
        {
            Id = illness.Id,
            Name = illness.Name,
            Description = illness.Description,
            DateDiagnosed = illness.DateDiagnosed,
            DiseaseId = illness.DiseaseId
        };
    }
}