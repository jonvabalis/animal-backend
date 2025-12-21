using animal_backend_infrastructure;
using MediatR;
using animal_backend_core.Queries;
using Microsoft.EntityFrameworkCore;
using animal_backend_domain.Dtos;

namespace animal_backend_core.Handlers;

public class GetByIdVaccineQueryHandler(AnimalDbContext dbContext)
    : IRequestHandler<GetByIdVaccineQuery, VaccineInfoDto?>
{
    public async Task<VaccineInfoDto?> Handle(GetByIdVaccineQuery request, CancellationToken cancellationToken)
    {
        var vaccine = await dbContext.Vaccines
            .Where(v => v.AnimalId == request.animalId && v.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (vaccine is null)
        {
            return null!;
        }

        return new VaccineInfoDto
        {
            Id = vaccine.Id,
            Name = vaccine.Name,
            Date = vaccine.Date,
            Manufacturer = vaccine.Manufacturer,
            Description = vaccine.Description,
        };
    }
}