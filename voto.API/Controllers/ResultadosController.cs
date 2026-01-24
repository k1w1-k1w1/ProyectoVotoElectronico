using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProyectoVotoElectronico;

namespace voto.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResultadosController : ControllerBase
    {
        private readonly APIContext _context;

        public ResultadosController(APIContext context)
        {
            _context = context;
        }

        // GET: api/Resultados/Reporte/5
        [HttpGet("Reporte/{idEleccion}")]
        public async Task<IActionResult> GetReporteFinal(int idEleccion)
        {
            var eleccion = await _context.Elecciones
                .FirstOrDefaultAsync(e => e.IdEleccion == idEleccion);

            if (eleccion == null) return NotFound("Elección no encontrada.");

            var votosCandidatos = await _context.Votos
                .Where(v => v.IdEleccion == idEleccion && v.IdCandidato != null)
                .GroupBy(v => v.IdCandidato)
                .Select(g => new {
                    Id = g.Key,
                    TotalVotos = g.Count()
                }).ToListAsync();

            var votosListas = await _context.Votos
                .Where(v => v.IdEleccion == idEleccion && v.IdLista != null)
                .GroupBy(v => v.IdLista)
                .Select(g => new {
                    Id = g.Key,
                    TotalVotos = g.Count()
                }).ToListAsync();

            return Ok(new
            {
                eleccion = eleccion.Nombre,
                estado = eleccion.Estado,
                votosPorCandidato = votosCandidatos,
                votosPorLista = votosListas,
                fechaReporte = DateTime.UtcNow
            });
        }

        // POST: api/Resultados/CerrarEleccion/5
        [HttpPost("CerrarEleccion/{idEleccion}")]
        public async Task<IActionResult> FinalizarProceso(int idEleccion)
        {
            var eleccion = await _context.Elecciones.FindAsync(idEleccion);
            if (eleccion == null) return NotFound();

            if (eleccion.Estado != "ABIERTA")
                return BadRequest("Solo se pueden cerrar elecciones que estén en curso.");

            eleccion.Estado = "CERRADA";
            eleccion.FechaFin = DateTime.UtcNow;

            var totalVotos = await _context.Votos.CountAsync(v => v.IdEleccion == idEleccion);

            var resultado = new Resultado
            {
                IdEleccion = idEleccion,
                TotalVotos = totalVotos,
                MetodoAsignacion = eleccion.Tipo, 
                FechaCalculo = DateTime.UtcNow
            };

            _context.Resultados.Add(resultado);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Elección cerrada y resultados procesados.", resultado });
        }
    }
}