using MediatR;
using animal_backend_domain.Dtos;

namespace animal_backend_core.Queries
{
    public record GetAllUsersQuery : IRequest<List<UserInfoDto>>;
}