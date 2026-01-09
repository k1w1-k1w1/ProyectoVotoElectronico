namespace ProyectoVotoElectronico
{
    public class Usuario
    {
        public Guid IdUsuario { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; }

        // Relaciones
        public int IdRol { get; set; }
        public Rol Rol { get; set; }

    }
}
