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

            lista.Eleccion = null;

            _context.ListasPoliticas.Add(lista);
            await _context.SaveChangesAsync();

            var resultado = await _context.ListasPoliticas
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Idlista == lista.Idlista);

            return Ok(resultado);
        }

        [HttpGet]
        public async Task<IActionResult> ListarTodas()
        {
            var listas = await _context.ListasPoliticas
                .Include(l => l.Candidatos) 
                .AsNoTracking()
                .ToListAsync();

            return Ok(listas);
        }

        [HttpGet("eleccion/{idEleccion}")]
        public async Task<IActionResult> ObtenerPorEleccion(int idEleccion)
        {
            var listas = await _context.ListasPoliticas
                .Where(l => l.EleccionId == idEleccion)
                .Select(l => new {
                    l.Idlista,
                    l.NombreLista,
                    l.Descripcion,
                    l.UrlLogo,
                    l.EleccionId,

                    Candidatos = _context.Candidatos
                        .Where(c => c.IdLista == l.Idlista)
                        .Select(c => new {
                            c.IdCandidato,
                            c.Nombre,
                            c.Apellido,
                            c.FotoUrl,
                            c.Cargo
                        }).ToList()
                })
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