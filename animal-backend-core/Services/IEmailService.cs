using System.Net;
using System.Net.Mail;
using animal_backend_domain.Dtos;

using Microsoft.Extensions.Options;

namespace animal_backend_core.Services;

public interface IEmailService
{
	Task SendVetReminderAsync(string toEmail, string user, string place, DateTime visitDate, string range, string veterinarianName);
}

public class ZohoEmailService(IOptions<EmailSettings> emailSettings) : IEmailService
{
    private readonly EmailSettings _emailSettings = emailSettings.Value;

    public async Task SendVetReminderAsync(string toEmail, string user, string place, DateTime visitDate, string range, string veterinarianName)
    {
        try
        {
            var smtpClient = new SmtpClient(_emailSettings.SmtpHost)
            {
                Port = _emailSettings.SmtpPort,
                Credentials = new NetworkCredential(_emailSettings.SmtpUser, _emailSettings.SmtpPassword),
                EnableSsl = true,
            };
            
            string htmlBody = $@"
            <!DOCTYPE html>
            <html>
            <body>
                <div style='font-family: Arial; max-width: 600px; margin: 0 auto;'>
                        <h2 style='color: #4CAF50;'>🐾 Upcoming Veterinarian Visit</h2>
                        <div style='background: #f5f5f5; padding: 20px; border-radius: 5px;'>
                            <p><strong>User:</strong> {user}</p>
                            <p><strong>Veterinarian:</strong> {veterinarianName}</p>
                            <p><strong>Place:</strong> {place}</p>
                            <p><strong>Date:</strong> {visitDate:MMMM dd, yyyy}</p>
                            <p><strong>Time:</strong> {range}</p>
                        </div>
                        <p>Please arrive 10 minutes early. See you soon!</p>
                    </div>
            </body>
            </html>
        ";
            
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.SmtpUser, "VetKlinika"),
                Subject = "Priminimas: Vizitas pas veterinarą",
                Body = htmlBody,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to send email: {ex.Message}", ex);
        }
    }
}