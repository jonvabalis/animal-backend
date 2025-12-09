using MediatR;
using animal_backend_domain.Dtos;
using animal_backend_domain.Types;
namespace animal_backend_core.Commands
{
    public record CreateUserCommand(
        string Name,
        string Surname,
        string Email,
        string Password,
        RoleType Role,
        string PhoneNumber,
        string PhotoUrl
    ) : IRequest<Guid>;
}