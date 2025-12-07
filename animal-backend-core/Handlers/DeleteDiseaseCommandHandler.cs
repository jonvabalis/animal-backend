using MediatR;
using Microsoft.EntityFrameworkCore;
using animal_backend_infrastructure;
using animal_backend_core.Commands;
namespace animal_backend_core.Handlers;

public class DeleteDiseaseCommandHandler(AnimalDbContext dbContext)
    : IRequestHandler<DeleteDiseaseCommand, Unit>
{
    public async Task<Unit> Handle(DeleteDiseaseCommand request, CancellationToken cancellationToken)
    {
        var disease = await dbContext.Diseases
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (disease is null)
        {
            throw new KeyNotFoundException($"Disease with ID {request.Id} not found");
        }

        dbContext.Diseases.Remove(disease);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}