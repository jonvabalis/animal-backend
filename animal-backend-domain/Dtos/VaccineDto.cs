using animal_backend_domain.Types;

namespace animal_backend_domain.Dtos;

public class VaccineInfoDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public VaccineCategory Category { get; set; }
    public required string LatinName { get; set; }

    public required string Description { get; set; }
}