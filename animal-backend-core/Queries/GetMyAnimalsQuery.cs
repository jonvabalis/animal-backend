using animal_backend_domain.Dtos.Animals;
using MediatR;

namespace animal_backend_core.Queries;

public record GetMyAnimalsQuery(Guid UserId) : IRequest<List<AnimalDto>>;
