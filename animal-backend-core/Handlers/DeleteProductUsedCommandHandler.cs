using MediatR;
using Microsoft.EntityFrameworkCore;
using animal_backend_infrastructure;
using animal_backend_core.Commands;
namespace animal_backend_core.Handlers;

public class DeleteProductUsedCommandHandler(AnimalDbContext dbContext)
    : IRequestHandler<DeleteProductUsedCommand, Unit>
{
    public async Task<Unit> Handle(DeleteProductUsedCommand request, CancellationToken cancellationToken)
    {
        var productUsed = await dbContext.ProductsUsed
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (productUsed is null)
        {
            throw new KeyNotFoundException($"ProductUsed with ID {request.Id} not found");
        }

        dbContext.ProductsUsed.Remove(productUsed);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}