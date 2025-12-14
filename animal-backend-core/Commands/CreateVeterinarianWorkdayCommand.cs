using animal_backend_domain.Dtos;
using MediatR;

namespace animal_backend_core.Commands;

public record CreateVeterinarianWorkdayCommand(Guid VeterinarianId, DateOnly Date, int StartHour, int EndHour) : IRequest<bool>;
