namespace animal_backend_domain.Entities;

public class WorkHours
{
	public DateOnly Date { get; set; }
	public TimeOnly StartTime { get; set; }
	public TimeOnly EndTime { get; set; }
	public Guid VeterinarianId { get; set; }
	public Veterinarian Veterinarian { get; set; } = null!;
}