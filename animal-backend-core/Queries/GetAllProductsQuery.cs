using MediatR;
using System.Collections.Generic;
using animal_backend_domain.Dtos;

namespace animal_backend_core.Queries
{
    public record GetAllProductsQuery() : IRequest<List<ProductInfoDto>>;
}