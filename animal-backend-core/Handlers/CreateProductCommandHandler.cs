using animal_backend_infrastructure;
using MediatR;
using animal_backend_core.Commands;

namespace animal_backend_core.Handlers;

public class CreateProductCommandHandler(AnimalDbContext dbContext)
    : IRequestHandler<CreateProductCommand, Guid>
{
    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new animal_backend_domain.Entities.Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Type = request.Type,
            PhotoUrl = request.PhotoUrl,
            Description = request.Description,
            Manufacturer = request.Manufacturer
        };

        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}