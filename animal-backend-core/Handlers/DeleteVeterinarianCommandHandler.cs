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
        // Find the user with this veterinarian ID
        var user = await dbContext.Users
            .Include(u => u.Veterinarian)
            .FirstOrDefaultAsync(u => u.VeterinarianId == request.Id, cancellationToken);

        if (user is null)
        {
            throw new KeyNotFoundException($"Veterinarian with ID {request.Id} not found");
        }

        // Remove both user and veterinarian (cascade will handle veterinarian if configured)
        dbContext.Users.Remove(user);
        if (user.Veterinarian != null)
        {
            dbContext.Veterinarians.Remove(user.Veterinarian);
        }
        
        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}