using animal_backend_core.Commands;
using animal_backend_infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace animal_backend_core.Handlers;

public class UpdateMyProfileCommandHandler(AnimalDbContext dbContext)
    : IRequestHandler<UpdateMyProfileCommand, Unit>
{
    public async Task<Unit> Handle(UpdateMyProfileCommand request, CancellationToken ct)
    {
        var user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, ct);

        if (user is null)
            throw new KeyNotFoundException("User not found.");

        user.Name = request.Name;
        user.Surname = request.Surname;
        user.PhoneNumber = request.PhoneNumber;
        user.PhotoUrl = request.PhotoUrl;

        await dbContext.SaveChangesAsync(ct);
        return Unit.Value;
    }
}