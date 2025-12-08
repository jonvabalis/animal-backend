namespace animal_backend_domain.Entities;

public class Illness : Entity //Sirgimas
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public DateTime DateDiagnosed { get; set; }

}