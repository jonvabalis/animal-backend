namespace animal_backend_domain.Dtos;

public class ConfirmEmailResponse
{
	public bool Success { get; set; }
	public string Message { get; set; }
	public bool AlreadyConfirmed { get; set; }
}