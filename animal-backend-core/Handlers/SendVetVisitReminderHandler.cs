using animal_backend_core.Commands;
using animal_backend_core.Services;
using animal_backend_domain.Dtos;
using animal_backend_infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace animal_backend_core.Handlers;

public class SendVetVisitReminderHandler(IEmailService emailService, AnimalDbContext context)
	: IRequestHandler<SendVetVisitReminderCommand, SendVetVisitReminderResponse>
{
	public async Task<SendVetVisitReminderResponse> Handle(SendVetVisitReminderCommand request, CancellationToken cancellationToken)
	{
		try
		{
			var visit = await context.Visits
				.Include(v => v.User)
				.Include(v => v.Veterinarian)
				.FirstOrDefaultAsync(v => v.Id == request.VisitId, cancellationToken);

			if (visit == null)
			{
				return new SendVetVisitReminderResponse
				{
					Success = false,
					Message = "Visit not found",
					VisitId = request.VisitId
				};
			}
			
			var veterinarian = await context.Users.FirstOrDefaultAsync(v => v.VeterinarianId == visit.VeterinarianId, cancellationToken);
			if (veterinarian == null)
			{
				throw new Exception("Veterinarian not found");
			}

			await emailService.SendVetReminderAsync(
				visit.User.Email,
				visit.User.Name,
				visit.Location,
				visit.Start.Date,
				$"{visit.Start.Hour}h - {visit.End.Hour}h",
				$"{visit.Veterinarian.Responsibilities} {veterinarian.Name} {veterinarian.Surname}"
			);

			return new SendVetVisitReminderResponse
			{
				Success = true,
				Message = "Reminder sent successfully!",
				RecipientEmail = visit.User.Email,
				VisitId = request.VisitId
			};
		}
		catch (Exception ex)
		{
			return new SendVetVisitReminderResponse
			{
				Success = false,
				Message = $"Failed to send reminder: {ex.Message}",
				VisitId = request.VisitId
			};
		}
	}
}