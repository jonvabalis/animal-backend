namespace animal_backend_domain.Entities;

public class WorkHours
{
	public DateOnly Date { get; set; }
	public int Hour { get; set; }
	public bool Taken { get; set; }
	public Guid VeterinarianId { get; set; }
	public Veterinarian Veterinarian { get; set; } = null!;
}