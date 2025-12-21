using animal_backend_infrastructure;
using MediatR;
using animal_backend_core.Commands;

namespace animal_backend_core.Handlers;

public class CreateIllnessCommandHandler(AnimalDbContext dbContext)
    : IRequestHandler<CreateIllnessCommand, Guid>
{
    public async Task<Guid> Handle(CreateIllnessCommand request, CancellationToken cancellationToken)
    {
        var illness = new animal_backend_domain.Entities.Illness
        {
            Description = request.Description,
            Name = request.Name,
            DateDiagnosed = DateTime.SpecifyKind(request.DateDiagnosed, DateTimeKind.Utc),
            AnimalId = request.AnimalId,
            DiseaseId = request.DiseaseId
        };

        dbContext.Illnesses.Add(illness);
        await dbContext.SaveChangesAsync(cancellationToken);

        return illness.Id;
    }
}