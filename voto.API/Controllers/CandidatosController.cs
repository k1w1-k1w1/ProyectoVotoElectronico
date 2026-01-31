using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoVotoElectronico;

namespace voto.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidatosController : ControllerBase
    {
        private readonly APIContext _context;

        public CandidatosController(APIContext context)
        {
            _context = context;
        }

        // GET: api/Candidatos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Candidato>>> GetCandidatos()
        {

            return await _context.Candidatos
                .Include(c => c.Eleccion)
                .AsNoTracking()
                .ToListAsync();
        }

        // GET: api/Candidatos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Candidato>> GetCandidato(int id)
        {
            var candidato = await _context.Candidatos
                .Include(c => c.Eleccion)
                .FirstOrDefaultAsync(c => c.IdCandidato == id);

            if (candidato == null) return NotFound();

            return candidato;
        }

        // POST: api/Candidatos
        [HttpPost]
        public async Task<ActionResult<Candidato>> PostCandidato(Candidato candidato)
        {
            try
            {
                candidato.Eleccion = null;

                _context.Candidatos.Add(candidato);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCandidato), new { id = candidato.IdCandidato }, candidato);
            }
            catch (Exception ex)
            {
                var mensajeError = ex.InnerException?.Message ?? ex.Message;
                Console.WriteLine("DEBUG BASE DE DATOS: " + mensajeError);
                return StatusCode(500, $"Error BD: {mensajeError}");
            }
        }

        // DELETE: api/Candidatos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCandidato(int id)
        {
            var candidato = await _context.Candidatos.FindAsync(id);
            if (candidato == null) return NotFound();

            var tieneVotos = await _context.Votos.AnyAsync(v => v.IdCandidato == id);
            if (tieneVotos) return BadRequest("No se puede eliminar un candidato con votos.");

            _context.Candidatos.Remove(candidato);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}