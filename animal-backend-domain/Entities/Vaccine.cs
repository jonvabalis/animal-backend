using animal_backend_domain.Types;
namespace animal_backend_domain.Entities;

public class Vaccine : Entity
{   
    public required string Name { get; set; }
    public required string LatinName { get; set; }
    public VaccineCategory Category { get; set; }
    public required string Description { get; set; }
}