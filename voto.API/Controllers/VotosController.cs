using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoVotoElectronico;
using voto;

namespace voto.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VotosController : ControllerBase
    {
        private readonly APIContext _context;

        public VotosController(APIContext context)
        {
            _context = context;
        }

        // GET: api/Votos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Voto>>> GetVoto()
        {
            return await _context.Voto.ToListAsync();
        }

        // GET: api/Votos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Voto>> GetVoto(int id)
        {
            var voto = await _context.Voto.FindAsync(id);

            if (voto == null)
            {
                return NotFound();
            }

            return voto;
        }

        // PUT: api/Votos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVoto(int id, Voto voto)
        {
            if (id != voto.IdVoto)
            {
                return BadRequest();
            }

            _context.Entry(voto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VotoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Votos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Voto>> PostVoto(Voto voto)
        {
            _context.Voto.Add(voto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVoto", new { id = voto.IdVoto }, voto);
        }

        // DELETE: api/Votos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVoto(int id)
        {
            var voto = await _context.Voto.FindAsync(id);
            if (voto == null)
            {
                return NotFound();
            }

            _context.Voto.Remove(voto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VotoExists(int id)
        {
            return _context.Voto.Any(e => e.IdVoto == id);
        }
    }
}
