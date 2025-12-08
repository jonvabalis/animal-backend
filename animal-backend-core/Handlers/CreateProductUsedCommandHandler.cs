using animal_backend_infrastructure;
using MediatR;
using animal_backend_core.Commands;

namespace animal_backend_core.Handlers;

public class CreateProductUsedCommandHandler(AnimalDbContext dbContext)
    : IRequestHandler<CreateProductUsedCommand, Guid>
{
    public async Task<Guid> Handle(CreateProductUsedCommand request, CancellationToken cancellationToken)
    {
        var productUsed = new animal_backend_domain.Entities.ProductUsed
        {
            Id = Guid.NewGuid(),
            Dosage = request.Dosage,
            TimesPerDay = request.TimesPerDay
        };

        dbContext.ProductsUsed.Add(productUsed);
        await dbContext.SaveChangesAsync(cancellationToken);

        return productUsed.Id;
    }
}