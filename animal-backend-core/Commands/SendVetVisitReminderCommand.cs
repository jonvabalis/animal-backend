using animal_backend_domain.Dtos;
using MediatR;

namespace animal_backend_core.Commands;

public class SendVetVisitReminderCommand : IRequest<SendVetVisitReminderResponse>
{
	public Guid VisitId { get; set; }
}