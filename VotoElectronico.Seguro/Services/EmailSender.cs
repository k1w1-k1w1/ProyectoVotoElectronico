using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace VotoElectronico.Seguro.Services
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");

            if (string.IsNullOrEmpty(apiKey))
            {
                Console.WriteLine("ERROR: La variable de entorno SENDGRID_API_KEY no está configurada en Render.");
                return;
            }

            var client = new SendGridClient(apiKey);

            var from = new EmailAddress("patricioquiguango.08@gmail.com", "Sistema de Voto Electrónico UTN");
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);

            try
            {
                var response = await client.SendEmailAsync(msg);

                Console.WriteLine($"[EmailSender] Estado de respuesta SendGrid: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EmailSender] Error crítico al enviar correo: {ex.Message}");
            }
        }
    }
}