namespace ProyectoVotoElectronico.DTOs
{
    public class UsuarioCreateDto
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public int IdRol { get; set; }
        public bool Estado { get; set; }
    }
}
