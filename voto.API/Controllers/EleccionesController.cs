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
    public class EleccionesController : ControllerBase
    {
        private readonly APIContext _context;

        public EleccionesController(APIContext context)
        {
            _context = context;
        }

        // GET: api/Elecciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Eleccion>>> GetEleccion()
        {
            return await _context.Eleccion.ToListAsync();
        }

        // GET: api/Elecciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Eleccion>> GetEleccion(int id)
        {
            var eleccion = await _context.Eleccion.FindAsync(id);

            if (eleccion == null)
            {
                return NotFound();
            }

            return eleccion;
        }

        // PUT: api/Elecciones/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEleccion(int id, Eleccion eleccion)
        {
            if (id != eleccion.IdEleccion)
            {
                return BadRequest();
            }

            _context.Entry(eleccion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EleccionExists(id))
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

        // POST: api/Elecciones
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Eleccion>> PostEleccion(Eleccion eleccion)
        {
            _context.Eleccion.Add(eleccion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEleccion", new { id = eleccion.IdEleccion }, eleccion);
        }

        // DELETE: api/Elecciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEleccion(int id)
        {
            var eleccion = await _context.Eleccion.FindAsync(id);
            if (eleccion == null)
            {
                return NotFound();
            }

            _context.Eleccion.Remove(eleccion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EleccionExists(int id)
        {
            return _context.Eleccion.Any(e => e.IdEleccion == id);
        }
    }
}
