using animal_backend_core.Commands;
using animal_backend_core.Services;
using animal_backend_domain.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace animal_backend_core.Handlers;

public class SendTestEmailHandler(IEmailService emailService)
	: IRequestHandler<SendTestEmailCommand, SendTestEmailResponse>
{
	public async Task<SendTestEmailResponse> Handle(SendTestEmailCommand request, CancellationToken cancellationToken)
	{
		try
		{
			await emailService.SendVetReminderAsync(
				request.Email,
				request.PetName,
				"Majamis",
				DateTime.Now.AddDays(1),
				$"{17}h - {19}h",
				"Veterinaras"
			);

			return new SendTestEmailResponse
			{
				Success = true,
				Message = "Test email sent successfully!",
				RecipientEmail = request.Email
			};
		}
		catch (Exception ex)
		{
			return new SendTestEmailResponse
			{
				Success = false,
				Message = $"Failed to send email: {ex.Message}",
				RecipientEmail = request.Email
			};
		}
	}
}
