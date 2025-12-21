using animal_backend_domain.Types;

namespace animal_backend_domain.Entities;

public class Visit : Entity
{
    public VisitType Type { get; set; }
    public required DateTime Start { get; set; }
    public required DateTime End { get; set; }
    public required string Location { get; set; }
    public required double Price { get; set; }
    public required bool ReminderSent { get; set; }
    public Guid VeterinarianId { get; set; }
    public Veterinarian Veterinarian { get; set; } = null!;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}