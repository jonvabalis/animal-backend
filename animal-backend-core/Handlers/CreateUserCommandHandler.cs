using animal_backend_infrastructure;
using MediatR;
using animal_backend_core.Commands;

namespace animal_backend_core.Handlers;

public class CreateUserCommandHandler(AnimalDbContext dbContext)
    : IRequestHandler<CreateUserCommand, Guid>
{
    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new animal_backend_domain.Entities.User
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Surname = request.Surname,
            Email = request.Email,
            Password = request.Password,
            Role = request.Role,
            PhoneNumber = request.PhoneNumber,
            PhotoUrl = request.PhotoUrl
        };

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}