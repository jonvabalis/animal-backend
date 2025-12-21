using animal_backend_core.Queries;
using animal_backend_domain.Dtos.Animals;
using animal_backend_domain.Dtos;
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
                Weight = a.Weight,
                Illnesses = a.Illnesses != null ? a.Illnesses.Select(i => new IllnessInfoDto
                {
                    Id = i.Id,
                    Name = i.Name,
                    Description = i.Description,
                    DateDiagnosed = i.DateDiagnosed,
                    AnimalId = a.Id,
                    DiseaseId = i.DiseaseId
                }).ToList() : new List<IllnessInfoDto>(),
                Vaccinations = a.Vaccines != null ? a.Vaccines.Select(v => new VaccineInfoDto
                {
                    Id = v.Id,
                    Name = v.Name,
                    Date = v.Date,
                    Description = v.Description,
                    Manufacturer = v.Manufacturer,
                    AnimalId = a.Id
                }).ToList() : new List<VaccineInfoDto>(),
                ProductsUsed = a.ProductsUsed != null ? a.ProductsUsed.Select(pu => new ProductUsedInfoDto
                {
                    Id = pu.Id,
                    Dosage = pu.Dosage,
                    TimesPerDay = pu.TimesPerDay,
                    AnimalId = a.Id,
                    ProductId = pu.ProductId
                }).ToList() : new List<ProductUsedInfoDto>()
            })
            .ToListAsync(ct);
    }
}