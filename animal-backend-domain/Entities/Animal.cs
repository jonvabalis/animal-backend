using animal_backend_domain.Types;

namespace animal_backend_domain.Entities;

public class Animal : Entity
{
    public required string Name { get; set; }
    public required string Breed { get; set; } // pvz. Labrodoro retriveris
    public required string Species { get; set; } // rūsis, pvz. šuo, katė
    public required string SpeciesLatin { get; set; } // pvz. šuo - Canis lupus familiaris
    public AnimalClass Class { get; set; }
    public required string PhotoUrl { get; set; }
    public DateTime DateOfBirth { get; set; }
    public required double Weight { get; set; }
    public List<Illness>? Illnesses { get; set; }
    public List<ProductUsed>? ProductsUsed { get; set; }
    public List<Vaccine>? Vaccines { get; set; }
    
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}