using animal_backend_infrastructure;
using MediatR;
using animal_backend_core.Queries;
using animal_backend_domain.Dtos;

namespace animal_backend_core.Handlers;

public class GetByIdIlnessQueryHandler(AnimalDbContext dbContext)
    : IRequestHandler<GetByIdIlnessQuery, IlnessInfoDto>
{
    public async Task<IlnessInfoDto> Handle(GetByIdIlnessQuery request, CancellationToken cancellationToken)
    {
        // TODO: adjust entity and mapping according to your domain model
        var ilness = await dbContext.Ilnesses.FindAsync(new object[] { request.Id }, cancellationToken);

        if (ilness is null)
        {
            return null!;
        }

        return new IlnessInfoDto
        {
            Id = ilness.Id,
            Name = ilness.Name,
            Description = ilness.Description,
            DateDiagnosed = ilness.DateDiagnosed,
        };
    }
}