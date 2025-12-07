namespace animal_backend_domain.Dtos;
public class IlnessInfoDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public DateTime DateDiagnosed { get; set; }

}