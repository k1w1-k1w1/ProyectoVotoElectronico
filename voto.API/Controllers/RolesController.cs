using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using voto;
using ProyectoVotoElectronico;

[ApiController]
[Route("api/roles")]
public class RolesController : ControllerBase
{
    private readonly APIContext _context;

    public RolesController(APIContext context)
    {
        _context = context;
    }

    [HttpPost("crear-iniciales")]
    public async Task<IActionResult> CrearRolesIniciales()
    {
        bool existen = await _context.Roles.AnyAsync();

        if (existen)
            return BadRequest("Los roles ya fueron creados.");

        var roles = new List<Rol>
    {
        new Rol
        {
            Nombre = "ADMIN",
            Descripcion = "Administrador del sistema"
        },
        new Rol
        {
            Nombre = "VOTANTE",
            Descripcion = "Usuario habilitado para votar"
        },
        new Rol
        {
            Nombre = "CANDIDATO",
            Descripcion = "Candidato participante en la elección"
        }
    };

        _context.Roles.AddRange(roles);
        await _context.SaveChangesAsync();

        return Ok("Roles ADMIN, VOTANTE y CANDIDATO creados correctamente.");
    }
}
