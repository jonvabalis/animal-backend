using animal_backend_infrastructure;
using MediatR;
using animal_backend_core.Queries;
using animal_backend_domain.Dtos;

namespace animal_backend_core.Handlers;

public class GetByIdProductQueryHandler(AnimalDbContext dbContext)
    : IRequestHandler<GetByIdProductQuery, ProductInfoDto>
{
    public async Task<ProductInfoDto> Handle(GetByIdProductQuery request, CancellationToken cancellationToken)
    {
        // TODO: adjust entity and mapping according to your domain model
        var product = await dbContext.Products.FindAsync(new object[] { request.Id }, cancellationToken);

        if (product is null)
        {
            return null!;
        }

        return new ProductInfoDto
        {
            Id = product.Id,
            Name = product.Name,
            Type = product.Type,
            PhotoUrl = product.PhotoUrl,
            Description = product.Description,
            Manufacturer = product.Manufacturer,
        };
    }
}