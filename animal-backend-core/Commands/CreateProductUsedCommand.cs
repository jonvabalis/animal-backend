using MediatR;

namespace animal_backend_core.Commands
{
    public record CreateProductUsedCommand(
        double Dosage,
        int TimesPerDay
    ) : IRequest<Guid>;
}
