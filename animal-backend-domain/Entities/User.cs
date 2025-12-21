using animal_backend_domain.Types;

namespace animal_backend_domain.Entities;

public class User : Entity
{
	public required string Name { get; set; }
	public required string Surname { get; set; }
	public required string Email { get; set; }
	public required string Password { get; set; }
	public required RoleType Role { get; set; }
	public required string PhoneNumber { get; set; }
	public required string PhotoUrl { get; set; }
	public required bool Confirmed { get; set; }
	public List<Animal> Animals { get; set; } = [];
	public List<Visit> Visits { get; set; } = [];
	public Guid? VeterinarianId { get; set; }
	public Veterinarian? Veterinarian { get; set; }
}