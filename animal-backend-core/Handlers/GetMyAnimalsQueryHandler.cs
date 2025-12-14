using animal_backend_core.Queries;
using animal_backend_domain.Dtos.Animals;
using animal_backend_infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace animal_backend_core.Handlers;

public class GetMyAnimalsQueryHandler(AnimalDbContext dbContext)
    : IRequestHandler<GetMyAnimalsQuery, List<AnimalDto>>
{
    public async Task<List<AnimalDto>> Handle(GetMyAnimalsQuery request, CancellationToken ct)
    {
        return await dbContext.Animals
            .AsNoTracking()
            .Where(a => a.UserId == request.UserId)
            .Select(a => new AnimalDto
            {
                Id = a.Id,
                Name = a.Name,
                Breed = a.Breed,
                Species = a.Species,
                SpeciesLatin = a.SpeciesLatin,
                Class = a.Class,
                PhotoUrl = a.PhotoUrl,
                DateOfBirth = a.DateOfBirth,
                Weight = a.Weight
            })
            .ToListAsync(ct);
    }
}