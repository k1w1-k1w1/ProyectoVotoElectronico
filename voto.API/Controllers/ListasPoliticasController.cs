using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProyectoVotoElectronico;

namespace voto.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize(Roles = "Administrador")] 
    public class ListasPoliticasController : ControllerBase
    {
        private readonly APIContext _context;

        public ListasPoliticasController(APIContext context)
        {
            _context = context;
        }

        // POST: api/ListasPoliticas
        [HttpPost]
        public async Task<IActionResult> Crear(ListaPolitica lista)
        {
            var eleccion = await _context.Elecciones
                .FirstOrDefaultAsync(e => e.IdEleccion == lista.EleccionId);

            if (eleccion == null)
                return BadRequest("La elección no existe.");

            if (eleccion.Tipo.ToUpper() != "PLANCHA")
                return BadRequest("Solo se pueden registrar listas en elecciones tipo PLANCHA.");

            if (eleccion.Estado != "CREADA")
                return BadRequest("No se pueden añadir listas a una elección que ya inició o finalizó.");

            _context.ListasPoliticas.Add(lista);
            await _context.SaveChangesAsync();

            return Ok(lista);
        }

        // GET: api/ListasPoliticas/eleccion/5
        [HttpGet("eleccion/{idEleccion}")]
        public async Task<IActionResult> ObtenerPorEleccion(int idEleccion)
        {
            var listas = await _context.ListasPoliticas
                .Include(l => l.Candidatos)
                .Where(l => l.EleccionId == idEleccion)
                .AsNoTracking()
                .ToListAsync();

            return Ok(listas);
        }

        // DELETE: api/ListasPoliticas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var lista = await _context.ListasPoliticas.FindAsync(id);
            if (lista == null) return NotFound();

            var tieneVotos = await _context.Votos.AnyAsync(v => v.IdLista == id);
            if (tieneVotos)
                return BadRequest("No se puede eliminar una lista que ya tiene votos registrados.");

            _context.ListasPoliticas.Remove(lista);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}