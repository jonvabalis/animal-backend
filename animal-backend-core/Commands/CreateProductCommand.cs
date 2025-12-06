using animal_backend_domain.Types;
using MediatR;

namespace animal_backend_core.Commands
{
    public record CreateProductCommand(
        string Name,
        ProductType Type,
        string PhotoUrl,
        string Description,
        string Manufacturer
    ) : IRequest<Guid>;
}
