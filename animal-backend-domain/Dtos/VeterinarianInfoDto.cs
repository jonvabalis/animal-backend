using animal_backend_domain.Entities;
using animal_backend_domain.Types;
namespace animal_backend_domain.Dtos;

public class VeterinarianInfoDto : UserInfoDto
{
    public Guid Id { get; set; }
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