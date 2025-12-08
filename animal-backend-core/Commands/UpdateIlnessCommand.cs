using MediatR;
using animal_backend_domain.Types;

namespace animal_backend_core.Commands
{
    public record UpdateIlnessCommand(
        Guid Id,
        string Name,
        string Description,
        DateTime DateDiagnosed
    ) : IRequest<Unit>;
}