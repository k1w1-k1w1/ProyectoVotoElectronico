namespace ProyectoVotoElectronico.DTOs
{
    public class UsuarioCreateDto
    {
        public string Cedula { get; set; }
        public string IdentityUserId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public int IdRol { get; set; }
        public bool Estado { get; set; }
        public int Edad { get; set; }
        public string Ciudad { get; set; }
    }
}
