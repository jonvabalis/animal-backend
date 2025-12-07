using MediatR;
using animal_backend_domain.Types;

namespace animal_backend_core.Commands
{
    public record UpdateVisitCommand(
        Guid Id,
        VisitType Type,
        DateTime Start,
        DateTime End,
        string Location,
        decimal Price
    ) : IRequest<Unit>;
}