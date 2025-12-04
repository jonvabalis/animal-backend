namespace animal_backend_domain.Entities;

public abstract class Entity
{
    public Guid Id { get; set; } = Guid.NewGuid();
}