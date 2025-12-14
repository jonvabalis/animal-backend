using MediatR;
using animal_backend_domain.Types;
namespace animal_backend_core.Commands
{
    public record UpdateVeterinarianCommand(
        Guid Id,
        DateTime BirthDate,
        string Rank,
        string Responsibilities,
        string Education,
        double Salary,
        double FullTime,
        DateTime HireDate,
        int ExperienceYears,
        GenderType Gender
    ) : IRequest<Unit>;
}