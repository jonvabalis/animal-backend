using MediatR;
using animal_backend_domain.Types;

namespace animal_backend_core.Commands
{
    public record UpdateProductCommand(
        Guid Id,
        string Name,
        decimal Price,
        ProductType Type,
        string? Description,
        string Manufacturer,
        string PhotoUrl
    ) : IRequest<Unit>;
}