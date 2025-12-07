using MediatR;
using animal_backend_domain.Dtos;
using animal_backend_core.Queries;

namespace animal_backend_core.Queries
{
    public record GetAllAnimalsQuery : IRequest<List<AnimalInfoDto>>;
}
