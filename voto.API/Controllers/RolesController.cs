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
    public class RolesController : ControllerBase
    {
        private readonly APIContext _context;

        public RolesController(APIContext context)
        {
            _context = context;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rol>>> GetRoles()
        {
            return await _context.Roles.ToListAsync();
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Rol>> GetRol(int id)
        {
            var rol = await _context.Roles.FindAsync(id);

            if (rol == null) return NotFound();

            return rol;
        }

        // POST: api/Roles
        [HttpPost]
        public async Task<ActionResult<Rol>> PostRol(Rol rol)
        {
            if (await _context.Roles.AnyAsync(r => r.Nombre == rol.Nombre))
            {
                return BadRequest("El nombre del rol ya existe.");
            }

            _context.Roles.Add(rol);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRol), new { id = rol.IdRol }, rol);
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRol(int id)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null) return NotFound();

            var tieneUsuarios = await _context.Usuarios.AnyAsync(u => u.IdRol == id);
            if (tieneUsuarios)
            {
                return BadRequest("No se puede eliminar un rol que tiene usuarios asociados.");
            }

            _context.Roles.Remove(rol);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RolExists(int id)
        {
            return _context.Roles.Any(e => e.IdRol == id);
        }
    }
}