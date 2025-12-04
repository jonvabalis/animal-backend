using MediatR;


namespace animal_backend_core.Commands
{
    public record DeleteProductCommand(Guid Id) : IRequest<bool>;
}