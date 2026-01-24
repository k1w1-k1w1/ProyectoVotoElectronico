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
    public class UsuariosController : ControllerBase
    {
        private readonly APIContext _context;

        public UsuariosController(APIContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios
                .Include(u => u.Rol) 
                .AsNoTracking()
                .ToListAsync();
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.IdUsuario == id);

            if (usuario == null) return NotFound();

            return usuario;
        }

        // POST: api/Usuarios
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Cedula == usuario.Cedula))
            {
                return BadRequest("La cédula ya se encuentra registrada.");
            }

            var rolExiste = await _context.Roles.AnyAsync(r => r.IdRol == usuario.IdRol);
            if (!rolExiste) return BadRequest("El rol especificado no existe.");

            usuario.FechaRegistro = DateTime.UtcNow;
            usuario.Estado = true;

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.IdUsuario }, usuario);
        }

        // PUT: api/Usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.IdUsuario) return BadRequest();

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.IdUsuario == id);
        }
    }
}