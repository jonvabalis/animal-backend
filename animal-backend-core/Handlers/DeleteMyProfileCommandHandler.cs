using animal_backend_core.Commands;
using animal_backend_infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace animal_backend_core.Handlers;

public class DeleteMyProfileCommandHandler(AnimalDbContext dbContext)
    : IRequestHandler<DeleteMyProfileCommand, Unit>
{
    public async Task<Unit> Handle(DeleteMyProfileCommand request, CancellationToken ct)
    {
        var user = await dbContext.Users
            .Include(u => u.Animals)
            .Include(u => u.Visits)
            .FirstOrDefaultAsync(u => u.Id == request.UserId, ct);

        if (user is null)
            throw new KeyNotFoundException("User not found.");

        if (user.Animals is { Count: > 0 })
            dbContext.Animals.RemoveRange(user.Animals);

        if (user.Visits is { Count: > 0 })
            dbContext.Visits.RemoveRange(user.Visits);

        dbContext.Users.Remove(user);
        await dbContext.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
