namespace animal_backend_domain.Dtos;

public class SendTestEmailResponse
{
	public bool Success { get; set; }
	public string Message { get; set; }
	public string RecipientEmail { get; set; }
}