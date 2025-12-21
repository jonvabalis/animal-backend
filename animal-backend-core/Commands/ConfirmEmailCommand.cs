using animal_backend_domain.Dtos;
using MediatR;

namespace animal_backend_core.Commands;

public class ConfirmEmailCommand : IRequest<ConfirmEmailResponse>
{
	public Guid UserId { get; set; }
}