using animal_backend_core.Queries;
using animal_backend_domain.Dtos;
using animal_backend_infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace animal_backend_core.Handlers;

public class GetAllVeterinariansQueryHandler(AnimalDbContext dbContext)
    : IRequestHandler<GetAllVeterinariansQuery, List<VeterinarianInfoDto>>
{
    public async Task<List<VeterinarianInfoDto>> Handle(GetAllVeterinariansQuery query, CancellationToken cancellationToken)
    {
        var veterinarians = await dbContext.Veterinarians.ToListAsync(cancellationToken);
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
            BirthDate = v.BirthDate,
            Rank = v.Rank,
            Responsibilities = v.Responsibilities,
            Education = v.Education,
            Salary = v.Salary,
            FullTime = v.FullTime,
            HireDate = v.HireDate,
            ExperienceYears = v.ExperienceYears,
            Gender = v.Gender
        }).ToList();
    }
}