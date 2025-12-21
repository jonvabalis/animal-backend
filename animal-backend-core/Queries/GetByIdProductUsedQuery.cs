using animal_backend_domain.Dtos;
using MediatR;

namespace animal_backend_core.Queries
{
    public record GetByIdProductUsedQuery(Guid AnimalId, Guid Id) : IRequest<ProductUsedInfoDto?>;
}
