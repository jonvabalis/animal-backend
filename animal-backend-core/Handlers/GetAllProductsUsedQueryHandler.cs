using animal_backend_core.Queries;
using animal_backend_domain.Dtos;
using animal_backend_infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace animal_backend_core.Handlers;

public class GetAllProductsUsedQueryHandler(AnimalDbContext dbContext)
    : IRequestHandler<GetAllProductsUsedQuery, List<ProductUsedInfoDto>>
{
    public async Task<List<ProductUsedInfoDto>> Handle(GetAllProductsUsedQuery query, CancellationToken cancellationToken)
    {
        var q = dbContext.ProductsUsed.AsQueryable();

        if (query.AnimalId.HasValue)
            q = q.Where(p => p.AnimalId == query.AnimalId.Value);

        var productsUsed = await q.ToListAsync(cancellationToken);

        return productsUsed.Select(p => new ProductUsedInfoDto
        {
            AnimalId = p.AnimalId,
            Id = p.Id,
            Dosage = p.Dosage,
            TimesPerDay = p.TimesPerDay,
        }).ToList();
    }
}