using animal_backend_infrastructure;
using MediatR;
using animal_backend_core.Queries;
using animal_backend_domain.Dtos;

namespace animal_backend_core.Handlers;

public class GetByIdVaccineQueryHandler(AnimalDbContext dbContext)
    : IRequestHandler<GetByIdVaccineQuery, VaccineInfoDto?>
{
    public async Task<VaccineInfoDto?> Handle(GetByIdVaccineQuery request, CancellationToken cancellationToken)
    {
        // TODO: adjust entity and mapping according to your domain model
        var vaccine = await dbContext.Vaccines.FindAsync(new object[] { request.Id }, cancellationToken);

        if (vaccine is null)
        {
            return null!;
        }

        return new VaccineInfoDto
        {
            Id = vaccine.Id,
            Name = vaccine.Name,
            Category = vaccine.Category,
            LatinName = vaccine.LatinName,
            Description = vaccine.Description,
        };
    }
}