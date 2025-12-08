using MediatR;

namespace animal_backend_core.Commands
{
    public record CreateProductUsedCommand(
        Guid ProductId,
        double Dosage,
        int TimesPerDay
    ) : IRequest<Guid>;
}
