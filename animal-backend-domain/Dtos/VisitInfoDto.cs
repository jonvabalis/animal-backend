using animal_backend_domain.Types;

public class VisitInfoDto
{
    public Guid Id { get; set; }
    public VisitType Type { get; set; }
    public required DateTime Start { get; set; }
    public required DateTime End { get; set; }
    public required string Location { get; set; }
    public required double Price { get; set; }
}