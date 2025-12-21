using animal_backend_infrastructure;
using MediatR;
using animal_backend_core.Commands;

namespace animal_backend_core.Handlers;

public class CreateVaccineCommandHandler(AnimalDbContext dbContext)
    : IRequestHandler<CreateVaccineCommand, Guid>
{
    public async Task<Guid> Handle(CreateVaccineCommand request, CancellationToken cancellationToken)
    {
        
        var vaccine = new animal_backend_domain.Entities.Vaccine
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Date = DateTime.SpecifyKind(request.Date, DateTimeKind.Utc),
            Description = request.Description,
            Manufacturer = request.Manufacturer,
            AnimalId = request.AnimalId
        };

        dbContext.Vaccines.Add(vaccine);
        await dbContext.SaveChangesAsync(cancellationToken);

        return vaccine.Id;
    }
}