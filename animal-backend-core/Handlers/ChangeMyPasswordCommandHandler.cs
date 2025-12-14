using animal_backend_core.Commands;
using animal_backend_core.Security;
using animal_backend_infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace animal_backend_core.Handlers;

public class ChangeMyPasswordCommandHandler(AnimalDbContext dbContext)
    : IRequestHandler<ChangeMyPasswordCommand, Unit>
{
    public async Task<Unit> Handle(ChangeMyPasswordCommand request, CancellationToken ct)
    {
        var user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, ct);

        if (user is null)
            throw new KeyNotFoundException("User not found.");

        var ok = PasswordHasher.VerifyFromStorage(request.CurrentPassword, user.Password);
        if (!ok)
            throw new InvalidOperationException("Current password is incorrect.");

        user.Password = PasswordHasher.HashForStorage(request.NewPassword);

        await dbContext.SaveChangesAsync(ct);
        return Unit.Value;
    }
}