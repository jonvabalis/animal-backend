namespace animal_backend_domain.Dtos;

public class ProductInfoDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; } // Vėliau bus enum
    public string PhotoUrl { get; set; }
    public string Manufacturer { get; set; }
}