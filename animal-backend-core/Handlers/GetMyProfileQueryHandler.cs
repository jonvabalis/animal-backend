using animal_backend_core.Queries;
using animal_backend_domain.Dtos.Users;
using animal_backend_infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace animal_backend_core.Handlers;

public class GetMyProfileQueryHandler(AnimalDbContext dbContext)
    : IRequestHandler<GetMyProfileQuery, UserMeDto>
{
    public async Task<UserMeDto> Handle(GetMyProfileQuery request, CancellationToken ct)
    {
        var user = await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == request.UserId, ct);

        if (user is null)
            throw new KeyNotFoundException("User not found.");

        return new UserMeDto
        {
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            PhotoUrl = user.PhotoUrl,
            Role = user.Role
        };
    }
}