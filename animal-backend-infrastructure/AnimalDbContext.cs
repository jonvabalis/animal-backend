using animal_backend_domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace animal_backend_infrastructure;

public class AnimalDbContext(DbContextOptions<AnimalDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Vaccine> Vaccines { get; set; }
    public DbSet<Disease> Diseases { get; set; }
    public DbSet<Animal> Animals { get; set; }
    public DbSet<Visit> Visits { get; set; }
    public DbSet<ProductUsed> ProductsUsed { get; set; }
    public DbSet<Illness> Illnesses { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Veterinarian> Veterinarians { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WorkHours>()
            .HasKey(wh => new { wh.VeterinarianId, wh.Date });
        
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<Veterinarian>().ToTable("Veterinarians");
    }
}