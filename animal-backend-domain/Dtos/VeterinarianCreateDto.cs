using animal_backend_domain.Entities;
using animal_backend_domain.Types;
namespace animal_backend_domain.Dtos;

public class VeterinarianCreateDto
{
	public required string Name { get; set; }
	public required string Surname { get; set; }
	public required string Email { get; set; }
	public required string Password { get; set; }
	public required RoleType Role { get; set; }
	public required string PhoneNumber { get; set; }
	public required string PhotoUrl { get; set; }
	public DateTime BirthDate { get; set; }
	public required string Rank { get; set; }
	public required string Responsibilities { get; set; }
	public required string Education { get; set; }
	public required double Salary { get; set; }
	public required double FullTime { get; set; }
	public DateTime HireDate { get; set; }
	public int ExperienceYears { get; set; }	
	public required GenderType Gender { get; set; }
}