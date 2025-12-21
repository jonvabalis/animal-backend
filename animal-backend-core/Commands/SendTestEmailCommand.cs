using animal_backend_domain.Dtos;
using MediatR;

namespace animal_backend_core.Commands;

public class SendTestEmailCommand : IRequest<SendTestEmailResponse>
{
	public string Email { get; set; }
	public string PetName { get; set; }
}