using animal_backend_domain.Types;
using animal_backend_domain.Dtos;

namespace animal_backend_domain.Dtos.Animals;

public class AnimalDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Breed { get; set; } = null!;
    public string Species { get; set; } = null!;
    public string SpeciesLatin { get; set; } = null!;
    public AnimalClass Class { get; set; }
    public string PhotoUrl { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public double Weight { get; set; }

    // Related collections so frontend can display illnesses, vaccines and used products
    public List<IllnessInfoDto>? Illnesses { get; set; }
    public List<VaccineInfoDto>? Vaccinations { get; set; }
    public List<ProductUsedInfoDto>? ProductsUsed { get; set; }
}