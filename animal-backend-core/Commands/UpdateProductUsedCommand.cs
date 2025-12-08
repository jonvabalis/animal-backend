using MediatR;

namespace animal_backend_core.Commands
{
    public record UpdateProductUsedCommand(
        Guid Id,
        double Dosage,
        int TimesPerDay
    ) : IRequest<Unit>;
}