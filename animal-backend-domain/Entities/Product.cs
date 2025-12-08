using animal_backend_domain.Types;

namespace animal_backend_domain.Entities;

public class Product : Entity
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public ProductType Type { get; set; }
    public required string PhotoUrl { get; set; }
    public required string Manufacturer { get; set; }
    public List<ProductUsed>? ProductsUsed { get; set; }
}