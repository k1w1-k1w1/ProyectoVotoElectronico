using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace VotoElectronico.Seguro.Controllers
{
    [ApiController]
    [Route("api/test-email")]
    public class TestEmailController : ControllerBase
    {
        private readonly IEmailSender _emailSender;

        public TestEmailController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        [HttpGet]
        public async Task<IActionResult> SendTestEmail()
        {
            await _emailSender.SendEmailAsync(
                "patricioquiguango.08@gmail.com",
                "Prueba de correo",
                "<h2>EmailSender funcionando correctamente 🚀</h2>"
            );

            return Ok("Correo enviado (revisa tu inbox o spam)");
        }
    }
}
