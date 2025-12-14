using animal_backend_domain.Types;
using MediatR;

namespace animal_backend_core.Commands
{
    public record CreateProductCommand(
        string Name,
        ProductType Type,
        string Description,
        string PhotoUrl,
        string Manufacturer
    ) : IRequest<Guid>;
}
