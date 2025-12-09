using MediatR;
using Microsoft.EntityFrameworkCore;
using animal_backend_infrastructure;
using animal_backend_core.Commands;
namespace animal_backend_core.Handlers;

public class UpdateVeterinarianCommandHandler(AnimalDbContext dbContext)
    : IRequestHandler<UpdateVeterinarianCommand, Unit>
{
    public async Task<Unit> Handle(UpdateVeterinarianCommand request, CancellationToken cancellationToken)
    {
        var veterinarian = await dbContext.Veterinarians
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (veterinarian is null)
        {
            throw new KeyNotFoundException($"Veterinarian with ID {request.Id} not found");
        }
        
        veterinarian.BirthDate = request.BirthDate;
        veterinarian.Rank = request.Rank;
        veterinarian.Responsibilities = request.Responsibilities;
        veterinarian.Education = request.Education;
        veterinarian.Salary = request.Salary;
        veterinarian.FullTime = request.FullTime;
        veterinarian.HireDate = request.HireDate;
        veterinarian.ExperienceYears = request.ExperienceYears;
        veterinarian.Gender = request.Gender;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}