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
            // Reemplaza esto con la clave que copiaste de SendGrid
            var apiKey = "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";
            var client = new SendGridClient(apiKey);

            // El emisor debe ser el mismo que verificaste en SendGrid
            var from = new EmailAddress("patricioquiguango.08@gmail.com", "Sistema de Voto Electrónico");
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);

            try
            {
                var response = await client.SendEmailAsync(msg);

                Console.WriteLine($"Status de SendGrid: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error crítico de SendGrid: {ex.Message}");
            }
        }
    }
}