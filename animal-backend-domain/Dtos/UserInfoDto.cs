using animal_backend_domain.Types;

namespace animal_backend_domain.Dtos;

public class UserInfoDto
{
    public Guid Id { get; set; }
	public required string Name { get; set; }
	public required string Surname { get; set; }
	public required string Email { get; set; }
	public required string Password { get; set; }
	public required RoleType Role { get; set; }
	public required string PhoneNumber { get; set; }
	public required string PhotoUrl { get; set; }

}