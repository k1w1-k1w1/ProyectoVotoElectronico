using Microsoft.AspNetCore.Mvc;
using VotoElectronico.Api.Models;

namespace voto.API.Controllers
{
    [ApiController]
    [Route("api/votantes")]
    public class VotantesController : ControllerBase
    {
        private readonly APIContext _context;

        public VotantesController(APIContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Votante votante)
        {
            _context.Votantes.Add(votante);
            await _context.SaveChangesAsync();
            return Ok(votante);
        }
    }
}