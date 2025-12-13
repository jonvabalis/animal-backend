using animal_backend_domain.Types;

namespace animal_backend_domain.Dtos.Users;

public class UserMeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string PhotoUrl { get; set; } = null!;
    public RoleType Role { get; set; }
}