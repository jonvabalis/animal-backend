using animal_backend_infrastructure;
using MediatR;
using animal_backend_core.Queries;
using animal_backend_domain.Dtos;
using Microsoft.EntityFrameworkCore;
namespace animal_backend_core.Handlers;

public class GetByIdProductUsedQueryHandler(AnimalDbContext dbContext)
    : IRequestHandler<GetByIdProductUsedQuery, ProductUsedInfoDto?>
{
    public async Task<ProductUsedInfoDto?> Handle(GetByIdProductUsedQuery request, CancellationToken cancellationToken)
    {

        var productUsed = await dbContext.ProductsUsed.Where(p => p.AnimalId == request.AnimalId)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (productUsed is null)
        {
            return null;
        }

        return new ProductUsedInfoDto
        {
            Id = productUsed.Id,
            Dosage = productUsed.Dosage,
            TimesPerDay = productUsed.TimesPerDay,
            ProductId = productUsed.ProductId
        };
    }
}