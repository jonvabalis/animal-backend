using animal_backend_infrastructure;
using MediatR;
using animal_backend_core.Queries;
using animal_backend_domain.Dtos;

namespace animal_backend_core.Handlers;

public class GetByIdDiseaseQueryHandler(AnimalDbContext dbContext)
    : IRequestHandler<GetByIdDiseaseQuery, DiseaseInfoDto?>
{
    public async Task<DiseaseInfoDto?> Handle(GetByIdDiseaseQuery request, CancellationToken cancellationToken)
    {
        // TODO: adjust entity and mapping according to your domain model
        var disease = await dbContext.Diseases.FindAsync(new object[] { request.Id }, cancellationToken);

        if (disease is null)
        {
            return null!;
        }

        return new DiseaseInfoDto
        {
            Id = disease.Id,
            Name = disease.Name,
            Category = disease.Category,
            LatinName = disease.LatinName,
            Description = disease.Description,
        };
    }
}