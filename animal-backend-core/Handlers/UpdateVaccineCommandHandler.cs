using MediatR;
using Microsoft.EntityFrameworkCore;
using animal_backend_infrastructure;
using animal_backend_core.Commands;
namespace animal_backend_core.Handlers;

public class UpdateVaccineCommandHandler(AnimalDbContext dbContext)
    : IRequestHandler<UpdateVaccineCommand, Unit>
{
    public async Task<Unit> Handle(UpdateVaccineCommand request, CancellationToken cancellationToken)
    {
        var vaccine = await dbContext.Vaccines
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (vaccine is null)
        {
            throw new KeyNotFoundException($"Vaccine with ID {request.Id} not found");
        }

        vaccine.Name = request.Name;
        vaccine.Date = request.Date;
        vaccine.Manufacturer = request.Manufacturer;
        vaccine.Description = request.Description;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}