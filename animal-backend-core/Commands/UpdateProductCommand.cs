using MediatR;
using animal_backend_domain.Types;

namespace animal_backend_core.Commands
{
    public record UpdateProductCommand(
        Guid Id,
        string Name,
        ProductType Type,
        string? Description,
        string PhotoUrl,
        string Manufacturer
    ) : IRequest<Unit>;
}