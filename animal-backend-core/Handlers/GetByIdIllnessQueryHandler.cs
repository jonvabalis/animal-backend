using animal_backend_infrastructure;
using MediatR;
using animal_backend_core.Queries;
using animal_backend_domain.Dtos;

namespace animal_backend_core.Handlers;

public class GetByIdIllnessQueryHandler(AnimalDbContext dbContext)
    : IRequestHandler<GetByIdIllnessQuery, IllnessInfoDto?>
{
    public async Task<IllnessInfoDto?> Handle(GetByIdIllnessQuery request, CancellationToken cancellationToken)
    {
        // TODO: adjust entity and mapping according to your domain model
        var illness = await dbContext.Illnesses.FindAsync([request.Id], cancellationToken);

        if (illness is null)
        {
            return null;
        }

        return new IllnessInfoDto
        {
            AnimalId = illness.AnimalId,
            Id = illness.Id,
            Name = illness.Name,
            Description = illness.Description,
            DateDiagnosed = illness.DateDiagnosed,
            DiseaseId = illness.DiseaseId
        };
    }
}