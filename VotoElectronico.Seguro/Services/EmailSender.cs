using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace VotoElectronico.Seguro.Services
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                // CONFIGURACIÓN DE TU CUENTA
                var correoEmisor = "votoelectronicoutn@gmail.com";
                var claveAplicacion = "tanp bcqm ihix pqnd";

                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(correoEmisor, claveAplicacion),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(correoEmisor, "Sistema de Voto Electrónico"),
                    Subject = subject,
                    Body = htmlMessage,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(email);

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Render bloqueó el correo, pero el usuario ya fue creado.");
            }
        }
    }
}
