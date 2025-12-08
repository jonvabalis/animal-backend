using animal_backend_domain.Types;
namespace animal_backend_domain.Entities;

public class Disease : Entity
{
    public required string Name { get; set; }
    public required string LatinName { get; set; }
    public DiseaseCategory Category { get; set; }
    public required string Description { get; set; }
    public List<Illness>? Illnesses { get; set; }
}