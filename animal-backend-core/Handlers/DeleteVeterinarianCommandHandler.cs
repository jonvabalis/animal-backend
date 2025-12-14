using MediatR;
using Microsoft.EntityFrameworkCore;
using animal_backend_infrastructure;
using animal_backend_core.Commands;
namespace animal_backend_core.Handlers;

public class DeleteVeterinarianCommandHandler(AnimalDbContext dbContext)
    : IRequestHandler<DeleteVeterinarianCommand, Unit>
{
    public async Task<Unit> Handle(DeleteVeterinarianCommand request, CancellationToken cancellationToken)
    {
        var veterinarian = await dbContext.Veterinarians
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (veterinarian is null)
        {
            throw new KeyNotFoundException($"Veterinarian with ID {request.Id} not found");
        }

        dbContext.Veterinarians.Remove(veterinarian);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}