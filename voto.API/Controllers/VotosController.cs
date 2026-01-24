using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ProyectoVotoElectronico;

namespace voto.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VotosController : ControllerBase
    {
        private readonly APIContext _context;

        public VotosController(APIContext context)
        {
            _context = context;
        }

        // POST: api/Votos/EmitirVoto
        [HttpPost("EmitirVoto")]
        public async Task<IActionResult> EmitirVoto(VotoRequest request)
        {
            var eleccion = await _context.Elecciones.FindAsync(request.IdEleccion);
            if (eleccion == null || eleccion.Estado != "ABIERTA")
                return BadRequest("La elección no está activa.");

            var yaVoto = await _context.RegistroVotaciones
                .AnyAsync(r => r.IdUsuario == request.IdUsuario && r.IdEleccion == request.IdEleccion);

            if (yaVoto) return BadRequest("El usuario ya ejerció su voto en esta elección.");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var registro = new RegistroVotacion
                {
                    IdUsuario = request.IdUsuario,
                    IdEleccion = request.IdEleccion,
                    FechaHora = DateTime.UtcNow
                };
                _context.RegistroVotaciones.Add(registro);

                // Crear el Voto Anónimo con Hash de Inmutabilidad
                string semilla = $"{request.IdEleccion}-{request.IdCandidato ?? request.IdLista}-{Guid.NewGuid()}";
                string hashInmutable = CalcularHash(semilla);

                var nuevoVoto = new Voto
                {
                    IdEleccion = request.IdEleccion,
                    IdCandidato = request.IdCandidato,
                    IdLista = request.IdLista,
                    FechaHora = DateTime.UtcNow,
                    HashVoto = hashInmutable // Requisito de seguridad
                };
                _context.Votos.Add(nuevoVoto);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { mensaje = "Voto procesado con éxito", comprobante = hashInmutable });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                var mensajeError = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, $"Error real: {mensajeError}");
            }
        }

        private string CalcularHash(string texto)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(texto));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes) builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }
    }

    public class VotoRequest
    {
        public int IdUsuario { get; set; }
        public int IdEleccion { get; set; }
        public int? IdCandidato { get; set; } 
        public int? IdLista { get; set; }    
    }
}