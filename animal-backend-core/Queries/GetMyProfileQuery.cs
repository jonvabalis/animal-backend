using animal_backend_domain.Dtos.Users;
using MediatR;

namespace animal_backend_core.Queries;

public record GetMyProfileQuery(Guid UserId) : IRequest<UserMeDto>;