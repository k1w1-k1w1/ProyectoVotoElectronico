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

            await _context.SaveChangesAsync(); // Guardamos los cambios de estado detectados
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
            await _context.SaveChangesAsync();

            return eleccion;
        }

        // POST: api/Elecciones
        [HttpPost]
        public async Task<ActionResult<Eleccion>> PostEleccion(Eleccion eleccion)
        {
            //if (eleccion.FechaInicio >= eleccion.FechaFin)
            //{
            //    return BadRequest("La fecha de inicio debe ser anterior a la fecha de fin.");
            //}

            // Calculamos el estado basándonos en la fecha que viene del MVC
            CalcularEstadoSinGuardar(eleccion);


            Console.WriteLine("====== DEBUG ELECCION ======");
            Console.WriteLine("Servidor UTC Now: " + DateTime.UtcNow);
            Console.WriteLine("FechaInicio RAW: " + eleccion.FechaInicio);
            Console.WriteLine("FechaFin RAW: " + eleccion.FechaFin);
            Console.WriteLine("FechaInicio UTC: " + eleccion.FechaInicio.ToUniversalTime());
            Console.WriteLine("FechaFin UTC: " + eleccion.FechaFin.ToUniversalTime());
            Console.WriteLine("============================");




            _context.Elecciones.Add(eleccion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEleccion", new { id = eleccion.IdEleccion }, eleccion);
        }

        // POST: api/Elecciones/Cerrar/5
        [HttpPost("Cerrar/{id}")]
        public async Task<IActionResult> CerrarManual(int id)
        {
            var eleccion = await _context.Elecciones.FindAsync(id);
            if (eleccion == null) return NotFound();

            eleccion.Estado = "CERRADA";
            eleccion.FechaFin = DateTime.Now;

            _context.Entry(eleccion).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Elección cerrada exitosamente." });
        }

        // PUT: api/Elecciones/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEleccion(int id, Eleccion eleccion)
        {
            if (id != eleccion.IdEleccion) return BadRequest();

            var eleccionExistente = await _context.Elecciones.AsNoTracking().FirstOrDefaultAsync(x => x.IdEleccion == id);
            if (eleccionExistente == null) return NotFound();

            if (eleccionExistente.Estado == "ABIERTA" || eleccionExistente.Estado == "CERRADA")
            {
                return BadRequest("No se puede modificar una elección activa o finalizada.");
            }

            _context.Entry(eleccion).State = EntityState.Modified;
            await _context.SaveChangesAsync();

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
                return BadRequest("No se puede eliminar una elección con votos registrados.");
            }

            _context.Elecciones.Remove(eleccion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // --- MÉTODOS DE APOYO ---

        private void ActualizarEstadoEleccion(Eleccion e)
        {
            if (e.Estado == "CERRADA") return;

            // Esto convierte la hora de Render (01:49 AM) a la hora de Ecuador (08:49 PM)
            var ecuadorZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
            var ahora = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, ecuadorZone);

            if (ahora >= e.FechaInicio && ahora <= e.FechaFin)
                e.Estado = "ABIERTA";
            else if (ahora > e.FechaFin)
                e.Estado = "CERRADA";
            else
                e.Estado = "PROGRAMADA";

            _context.Entry(e).State = EntityState.Modified;
        }




        private void CalcularEstadoSinGuardar(Eleccion e)
        {
            var ecuadorZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
            var ahora = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, ecuadorZone);

            if (ahora >= e.FechaInicio && ahora <= e.FechaFin)
                e.Estado = "ABIERTA";
            else if (ahora > e.FechaFin)
                e.Estado = "CERRADA";
            else
                e.Estado = "PROGRAMADA";
        }




        private bool EleccionExists(int id)
        {
            return _context.Elecciones.Any(e => e.IdEleccion == id);
        }
    }
}