using animal_backend_domain.Dtos;
using MediatR;

namespace animal_backend_core.Queries
{
    public record GetByIdVisitQuery(Guid Id) : IRequest<VisitInfoDto?>;
}
