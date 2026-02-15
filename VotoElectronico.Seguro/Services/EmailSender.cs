using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace VotoElectronico.Seguro.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY")
                ?? _configuration["SendGrid:ApiKey"];

            var fromEmail = _configuration["SendGrid:FromEmail"]
                ?? Environment.GetEnvironmentVariable("SENDGRID_FROM_EMAIL")
                ?? "no-reply@votoelectronico.local";

            var fromName = _configuration["SendGrid:FromName"]
                ?? Environment.GetEnvironmentVariable("SENDGRID_FROM_NAME")
                ?? "Sistema de Voto Electrónico";

            if (string.IsNullOrEmpty(apiKey))
            {
                Console.WriteLine("ERROR: No se encontró la API Key de SendGrid (SENDGRID_API_KEY o SendGrid:ApiKey).");
                return;
            }

            var client = new SendGridClient(apiKey);

            var from = new EmailAddress(fromEmail, fromName);
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);

            try
            {
                var response = await client.SendEmailAsync(msg);

                var body = await response.Body.ReadAsStringAsync();

                Console.WriteLine($"[EmailSender] Status: {response.StatusCode}");
                Console.WriteLine($"[EmailSender] Body: {body}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EmailSender] Error crítico al enviar correo: {ex.Message}");
            }

        }
    }
}
