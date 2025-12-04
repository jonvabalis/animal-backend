using MediatR;

namespace animal_backend_core.Commands
{
    public record CreateProductCommand(
        string Name,
        string Type,
        string PhotoUrl,
        string Description,
        string Manufacturer
    ) : IRequest<int>;
}
