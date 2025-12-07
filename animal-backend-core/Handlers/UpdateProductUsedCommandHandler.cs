using MediatR;
using Microsoft.EntityFrameworkCore;
using animal_backend_domain.Dtos;
using animal_backend_infrastructure;
using animal_backend_core.Commands;
namespace animal_backend_core.Handlers;

public class UpdateProductUsedCommandHandler(AnimalDbContext dbContext)
    : IRequestHandler<UpdateProductUsedCommand, Unit>
{
    public async Task<Unit> Handle(UpdateProductUsedCommand request, CancellationToken cancellationToken)
    {
        var productUsed = await dbContext.ProductsUsed
            .FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);

        if (productUsed is null)
        {
            throw new KeyNotFoundException($"ProductUsed with ID {request.Id} not found");
        }

        productUsed.Dosage = request.Dosage;
        productUsed.TimesPerDay = request.TimesPerDay;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}