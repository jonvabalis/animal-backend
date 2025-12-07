using animal_backend_domain.Types;

namespace animal_backend_domain.Dtos;

public class DiseaseInfoDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public DiseaseCategory Category { get; set; }
    public required string LatinName { get; set; }

    public required string Description { get; set; }
}