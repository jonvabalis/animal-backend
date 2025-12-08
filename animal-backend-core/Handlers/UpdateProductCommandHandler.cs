using MediatR;
using Microsoft.EntityFrameworkCore;
using animal_backend_infrastructure;
using animal_backend_core.Commands;
namespace animal_backend_core.Handlers;

public class UpdateProductCommandHandler(AnimalDbContext dbContext)
    : IRequestHandler<UpdateProductCommand, Unit>
{
    public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (product is null)
        {
            throw new KeyNotFoundException($"Product with ID {request.Id} not found");
        }

        product.Name = request.Name;
        product.Type = request.Type;
        product.PhotoUrl = request.PhotoUrl;
        product.Manufacturer = request.Manufacturer;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}