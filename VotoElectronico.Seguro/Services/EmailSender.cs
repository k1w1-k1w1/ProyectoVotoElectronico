using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace VotoElectronico.Seguro.Services
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);

            var from = new EmailAddress("patricioquiguango.08@gmail.com", "Sistema de Voto UTN");
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);

            try
            {
                var response = await client.SendEmailAsync(msg);
                Console.WriteLine($"SendGrid respondió con: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar con SendGrid: {ex.Message}");
            }
        }
    }
}