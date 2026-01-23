using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoVotoElectronico;
using voto;
using BCrypt.Net;

[ApiController]
[Route("api/usuarios")]
public class UsuariosController : ControllerBase
{
    private readonly APIContext _context;
    private readonly EmailService _emailService;

    public UsuariosController(APIContext context, EmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    // DTOs

    public class RegistroUsuarioDto
    {
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginRequestDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class Verify2FADTO
    {
        public string Email { get; set; }
        public string Codigo { get; set; }
    }

    // REGISTRO

    [HttpPost("registro")]
    public async Task<IActionResult> Registrar(RegistroUsuarioDto dto)
    {
        bool existe = await _context.Usuarios
            .AnyAsync(u => u.Email == dto.Email);

        if (existe)
            return BadRequest("El correo ya está registrado.");

        var rolVotante = await _context.Roles
            .FirstOrDefaultAsync(r => r.Nombre == "VOTANTE");

        if (rolVotante == null)
            return BadRequest("No existe el rol VOTANTE.");

        string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

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

    // LOGIN

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDTO dto)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (usuario == null)
            return Unauthorized("Credenciales incorrectas");

        bool passwordValido = BCrypt.Net.BCrypt.Verify(
            dto.Password,
            usuario.Password
        );

        if (!passwordValido)
            return Unauthorized("Credenciales incorrectas");

        string codigo = new Random().Next(100000, 999999).ToString();

        usuario.Codigo2FA = codigo;
        usuario.CodigoExpira = DateTime.UtcNow.AddMinutes(5);

        await _context.SaveChangesAsync();

        _emailService.EnviarCodigo(usuario.Email, codigo);

        return Ok("Código de verificación enviado al correo.");
    }

    // VERIFICAR 2FA

    [HttpPost("verificar-2fa")]
    public async Task<IActionResult> Verificar2FA(Verify2FADTO dto)
    {
        var usuario = await _context.Usuarios
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (usuario == null)
            return Unauthorized("Usuario no encontrado.");

        if (usuario.Codigo2FA != dto.Codigo ||
            usuario.CodigoExpira < DateTime.UtcNow)
        {
            return Unauthorized("Código inválido o expirado.");
        }

        // Limpiar código 2FA
        usuario.Codigo2FA = null;
        usuario.CodigoExpira = null;
        await _context.SaveChangesAsync();

        //JWT
        return Ok(new
        {
            mensaje = "Login exitoso",
            usuario = new
            {
                usuario.IdUsuario,
                usuario.Nombre,
                usuario.Apellido,
                Rol = usuario.Rol.Nombre
            }
        });
    }
}
