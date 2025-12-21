using MediatR;
using Microsoft.EntityFrameworkCore;
using animal_backend_infrastructure;
using animal_backend_core.Commands;
namespace animal_backend_core.Handlers;

public class UpdateIllnessCommandHandler(AnimalDbContext dbContext)
    : IRequestHandler<UpdateIllnessCommand, Unit>
{
    public async Task<Unit> Handle(UpdateIllnessCommand request, CancellationToken cancellationToken)
    {
        var illness = await dbContext.Illnesses
            .FirstOrDefaultAsync(v => v.Id == request.Id && v.AnimalId == request.AnimalId, cancellationToken);

        if (illness is null)
        {
            throw new KeyNotFoundException($"Illness with ID {request.Id} not found");
        }

        illness.Description = request.Description;
        illness.Name = request.Name;
        illness.DateDiagnosed = DateTime.SpecifyKind(request.DateDiagnosed, DateTimeKind.Utc);
        illness.DiseaseId = request.DiseaseId;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}