namespace animal_backend_api.Models;

public class CreateVaccineRequest
{
    public string? Name { get; set; }
    public string? Date { get; set; }
    public string? Description { get; set; }
    public string? Manufacturer { get; set; }
}
