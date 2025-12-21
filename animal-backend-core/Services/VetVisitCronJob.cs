using animal_backend_infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace animal_backend_core.Services;

public class VetVisitReminderCronJob(IServiceProvider serviceProvider) : BackgroundService
{
    private const int MinuteInterval = 2;
        private const int DaysAhead = 7;
        
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(MinuteInterval);
    
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CheckAndSendReminders();
                await Task.Delay(_interval, stoppingToken);
            }
        }
    
        private async Task CheckAndSendReminders()
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AnimalDbContext>();
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
            
            var tomorrow = DateTime.UtcNow.Date.AddDays(1);
            var endOfRange = tomorrow.AddDays(DaysAhead-1);
    
            var visitsToRemind = await context.Visits
                .Include(v => v.User)
                .Include(v => v.Veterinarian)
                .Where(v => v.Start >= tomorrow 
                         && v.Start < endOfRange 
                         && !v.ReminderSent)
                .ToListAsync();
    
            foreach (var visit in visitsToRemind)
            {
                try
                {
                    var veterinarian = await context.Users.FirstOrDefaultAsync(v => v.VeterinarianId == visit.VeterinarianId);
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
    
                    visit.ReminderSent = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send email for visit {visit.Id}: {ex.Message}");
                }
            }
    
            if (visitsToRemind.Any())
            {
                await context.SaveChangesAsync();
                Console.WriteLine($"[{DateTime.Now}] Sent {visitsToRemind.Count} reminders");
            }
        }
}