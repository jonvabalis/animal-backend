using animal_backend_infrastructure;
using MediatR;
using animal_backend_core.Queries;
using animal_backend_domain.Dtos;

namespace animal_backend_core.Handlers;

public class GetByIdVeterinarianQueryHandler(AnimalDbContext dbContext)
    : IRequestHandler<GetByIdVeterinarianQuery, VeterinarianInfoDto?>
{
    public async Task<VeterinarianInfoDto?> Handle(GetByIdVeterinarianQuery request, CancellationToken cancellationToken)
    {
        // TODO: adjust entity and mapping according to your domain model
        var veterinarian = await dbContext.Veterinarians.FindAsync([request.Id], cancellationToken);

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
            BirthDate = veterinarian.BirthDate,
            Rank = veterinarian.Rank,
            Responsibilities = veterinarian.Responsibilities,
            Education = veterinarian.Education,
            Salary = veterinarian.Salary,
            FullTime = veterinarian.FullTime,
            HireDate = veterinarian.HireDate,
            ExperienceYears = veterinarian.ExperienceYears,
            Gender = veterinarian.Gender
        };
    }
}