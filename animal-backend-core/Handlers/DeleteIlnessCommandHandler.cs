using MediatR;
using Microsoft.EntityFrameworkCore;
using animal_backend_infrastructure;
using animal_backend_core.Commands;
namespace animal_backend_core.Handlers;

public class DeleteIlnessCommandHandler(AnimalDbContext dbContext)
    : IRequestHandler<DeleteIlnessCommand, Unit>
{
    public async Task<Unit> Handle(DeleteIlnessCommand request, CancellationToken cancellationToken)
    {
        var ilness = await dbContext.Ilnesses
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (ilness is null)
        {
            throw new KeyNotFoundException($"Ilness with ID {request.Id} not found");
        }

        dbContext.Ilnesses.Remove(ilness);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}