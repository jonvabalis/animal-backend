using MediatR;
using Microsoft.EntityFrameworkCore;
using animal_backend_domain.Dtos;
using animal_backend_infrastructure;
using animal_backend_core.Commands;
namespace animal_backend_core.Handlers;

public class UpdateIlnessCommandHandler(AnimalDbContext dbContext)
    : IRequestHandler<UpdateIlnessCommand, Unit>
{
    public async Task<Unit> Handle(UpdateIlnessCommand request, CancellationToken cancellationToken)
    {
        var ilness = await dbContext.Ilnesses
            .FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);

        if (ilness is null)
        {
            throw new KeyNotFoundException($"Ilness with ID {request.Id} not found");
        }

        ilness.Description = request.Description;
        ilness.Name = request.Name;
        ilness.DateDiagnosed = request.DateDiagnosed;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}