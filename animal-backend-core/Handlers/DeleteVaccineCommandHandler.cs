using MediatR;
using Microsoft.EntityFrameworkCore;
using animal_backend_infrastructure;
using animal_backend_core.Commands;
namespace animal_backend_core.Handlers;

public class DeleteVaccineCommandHandler(AnimalDbContext dbContext)
    : IRequestHandler<DeleteVaccineCommand, Unit>
{
    public async Task<Unit> Handle(DeleteVaccineCommand request, CancellationToken cancellationToken)
    {
        var vaccine = await dbContext.Vaccines
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (vaccine is null)
        {
            throw new KeyNotFoundException($"Vaccine with ID {request.Id} not found");
        }

        dbContext.Vaccines.Remove(vaccine);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}