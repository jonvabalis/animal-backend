using animal_backend_domain.Types;
using MediatR;

namespace animal_backend_core.Commands
{
    public record CreateVisitCommand(
        VisitType Type,
        DateTime Start,
        DateTime End,
        string Location,
        decimal Price
    ) : IRequest<Guid>;
}
