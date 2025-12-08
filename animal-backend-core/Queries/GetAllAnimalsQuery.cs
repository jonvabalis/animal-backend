using MediatR;
using animal_backend_domain.Dtos;

namespace animal_backend_core.Queries
{
    public record GetAllAnimalsQuery : IRequest<List<AnimalInfoDto>>;
}
