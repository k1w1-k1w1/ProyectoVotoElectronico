using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoVotoElectronico;

namespace voto.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Administrador")] 
    public class EleccionesController : ControllerBase
    {
        private readonly APIContext _context;

        public EleccionesController(APIContext context)
        {
            _context = context;
        }

        // GET: api/Elecciones
        [HttpGet]
        [AllowAnonymous] 
        public async Task<ActionResult<IEnumerable<Eleccion>>> GetElecciones()
        {
            var elecciones = await _context.Elecciones.ToListAsync();

            foreach (var e in elecciones)
            {
                ActualizarEstadoEleccion(e);
            }

            return elecciones;
        }

        // GET: api/Elecciones/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Eleccion>> GetEleccion(int id)
        {
            var eleccion = await _context.Elecciones
                .Include(e => e.Candidatos)
                .FirstOrDefaultAsync(e => e.IdEleccion == id);

            if (eleccion == null) return NotFound();

            ActualizarEstadoEleccion(eleccion);
            return eleccion;
        }

        // POST: api/Elecciones
        [HttpPost]
        public async Task<ActionResult<Eleccion>> PostEleccion(Eleccion eleccion)
        {
            if (eleccion.FechaInicio >= eleccion.FechaFin)
            {
                return BadRequest("La fecha de inicio debe ser anterior a la fecha de fin.");
            }

            eleccion.Estado = "CREADA"; 
            _context.Elecciones.Add(eleccion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEleccion", new { id = eleccion.IdEleccion }, eleccion);
        }

        // PUT: api/Elecciones/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEleccion(int id, Eleccion eleccion)
        {
            if (id != eleccion.IdEleccion) return BadRequest();

            var eleccionExistente = await _context.Elecciones.AsNoTracking().FirstOrDefaultAsync(x => x.IdEleccion == id);
            if (eleccionExistente.Estado == "ABIERTA" || eleccionExistente.Estado == "CERRADA")
            {
                return BadRequest("No se puede modificar una elección que ya ha iniciado o finalizado.");
            }

            _context.Entry(eleccion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EleccionExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        // DELETE: api/Elecciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEleccion(int id)
        {
            var eleccion = await _context.Elecciones.FindAsync(id);
            if (eleccion == null) return NotFound();

            bool tieneVotos = await _context.Votos.AnyAsync(v => v.IdEleccion == id);
            if (tieneVotos)
            {
                return BadRequest("No se puede eliminar una elección que ya registra sufragios.");
            }

            _context.Elecciones.Remove(eleccion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private void ActualizarEstadoEleccion(Eleccion e)
        {
            var ahora = DateTime.UtcNow;
            if (ahora >= e.FechaInicio && ahora <= e.FechaFin)
                e.Estado = "ABIERTA";
            else if (ahora > e.FechaFin)
                e.Estado = "CERRADA";

            _context.Entry(e).State = EntityState.Modified;
            _context.SaveChanges();
        }

        private bool EleccionExists(int id)
        {
            return _context.Elecciones.Any(e => e.IdEleccion == id);
        }
    }
}