using MediatR;
using Microsoft.EntityFrameworkCore;
using animal_backend_infrastructure;
using animal_backend_core.Commands;
namespace animal_backend_core.Handlers;

public class DeleteUserCommandHandler(AnimalDbContext dbContext)
    : IRequestHandler<DeleteUserCommand, Unit>
{
    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (user is null)
        {
            throw new KeyNotFoundException($"User with ID {request.Id} not found");
        }

        dbContext.Users.Remove(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}