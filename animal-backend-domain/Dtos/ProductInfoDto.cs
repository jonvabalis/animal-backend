using animal_backend_domain.Types;

namespace animal_backend_domain.Dtos;

public class ProductInfoDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public ProductType Type { get; set; }
    public required string PhotoUrl { get; set; }

    public required string Description { get; set; }
    public required string Manufacturer { get; set; }
}