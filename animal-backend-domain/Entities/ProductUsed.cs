using animal_backend_domain.Types;

namespace animal_backend_domain.Entities;

public class ProductUsed : Entity
{
    public required double Dosage { get; set; }
    public required int TimesPerDay { get; set; }

}