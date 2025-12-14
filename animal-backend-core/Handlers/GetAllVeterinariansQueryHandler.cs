using animal_backend_core.Queries;
using animal_backend_domain.Dtos;
using animal_backend_infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace animal_backend_core.Handlers;

public class GetAllVeterinariansQueryHandler(AnimalDbContext dbContext)
    : IRequestHandler<GetAllVeterinariansQuery, List<VeterinarianInfoDto>>
{
    public async Task<List<VeterinarianInfoDto>> Handle(GetAllVeterinariansQuery query,
        CancellationToken cancellationToken)
    {
        var veterinarians = await dbContext.Users.Include(v => v.Veterinarian).ToListAsync(cancellationToken);
        return veterinarians.Select(v => new VeterinarianInfoDto
        {
            Name = v.Name,
            Surname = v.Surname,
            Email = v.Email,
            PhoneNumber = v.PhoneNumber,
            Password = v.Password,
            Role = v.Role,
            PhotoUrl = v.PhotoUrl,
            Id = v.Id,
            BirthDate = v.Veterinarian?.BirthDate ?? new DateTime(),
            Rank = v.Veterinarian != null ? v.Veterinarian.Rank : "",
            Responsibilities = v.Veterinarian != null ? v.Veterinarian.Responsibilities : "",
            Education = v.Veterinarian != null ? v.Veterinarian.Education : "",
            Salary = v.Veterinarian?.Salary ?? 0,
            FullTime = v.Veterinarian?.FullTime ?? 0,
            HireDate = v.Veterinarian?.HireDate ?? new DateTime(),
            ExperienceYears = v.Veterinarian?.ExperienceYears ?? 0,
            Gender = v.Veterinarian?.Gender ?? 0
        }).ToList();
    }
}