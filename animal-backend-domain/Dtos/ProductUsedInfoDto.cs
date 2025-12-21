
namespace animal_backend_domain.Dtos;

public class ProductUsedInfoDto
{
    public Guid Id { get; set; }
    public required double Dosage { get; set; }
    public required int TimesPerDay { get; set; }

    public Guid AnimalId { get; set; }
    public Guid? ProductId { get; set; }

}