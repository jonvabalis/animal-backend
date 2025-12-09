using System;
using animal_backend_infrastructure;
using MediatR;
using animal_backend_core.Commands;

namespace animal_backend_core.Handlers;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}

public class CreateVeterinarianCommandHandler(AnimalDbContext dbContext)
    : IRequestHandler<CreateVeterinarianCommand, Guid>
{
    public async Task<Guid> Handle(CreateVeterinarianCommand request, CancellationToken cancellationToken)
    {

        var veterinarian = new animal_backend_domain.Entities.Veterinarian
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Surname = request.Surname,
            Email = request.Email,
            Password = request.Password,
            Role = animal_backend_domain.Types.RoleType.Veterinarian,
            PhoneNumber = request.PhoneNumber,
            PhotoUrl = request.PhotoUrl,

            BirthDate = request.BirthDate,
            Rank = request.Rank,
            Responsibilities = request.Responsibilities,
            Education = request.Education,
            Salary = request.Salary,
            FullTime = request.FullTime,
            HireDate = request.HireDate,
            ExperienceYears = request.ExperienceYears,
            Gender = request.Gender
        };

        dbContext.Veterinarians.Add(veterinarian);

        await dbContext.SaveChangesAsync(cancellationToken);
        return veterinarian.Id;
    }
}