using System;
using animal_backend_infrastructure;
using MediatR;
using animal_backend_core.Commands;
using animal_backend_core.Security;
using animal_backend_domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace animal_backend_core.Handlers;

public class NotFoundException : Exception
{
	public NotFoundException(string message) : base(message)
	{
	}
}

public class CreateVeterinarianCommandHandler(AnimalDbContext dbContext)
	: IRequestHandler<CreateVeterinarianCommand, Guid>
{
	public async Task<Guid> Handle(CreateVeterinarianCommand request, CancellationToken cancellationToken)
	{
		var email = request.Email.Trim().ToLowerInvariant();

		var exists = await dbContext.Users.AnyAsync(
			u => u.Email.ToLower() == email,
			cancellationToken);

		if (exists)
			throw new InvalidOperationException("User with this email already exists.");
		
		var veterinarian = new Veterinarian
		{
			Id = Guid.NewGuid(),
			BirthDate = request.BirthDate,
			Rank = request.Rank,
			Responsibilities = request.Responsibilities,
			Education = request.Education,
			Salary = request.Salary,
			FullTime = request.FullTime,
			HireDate = request.HireDate,
			ExperienceYears = request.ExperienceYears,
			Gender = request.Gender
		};

		var user = new User
		{
			Id = Guid.NewGuid(),
			Name = request.Name,
			Surname = request.Surname,
			Email = request.Email,
			Password = PasswordHasher.HashForStorage(request.Password),
			Role = animal_backend_domain.Types.RoleType.Veterinarian,
			PhoneNumber = request.PhoneNumber,
			PhotoUrl = request.PhotoUrl,
			VeterinarianId = veterinarian.Id,
			Veterinarian = veterinarian
		};

		dbContext.Users.Add(user);

		await dbContext.SaveChangesAsync(cancellationToken);
		return user.Id;
	}
}