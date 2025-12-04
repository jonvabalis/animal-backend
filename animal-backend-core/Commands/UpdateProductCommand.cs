using MediatR;

namespace animal_backend_core.Commands
{
    public record UpdateProductCommand(
        Guid Id,
        string Name,
        decimal Price,
        string? Description
    ) : IRequest<bool>;
}