using animal_backend_infrastructure;
using MediatR;
using animal_backend_core.Queries;
using animal_backend_domain.Dtos;

namespace animal_backend_core.Handlers;

public class GetByIdProductUsedQueryHandler(AnimalDbContext dbContext)
    : IRequestHandler<GetByIdProductUsedQuery, ProductUsedInfoDto>
{
    public async Task<ProductUsedInfoDto> Handle(GetByIdProductUsedQuery request, CancellationToken cancellationToken)
    {
        // TODO: adjust entity and mapping according to your domain model
        var productUsed = await dbContext.ProductsUsed.FindAsync(new object[] { request.Id }, cancellationToken);

        if (productUsed is null)
        {
            return null!;
        }

        return new ProductUsedInfoDto
        {
            Id = productUsed.Id,
            Dosage = productUsed.Dosage,
            TimesPerDay = productUsed.TimesPerDay,
        };
    }
}