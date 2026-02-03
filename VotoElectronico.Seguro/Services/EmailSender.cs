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

            // Importante: usar el correo que verificaste en la imagen anterior
            var from = new EmailAddress("patricioquiguango.08@gmail.com", "Sistema de Voto Electronico");
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);

            try
            {
                var response = await client.SendEmailAsync(msg);
                Console.WriteLine($"SendGrid Response: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error de envío API: {ex.Message}");
            }
        }


        //
        //
    }
}