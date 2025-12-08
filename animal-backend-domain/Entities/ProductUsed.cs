namespace animal_backend_domain.Entities;

public class ProductUsed : Entity
{
    public required double Dosage { get; set; }
    public required int TimesPerDay { get; set; }
    public Guid? ProductId { get; set; }
    public Product? Product { get; set; }
    public Guid AnimalId { get; set; }
    public Animal Animal { get; set; } = null!;
}