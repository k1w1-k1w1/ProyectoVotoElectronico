using System.Net;
using System.Net.Mail;

public class EmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public void EnviarCodigo(string destinatario, string codigo)
    {
        var smtp = new SmtpClient
        {
            Host = _config["Smtp:Host"],
            Port = int.Parse(_config["Smtp:Port"]),
            EnableSsl = true,
            Credentials = new NetworkCredential(
                _config["Smtp:Email"],
                _config["Smtp:Password"]
            )
        };

        var mensaje = new MailMessage
        {
            From = new MailAddress(_config["Smtp:Email"], "Sistema de Voto Electrónico"),
            Subject = "Código de verificación",
            Body = $"Tu código de verificación es: {codigo}",
            IsBodyHtml = false
        };

        mensaje.To.Add(destinatario);

        smtp.Send(mensaje);
    }
}
