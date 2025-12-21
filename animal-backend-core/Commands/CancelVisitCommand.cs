using MediatR;

namespace animal_backend_core.Commands;

public record CancelVisitCommand(
	Guid Id
) : IRequest<Guid>;