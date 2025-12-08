using animal_backend_infrastructure;
using MediatR;
using animal_backend_core.Commands;

namespace animal_backend_core.Handlers;

public class CreateIlnessCommandHandler(AnimalDbContext dbContext)
    : IRequestHandler<CreateIlnessCommand, Guid>
{
    public async Task<Guid> Handle(CreateIlnessCommand request, CancellationToken cancellationToken)
    {
        var ilness = new animal_backend_domain.Entities.Ilness
        {
            Description = request.Description,
            Name = request.Name,
            DateDiagnosed = request.DateDiagnosed
        };

        dbContext.Ilnesses.Add(ilness);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ilness.Id;
    }
}