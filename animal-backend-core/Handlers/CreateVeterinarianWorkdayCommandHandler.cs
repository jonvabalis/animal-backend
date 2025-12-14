using animal_backend_core.Commands;
using animal_backend_core.Security;
using animal_backend_domain.Entities;
using animal_backend_infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace animal_backend_core.Handlers;

public class CreateVeterinarianWorkdayCommandHandler(AnimalDbContext dbContext)
	: IRequestHandler<CreateVeterinarianWorkdayCommand, bool>
{
	public async Task<bool> Handle(CreateVeterinarianWorkdayCommand request, CancellationToken cancellationToken)
	{
		if (request.StartHour < 0 || request.StartHour > 23 ||
		    request.EndHour < 0 || request.EndHour > 24 ||
		    request.StartHour > request.EndHour)
		{
			throw new InvalidOperationException("Invalid start or end hour.");
		}
		
		var exists = await dbContext.Users.AnyAsync(
			u => u.VeterinarianId == request.VeterinarianId,
			cancellationToken);

		if (!exists)
			throw new InvalidOperationException("Veterinarian with this ID does not exist.");
		
		var veterinarian = await dbContext.Veterinarians.Include(v => v.WorkHours)
			.FirstOrDefaultAsync(v => v.Id == request.VeterinarianId, cancellationToken);
		
		if (veterinarian == null)
			throw new InvalidOperationException("Veterinarian with this ID does not exist.");
		
		if (veterinarian?.WorkHours != null)
		{
			if (veterinarian.WorkHours.Any(workhour => workhour.Date == request.Date))
			{
				throw new InvalidOperationException("Workday for this date already exists.");
			}
		}
		
		for (var i = request.StartHour; i <= request.EndHour; i++)
		{
			veterinarian?.WorkHours?.Add(new WorkHours
			{
				Date = request.Date,
				Hour = i,
				Taken = false,
				VeterinarianId = request.VeterinarianId,
				Veterinarian = veterinarian
			});
		}

		await dbContext.SaveChangesAsync(cancellationToken);
		
		return true;
	}
}