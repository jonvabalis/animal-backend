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
    public DbSet<Ilness> Ilnesses { get; set; }
}