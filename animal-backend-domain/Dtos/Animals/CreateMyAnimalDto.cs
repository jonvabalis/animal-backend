using animal_backend_domain.Types;

namespace animal_backend_domain.Dtos.Animals;

public class CreateMyAnimalDto
{
    public string Name { get; set; } = null!;
    public string Breed { get; set; } = null!;
    public string Species { get; set; } = null!;
    public string SpeciesLatin { get; set; } = null!;
    public AnimalClass Class { get; set; }
    public string PhotoUrl { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public double Weight { get; set; }
}