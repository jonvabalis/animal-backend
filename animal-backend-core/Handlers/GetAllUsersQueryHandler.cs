using animal_backend_core.Queries;
using animal_backend_domain.Dtos;
using animal_backend_infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace animal_backend_core.Handlers;

public class GetAllUsersQueryHandler(AnimalDbContext dbContext)
    : IRequestHandler<GetAllUsersQuery, List<UserInfoDto>>
{
    public async Task<List<UserInfoDto>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
    {
        var users = await dbContext.Users.ToListAsync(cancellationToken);

        return users.Select(u => new UserInfoDto
        {
            Id = u.Id,
            Name = u.Name,
            Surname = u.Surname,
            Email = u.Email,
            Password = u.Password,
            Role = u.Role,
            PhoneNumber = u.PhoneNumber,
            PhotoUrl = u.PhotoUrl
        }).ToList();
    }
}