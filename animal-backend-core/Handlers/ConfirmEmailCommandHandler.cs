using animal_backend_core.Commands;
using animal_backend_domain.Dtos;
using animal_backend_infrastructure;
using MediatR;

namespace animal_backend_core.Handlers;

public class ConfirmEmailCommandHandler(AnimalDbContext dbContext)
	: IRequestHandler<ConfirmEmailCommand, ConfirmEmailResponse>
{
	public async Task<ConfirmEmailResponse> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
	{
		var user = await dbContext.Users.FindAsync([request.UserId], cancellationToken);

		if (user == null)
		{
			return new ConfirmEmailResponse
			{
				Success = false,
				Message = "User not found",
				AlreadyConfirmed = false
			};
		}

		if (user.Confirmed)
		{
			return new ConfirmEmailResponse
			{
				Success = true,
				Message = "Email already confirmed",
				AlreadyConfirmed = true
			};
		}

		user.Confirmed = true;
		await dbContext.SaveChangesAsync(cancellationToken);

		return new ConfirmEmailResponse
		{
			Success = true,
			Message = "Email confirmed successfully",
			AlreadyConfirmed = false
		};
	}
}