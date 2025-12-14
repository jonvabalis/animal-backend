using MediatR;
using animal_backend_domain.Dtos;

namespace animal_backend_core.Queries
{
    public record GetAllIllnessesQuery(Guid? AnimalId = null) : IRequest<List<IllnessInfoDto>>;
}