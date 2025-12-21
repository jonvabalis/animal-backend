namespace animal_backend_domain.Dtos;

public class SendVetVisitReminderResponse
{
	public bool Success { get; set; }
	public string Message { get; set; }
	public string RecipientEmail { get; set; }
	public Guid VisitId { get; set; }
}