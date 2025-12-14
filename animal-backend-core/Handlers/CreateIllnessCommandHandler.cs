using animal_backend_infrastructure;
using MediatR;
using animal_backend_core.Commands;
using animal_backend_domain.Entities;
using animal_backend_domain.Dtos;

namespace animal_backend_core.Handlers;

public class CreateIllnessCommandHandler(AnimalDbContext dbContext)
    : IRequestHandler<CreateIllnessCommand, Guid>
{
    public async Task<Guid> Handle(CreateIllnessCommand request, CancellationToken cancellationToken)
    {   
        var diseas = await dbContext.Diseases.FindAsync(new object?[] { request.DiseaseId }, cancellationToken); 
        var illness = new animal_backend_domain.Entities.Illness
        {
            AnimalId = request.AnimalId,
            Description = request.Description,
            Name = request.Name,
            DateDiagnosed = request.DateDiagnosed,
            DiseaseId = request.DiseaseId,
            Disease = diseas
        };

        dbContext.Illnesses.Add(illness);
        await dbContext.SaveChangesAsync(cancellationToken);

        return illness.Id;
    }
}