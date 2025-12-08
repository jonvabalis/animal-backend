namespace animal_backend_domain.Dtos;

public class VaccineInfoDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public DateTime Date { get; set; }
    public required string Description { get; set; }
    public required string Manufacturer { get; set; }
}