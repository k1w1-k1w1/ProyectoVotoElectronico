using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoVotoElectronico;
using voto;
using System.Security.Cryptography;
using System.Text;

[ApiController]
[Route("api/usuarios")]
public class UsuariosController : ControllerBase
{
    private readonly APIContext _context;

    public UsuariosController(APIContext context)
    {
        _context = context;
    }

    // 🔹 DTO DENTRO DEL CONTROLLER
    public class RegistroUsuarioDto
    {
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    // 🔹 REGISTRO DE USUARIO
    [HttpPost("registro")]
    public async Task<IActionResult> Registrar(RegistroUsuarioDto dto)
    {
        // 1️⃣ Validar email único
        bool existe = await _context.Usuarios
            .AnyAsync(u => u.Email == dto.Email);

        if (existe)
            return BadRequest("El correo ya está registrado.");

        // 2️⃣ Obtener rol VOTANTE
        var rolVotante = await _context.Roles
            .FirstOrDefaultAsync(r => r.Nombre == "VOTANTE");

        if (rolVotante == null)
            return BadRequest("No existe el rol VOTANTE.");

        // 3️⃣ Hash de contraseña
        using var sha = SHA256.Create();
        var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));
        string passwordHash = Convert.ToBase64String(hash);

        // 4️⃣ Crear usuario
        var usuario = new Usuario
        {
            Cedula = dto.Cedula,
            Nombre = dto.Nombre,
            Apellido = dto.Apellido,
            Email = dto.Email,
            Password = passwordHash,
            Estado = true,
            FechaRegistro = DateTime.UtcNow,
            IdRol = rolVotante.IdRol
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return Ok("Usuario registrado correctamente.");
    }
}
