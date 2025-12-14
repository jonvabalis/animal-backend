using animal_backend_infrastructure;
using MediatR;
using animal_backend_core.Queries;
using animal_backend_domain.Dtos;

namespace animal_backend_core.Handlers;

public class GetByIdUserQueryHandler(AnimalDbContext dbContext)
    : IRequestHandler<GetByIdUserQuery, UserInfoDto?>
{
    public async Task<UserInfoDto?> Handle(GetByIdUserQuery request, CancellationToken cancellationToken)
    {
        // TODO: adjust entity and mapping according to your domain model
        var user = await dbContext.Users.FindAsync(new object[] { request.Id }, cancellationToken);

        if (user is null)
        {
            return null;
        }

        return new UserInfoDto
        {
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname,
            Email = user.Email,
            Password = user.Password,
            Role = user.Role,
            PhoneNumber = user.PhoneNumber,
            PhotoUrl = user.PhotoUrl
        };
    }
}