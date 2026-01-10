using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using voto;
using ProyectoVotoElectronico;

[ApiController]
[Route("api/listas")]
public class ListasPoliticasController : ControllerBase
{
    private readonly APIContext _context;

    public ListasPoliticasController(APIContext context)
    {
        _context = context;
    }

    // POST: api/listas
    [HttpPost]
    public async Task<IActionResult> Crear(ListaPolitica lista)
    {
        // 1️⃣ Verificar elección
        var eleccion = await _context.Elecciones
            .FirstOrDefaultAsync(e => e.IdEleccion == lista.EleccionId);

        if (eleccion == null)
            return BadRequest("La elección no existe.");

        // 2️⃣ Validar tipo de elección
        if (eleccion.Tipo != "PLANCHA")
            return BadRequest("Solo se pueden registrar listas en elecciones tipo PLANCHA.");

        _context.ListasPoliticas.Add(lista);
        await _context.SaveChangesAsync();

        return Ok(lista);
    }

    // GET: api/listas/eleccion/5
    [HttpGet("eleccion/{idEleccion}")]
    public async Task<IActionResult> ObtenerPorEleccion(int idEleccion)
    {
        var listas = await _context.ListasPoliticas
            .Where(l => l.EleccionId == idEleccion)
            .ToListAsync();

        return Ok(listas);
    }
}
