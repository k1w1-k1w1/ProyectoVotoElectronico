using ProyectoVotoElectronico.DTOs;
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

            if (usuario == null)
                return NotFound();

            return usuario;
        }

        // GET: api/Usuarios/ByEmail/test@mail.com

        [HttpGet("ByEmail/{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }


        // POST: api/Usuarios
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(UsuarioCreateDto dto)
        {
            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Email = dto.Email,
                IdRol = dto.IdRol,
                Estado = dto.Estado,
                FechaRegistro = System.DateTime.Now
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetUsuario),
                new { id = usuario.IdUsuario },
                usuario
            );
        }

 
        // PUT: api/Usuarios/5

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, UsuarioUpdateDto dto)
        {
            if (id != dto.IdUsuario)
                return BadRequest("El id no coincide.");

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound();

            usuario.Nombre = dto.Nombre;
            usuario.Apellido = dto.Apellido;
            usuario.Email = dto.Email;
            usuario.IdRol = dto.IdRol;
            usuario.Estado = dto.Estado;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Usuarios/5

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound();

            usuario.Estado = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
