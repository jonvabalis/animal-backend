using animal_backend_infrastructure;
using MediatR;
using animal_backend_core.Commands;

namespace animal_backend_core.Handlers;

public class CreateDiseaseCommandHandler(AnimalDbContext dbContext)
    : IRequestHandler<CreateDiseaseCommand, Guid>
{
    public async Task<Guid> Handle(CreateDiseaseCommand request, CancellationToken cancellationToken)
    {
        var disease = new animal_backend_domain.Entities.Disease
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Category = request.Category,
            LatinName = request.LatinName,
            Description = request.Description,
        };

        dbContext.Diseases.Add(disease);
        await dbContext.SaveChangesAsync(cancellationToken);

        return disease.Id;
    }
}