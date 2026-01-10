using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using voto;
using ProyectoVotoElectronico;

[ApiController]
[Route("api/votos")]
public class VotosController : ControllerBase
{
    private readonly APIContext _context;

    public VotosController(APIContext context)
    {
        _context = context;
    }

    // DTO para emitir voto
    public class VotoRequestDto
    {
        public int IdUsuario { get; set; }
        public int IdEleccion { get; set; }
        public int IdCandidato { get; set; }
    }

    [HttpPost]
    public async Task<IActionResult> EmitirVoto([FromBody] VotoRequestDto dto)
    {
        // 1️⃣ Verificar elección
        var eleccion = await _context.Elecciones
            .FirstOrDefaultAsync(e => e.IdEleccion == dto.IdEleccion);

        if (eleccion == null)
            return BadRequest("La elección no existe.");

        if (eleccion.Estado != "ABIERTA")
            return BadRequest("La elección no está abierta.");

        // 2️⃣ Verificar si el usuario ya votó
        bool yaVoto = await _context.Set<RegistroVotacion>()
            .AnyAsync(r => r.IdUsuario == dto.IdUsuario
                        && r.IdEleccion == dto.IdEleccion);

        if (yaVoto)
            return BadRequest("El usuario ya ha votado en esta elección.");

        // 3️⃣ Verificar candidato
        var candidato = await _context.Candidatos
            .FirstOrDefaultAsync(c => c.IdCandidato == dto.IdCandidato
                                   && c.IdEleccion == dto.IdEleccion);

        if (candidato == null)
            return BadRequest("El candidato no pertenece a esta elección.");

        // 4️⃣ Crear voto (ANÓNIMO)
        var voto = new Voto
        {
            IdEleccion = dto.IdEleccion,
            IdCandidato = dto.IdCandidato,
            FechaHora = DateTime.UtcNow,
            HashVoto = Guid.NewGuid().ToString() // hash ≠ usuario
        };

        // 5️⃣ Registrar auditoría (NO anónimo)
        var registro = new RegistroVotacion
        {
            IdUsuario = dto.IdUsuario,
            IdEleccion = dto.IdEleccion,
            FechaHora = DateTime.UtcNow
        };

        _context.Votos.Add(voto);
        _context.Set<RegistroVotacion>().Add(registro);

        await _context.SaveChangesAsync();

        return Ok("Voto registrado correctamente.");
    }
}
