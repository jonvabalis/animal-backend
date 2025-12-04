using animal_backend_domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace animal_backend_infrastructure;

public class AnimalDbContext(DbContextOptions<AnimalDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
}