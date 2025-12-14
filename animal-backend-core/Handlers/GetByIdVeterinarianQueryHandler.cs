using animal_backend_infrastructure;
using MediatR;
using animal_backend_core.Queries;
using animal_backend_domain.Dtos;
using Microsoft.EntityFrameworkCore;

namespace animal_backend_core.Handlers;

public class GetByIdVeterinarianQueryHandler(AnimalDbContext dbContext)
    : IRequestHandler<GetByIdVeterinarianQuery, VeterinarianInfoDto?>
{
    public async Task<VeterinarianInfoDto?> Handle(GetByIdVeterinarianQuery request, CancellationToken cancellationToken)
    {
        // TODO: adjust entity and mapping according to your domain model
        var veterinarian = await dbContext.Users
            .Include(v => v.Veterinarian)
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

        if (veterinarian is null)
        {
            return null!;
        }

        return new VeterinarianInfoDto
        {
            Name = veterinarian.Name,
            Surname = veterinarian.Surname,
            Email = veterinarian.Email,
            Password = veterinarian.Password,
            Role = veterinarian.Role,
            PhoneNumber = veterinarian.PhoneNumber,
            PhotoUrl = veterinarian.PhotoUrl,
            Id = veterinarian.Id,
            BirthDate = veterinarian.Veterinarian?.BirthDate ?? default,
            Rank = veterinarian.Veterinarian?.Rank ?? "",
            Responsibilities = veterinarian.Veterinarian?.Responsibilities ?? "",
            Education = veterinarian.Veterinarian?.Education ?? "",
            Salary = veterinarian.Veterinarian?.Salary ?? 0,
            FullTime = veterinarian.Veterinarian?.FullTime ?? 0,
            HireDate = veterinarian.Veterinarian?.HireDate ?? default,
            ExperienceYears = veterinarian.Veterinarian?.ExperienceYears ?? 0,
            Gender = veterinarian.Veterinarian?.Gender ?? 0
        };

    }
}