namespace animal_backend_domain.Dtos;

public class CreateWorkday
{
	public Guid VeterinarianId { get; set; }
	public DateOnly Date { get; set; }
	public int StartHour { get; set; }
	public int EndHour { get; set; }
}