namespace animal_backend_domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Type { get; set; } // Vėliau bus enum
    public string PhotoUrl { get; set; }
    public string Manufacturer { get; set; }
}