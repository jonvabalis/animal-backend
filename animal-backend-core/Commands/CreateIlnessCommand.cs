using animal_backend_domain.Types;
using MediatR;

namespace animal_backend_core.Commands
{
    public record CreateIlnessCommand(
        string Name,
        string Description,
        DateTime DateDiagnosed
    ) : IRequest<Guid>;
}
