using animal_backend_core.Queries;
using animal_backend_domain.Dtos;
using animal_backend_infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace animal_backend_core.Handlers;

public class GetAllProductsQueryHandler(AnimalDbContext dbContext)
    : IRequestHandler<GetAllProductsQuery, List<ProductInfoDto>>
{
    public async Task<List<ProductInfoDto>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await dbContext.Products.ToListAsync(cancellationToken);

        return products.Select(p => new ProductInfoDto
        {
            Id = p.Id,
            Name = p.Name,
            Type = p.Type,
            PhotoUrl = p.PhotoUrl,
            Manufacturer = p.Manufacturer,
        }).ToList();
    }
}