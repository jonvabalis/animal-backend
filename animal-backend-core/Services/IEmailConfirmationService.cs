using System.Net;
using System.Net.Mail;
using animal_backend_domain.Dtos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace animal_backend_core.Services;

public interface IEmailConfirmationService
{
    Task SendConfirmationEmailAsync(string email, string name, Guid userId);
}

public class EmailConfirmationService(IOptions<EmailSettings> emailSettings, IConfiguration configuration)
    : IEmailConfirmationService
{
    private readonly EmailSettings _emailSettings = emailSettings.Value;

    public async Task SendConfirmationEmailAsync(string email, string name, Guid userId)
    {
        var backendUrl = configuration["BackendUrl"] ?? "https://localhost:7102";
        var confirmationLink = $"{backendUrl}/api/auth/confirm-email/{userId}";

        var smtpClient = new SmtpClient(_emailSettings.SmtpHost)
        {
            Port = _emailSettings.SmtpPort,
            Credentials = new NetworkCredential(_emailSettings.SmtpUser, _emailSettings.SmtpPassword),
            EnableSsl = true,
        };

        string htmlBody = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <style>
                    body {{ font-family: Arial, sans-serif; }}
                    .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                    .header {{ background-color: #4CAF50; color: white; padding: 20px; text-align: center; border-radius: 5px; }}
                    .content {{ background: #f5f5f5; padding: 30px; margin-top: 20px; border-radius: 5px; }}
                    .button {{ 
                        display: inline-block; 
                        padding: 15px 30px; 
                        background-color: #4CAF50; 
                        color: white; 
                        text-decoration: none; 
                        border-radius: 5px; 
                        margin-top: 20px;
                        font-weight: bold;
                    }}
                    .footer {{ text-align: center; margin-top: 20px; color: #666; font-size: 12px; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1>Sveiki, {name}!</h1>
                    </div>
                    <div class='content'>
                        <h2>Patvirtinkite savo el. paštą</h2>
                        <p>Ačiū, kad užsiregistravote VetKlinika sistemoje!</p>
                        <p>Norėdami užbaigti registraciją, prašome patvirtinti savo el. pašto adresą:</p>
                        <div style='text-align: center;'>
                            <a href='{confirmationLink}' class='button'>Patvirtinti el. paštą</a>
                        </div>
                    </div>
                    <div class='footer'>
                        <p>Su pagarba, VetKlinika komanda</p>
                    </div>
                </div>
            </body>
            </html>
        ";

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_emailSettings.SmtpUser, "VetKlinika"),
            Subject = "Patvirtinkite savo el. paštą",
            Body = htmlBody,
            IsBodyHtml = true,
        };

        mailMessage.To.Add(email);

        await smtpClient.SendMailAsync(mailMessage);
    }
}