using animal_backend_core.Queries;
using animal_backend_domain.Dtos;
using animal_backend_infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace animal_backend_core.Handlers;

public class GetAllAnimalsQueryHandler(AnimalDbContext dbContext)
    : IRequestHandler<GetAllAnimalsQuery, List<AnimalInfoDto>>
{
    public async Task<List<AnimalInfoDto>> Handle(GetAllAnimalsQuery query, CancellationToken cancellationToken)
    {
        var animals = await dbContext.Animals.ToListAsync(cancellationToken);

        return animals.Select(a => new AnimalInfoDto
        {
            Id = a.Id,
            Name = a.Name,
            Species = a.Species,
            SpeciesLatin = a.SpeciesLatin,
            Breed = a.Breed,
            DateOfBirth = a.DateOfBirth,
            Weight = a.Weight,
            PhotoUrl = a.PhotoUrl,
            Class = a.Class,
        }).ToList();
    }
}