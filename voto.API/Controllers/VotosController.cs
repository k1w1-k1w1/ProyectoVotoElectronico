using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoVotoElectronico;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
                    FechaHora = DateTime.Now
                };
                _context.RegistroVotaciones.Add(registro);

                string semilla = $"{request.IdEleccion}-{request.IdCandidato ?? request.IdLista}-{Guid.NewGuid()}";
                string hashInmutable = CalcularHash(semilla);

                var nuevoVoto = new Voto
                {
                    IdEleccion = request.IdEleccion,
                    IdCandidato = request.IdCandidato,
                    IdLista = request.IdLista,
                    FechaHora = DateTime.UtcNow,
                    HashVoto = hashInmutable 
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


        [HttpGet("YaVoto/{idUsuario}/{idEleccion}")]
        public async Task<IActionResult> VerificarVoto(int idUsuario, int idEleccion)
        {
            var existe = await _context.RegistroVotaciones
                                .AnyAsync(v => v.IdUsuario == idUsuario && v.IdEleccion == idEleccion);
            return Ok(existe);
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

        [HttpGet("Resultados/{idEleccion}")]
        public async Task<IActionResult> ObtenerResultados(int idEleccion)
        {
            var resultados = await _context.Votos
                .Where(v => v.IdEleccion == idEleccion)
                .GroupBy(v => v.IdLista)
                .Select(g => new
                {
                    NombreLista = _context.ListasPoliticas
                                    .Where(l => l.Idlista == g.Key)
                                    .Select(l => l.NombreLista)
                                    .FirstOrDefault() ?? "Voto en Blanco",
                    TotalVotos = g.Count()
                })
                .OrderByDescending(r => r.TotalVotos)
                .ToListAsync();

            return Ok(resultados);
        }

        private List<ResultadoConEscaños> CalcularWebster(List<ResultadoConEscaños> resultados, int escañosDisponibles)
        {
            var cocientes = new List<CocienteCalculado>();

            foreach (var lista in resultados)
            {
                for (int i = 0; i < escañosDisponibles; i++)
                {
                    double divisor = (2 * i) + 1; 
                    cocientes.Add(new CocienteCalculado
                    {
                        NombreLista = lista.NombreLista,
                        Valor = (double)lista.TotalVotos / divisor
                    });
                }
            }

            var listaGanadores = cocientes
                .OrderByDescending(c => c.Valor)
                .Take(escañosDisponibles)
                .Select(c => c.NombreLista)
                .ToList();


            return resultados;
        }



    }


    public class VotoRequest
    {
        public int IdUsuario { get; set; }
        public int IdEleccion { get; set; }
        public int? IdCandidato { get; set; } 
        public int? IdLista { get; set; }    
    }

    public class ResultadoConEscaños
    {
        [JsonPropertyName("nombreLista")] 
        public string NombreLista { get; set; }

        [JsonPropertyName("totalVotos")]
        public int TotalVotos { get; set; }
    }

    public class CocienteCalculado
    {
        public string NombreLista { get; set; }
        public double Valor { get; set; }
    }
}