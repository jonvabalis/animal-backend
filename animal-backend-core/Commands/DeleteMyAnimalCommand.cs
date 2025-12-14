using MediatR;

namespace animal_backend_core.Commands;

public record DeleteMyAnimalCommand(Guid UserId, Guid AnimalId) : IRequest<Unit>;
