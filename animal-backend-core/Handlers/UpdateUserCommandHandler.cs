using MediatR;
using Microsoft.EntityFrameworkCore;
using animal_backend_infrastructure;
using animal_backend_core.Commands;
namespace animal_backend_core.Handlers;

public class UpdateUserCommandHandler(AnimalDbContext dbContext)
    : IRequestHandler<UpdateUserCommand, Unit>
{
    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);

        if (user is null)
        {
            throw new KeyNotFoundException($"User with ID {request.Id} not found");
        }

        user.Name = request.Name;
        user.Surname = request.Surname;
        user.Email = request.Email;
        user.Password = request.Password;
        user.Role = request.Role;
        user.PhoneNumber = request.PhoneNumber;
        user.PhotoUrl = request.PhotoUrl;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}