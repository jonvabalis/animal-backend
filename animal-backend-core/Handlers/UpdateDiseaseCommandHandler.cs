using MediatR;
using Microsoft.EntityFrameworkCore;
using animal_backend_domain.Dtos;
using animal_backend_infrastructure;
using animal_backend_core.Commands;
namespace animal_backend_core.Handlers;

public class UpdateDiseaseCommandHandler(AnimalDbContext dbContext)
    : IRequestHandler<UpdateDiseaseCommand, Unit>
{
    public async Task<Unit> Handle(UpdateDiseaseCommand request, CancellationToken cancellationToken)
    {
        var disease = await dbContext.Diseases
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (disease is null)
        {
            throw new KeyNotFoundException($"Disease with ID {request.Id} not found");
        }

        disease.Name = request.Name;
        disease.Category = request.Category;
        disease.LatinName = request.LatinName;
        disease.Description = request.Description;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}